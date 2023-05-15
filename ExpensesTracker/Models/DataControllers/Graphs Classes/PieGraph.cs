using ExpensesTracker.DataTypes;
using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.Models.Settings;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpensesTracker.Models.DataControllers.Graphs_Classes
{
  internal class PieGraph : GraphWPF
  {
    public PieGraph(GraphSettings settings, List<Expense>? data, WpfPlot barPlot) : base(settings, data, barPlot) { }

    protected override void PlotGraph()
    {
      _plot.Plot.Clear();
      SetTitle();
      _xAxisValues = CalculateXAxisIntervals();
      _yAxisValues = CalculateYAxisValues();
      AddPieGraph();
      _plot.Plot.Legend(true, Alignment.UpperLeft);
      _plot.Refresh();
    }

    protected override List<DateRange> CalculateXAxisIntervals()
    {
      var currentDate = DateTime.Today;
      var rangesToReturn = new List<DateRange>();
      var iterationDate = currentDate;

      switch (_settings.TimeScope)
      {
        //This pattern repeats in all cases
        case TimeRanges.week:

          //Determine DateRange
          do { iterationDate = iterationDate.AddDays(-1); }
          while (iterationDate.DayOfWeek != DayOfWeek.Sunday);
          rangesToReturn.Add(new DateRange(iterationDate.AddDays(1), currentDate));
          break;

        case TimeRanges.weekExact:
          rangesToReturn.Add(new DateRange(currentDate.AddDays(-6), currentDate));
          break;

        case TimeRanges.month:
          do { iterationDate = iterationDate.AddDays(-1); }
          while (iterationDate.Month == currentDate.Month);
          rangesToReturn.Add(new DateRange(iterationDate.AddDays(1), currentDate));
          break;

        case TimeRanges.monthExact:
          rangesToReturn.Add(new DateRange(currentDate.AddDays(-29), currentDate));
          break;

        case TimeRanges.year:
          do { iterationDate = iterationDate.AddDays(-1); }
          while (iterationDate.Year == currentDate.Year);
          rangesToReturn.Add(new DateRange(iterationDate.AddDays(1), currentDate));
          break;

        case TimeRanges.yearExact:
          rangesToReturn.Add(new DateRange(currentDate.AddDays(-364), currentDate));
          break;

        case TimeRanges.custom:
          if (_settings.UserTimeScope != null && _settings.TimeDivisor != null) rangesToReturn.Add(_settings.UserTimeScope);
          break;
      }
      return rangesToReturn;
    }

    protected override double?[,] CalculateYAxisValues()
    {
      double?[,] data = new double?[0, 0];
      var legendLabels = Array.Empty<string>();

      //Calculate values based on chosen scope
      switch (_settings.ValuesScope)
      {
        //This pattern repeats in all cases
        case ValuesScopes.Balance:

          //Initialize array of proper dimension sizes
          data = new double?[_xAxisValues.Count, 2];
          //Get legend strings
          legendLabels = new string[2] { "Income", "Expense" };
          //Iterate through ranges applying boundary conditions to get sum of Record.Total
          for (int i = 0; i < _xAxisValues.Count; i++)
          {
            var range = _xAxisValues[i];

            var incomeValue = (from income in Data
                               where income.Income == true
                               where (income.Date.HasValue && income.Date >= range.StartDate && income.Date < range.EndDate) ||
                               (!income.Date.HasValue && income.DateOfEntry >= range.StartDate && income.DateOfEntry < range.EndDate)
                               where income.Total != null
                               select income.Total).Sum() ?? 0;
            var expenseValue = (from income in Data
                                where income.Income != true
                                where (income.Date.HasValue && income.Date >= range.StartDate && income.Date < range.EndDate) ||
                                (!income.Date.HasValue && income.DateOfEntry >= range.StartDate && income.DateOfEntry < range.EndDate)
                                where income.Total != null
                                select income.Total).Sum() ?? 0;

            data[i, 0] = (double)incomeValue;
            data[i, 1] = (double)expenseValue;
          }
          break;

        case ValuesScopes.Categories:

          var categories = from category in Data
                           group category by category.CategoryId into categoryGroups
                           select categoryGroups.Key;

          data = new double?[_xAxisValues.Count, categories.Count()];
          legendLabels = DatabaseModel.GetCategoriesNames(categories.ToList()).ToArray();

          for (int i = 0; i < _xAxisValues.Count; i++)
          {
            var range = _xAxisValues[i];
            var categoryValues = new List<decimal>();

            foreach (var category in categories)
            {
              var categoryValue = (from cat in Data
                                   where cat.CategoryId == category
                                   where (cat.Date.HasValue && cat.Date >= range.StartDate && cat.Date < range.EndDate) ||
                                   (!cat.Date.HasValue && cat.DateOfEntry >= range.StartDate && cat.DateOfEntry < range.EndDate)
                                   select cat.Total).Sum() ?? 0M;
              categoryValues.Add(categoryValue);
            }

            for (int j = 0; j < categoryValues.Count; j++) data[i, j] = (double)(categoryValues.ToArray()[j]);
          }
          break;

        case ValuesScopes.Recurring:

          var recurrences = from rec in Data
                            where rec.Recurring == true
                            group rec by rec.RecurringId into recurringGroups
                            select recurringGroups.Key;

          data = new double?[_xAxisValues.Count, recurrences.Count()];
          legendLabels = DatabaseModel.GetRecurringNames(recurrences.ToList()).ToArray();

          for (int i = 0; i < _xAxisValues.Count; i++)
          {
            var range = _xAxisValues[i];
            var recurrencesValues = new List<decimal>();

            foreach (var recurrence in recurrences)
            {
              var recurrenceValue = (from rec in Data
                                     where rec.RecurringId == recurrence
                                     where (rec.Date.HasValue && rec.Date >= range.StartDate && rec.Date < range.EndDate) ||
                                     (!rec.Date.HasValue && rec.DateOfEntry >= range.StartDate && rec.DateOfEntry < range.EndDate)
                                     select rec.Total).Sum() ?? 0M;
              recurrencesValues.Add(recurrenceValue);
            }

            for (int j = 0; j < recurrencesValues.Count; j++) data[i, j] = (double)(recurrencesValues.ToArray()[j]);
          }
          break;

        case ValuesScopes.BalanceRecurring:

          data = new double?[_xAxisValues.Count, 2];
          legendLabels = new string[2] { "Recurring", "Nonrecurring" };

          for (int i = 0; i < _xAxisValues.Count; i++)
          {
            var range = _xAxisValues[i];

            var recValue = (from rec in Data
                            where rec.Recurring == true
                            where (rec.Date.HasValue && rec.Date >= range.StartDate && rec.Date < range.EndDate) ||
                            (!rec.Date.HasValue && rec.DateOfEntry >= range.StartDate && rec.DateOfEntry < range.EndDate)
                            where rec.Total != null
                            select rec.Total).Sum() ?? 0M;
            var nonRecValue = (from rec in Data
                               where rec.Recurring != true
                               where (rec.Date.HasValue && rec.Date >= range.StartDate && rec.Date < range.EndDate) ||
                               (!rec.Date.HasValue && rec.DateOfEntry >= range.StartDate && rec.DateOfEntry < range.EndDate)
                               where rec.Total != null
                               select rec.Total).Sum() ?? 0M;

            data[i, 0] = (double)recValue;
            data[i, 1] = (double)nonRecValue;
          }
          break;

        case ValuesScopes.Subcategories:

          var subCategories = from subCat in Data
                              where subCat.SubcategoryId != null
                              group subCat by subCat.SubcategoryId into subCatGroups
                              select subCatGroups.Key;

          data = new double?[_xAxisValues.Count, subCategories.Count()];
          legendLabels = DatabaseModel.GetSubcategoriesNames(subCategories.ToList()).ToArray();
          for (int i = 0; i < _xAxisValues.Count; i++)
          {
            var range = _xAxisValues[i];
            var subCategoryValues = new List<decimal>();

            foreach (var subCategory in subCategories)
            {
              var subCategoryValue = (from subCat in Data
                                      where subCat.SubcategoryId == subCategory
                                      where (subCat.Date.HasValue && subCat.Date >= range.StartDate && subCat.Date < range.EndDate) ||
                                      (!subCat.Date.HasValue && subCat.DateOfEntry >= range.StartDate && subCat.DateOfEntry < range.EndDate)
                                      select subCat.Total).Sum() ?? 0M;
              subCategoryValues.Add(subCategoryValue);
            }

            for (int j = 0; j < subCategoryValues.Count; j++) data[i, j] = (double)(subCategoryValues.ToArray()[j]);
          }
          break;
      }

      _legendLabels = legendLabels;
      return data;
    }

    private void AddPieGraph()
    {
      //Plot last first, because they are higher
      for (int i = _yAxisValues.GetLength(0) - 1; i >= 0; i--)
      {
        //One row is one data set
        double?[] row = new double?[_yAxisValues.GetLength(1)];

        for (int j = 0; j < _yAxisValues.GetLength(1); j++) row[j] = _yAxisValues[i, j];
        var pie = _plot.Plot.AddPie(row.Select(r => (double)r).ToArray());
        pie.Explode = true;
        if (_settings.ValuesRelativeType) pie.ShowPercentages = true;
        else pie.ShowValues = true;
        pie.Size = .85;
        pie.SliceLabelPosition = 0.55;
        pie.SliceLabelColors = pie.SliceFillColors;
        pie.SliceLabels = _legendLabels;
      }
    }
  }
}
