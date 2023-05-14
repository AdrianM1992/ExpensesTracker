using ExpensesTracker.DataTypes;
using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.DataProviders;
using Microsoft.IdentityModel.Tokens;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpensesTracker.Models.DataControllers.Graphs_Classes
{
  internal class BarGraph : GraphWPF
  {
    private string[] _xAxisLabels = Array.Empty<string>();
    private double[] _xAxisPositions = Array.Empty<double>();
    public BarGraph(GraphSettings settings, List<Expense>? data, WpfPlot barPlot) : base(settings, data, barPlot) { }

    protected override void PlotGraph()
    {
      _plot.Plot.Clear();
      SetTitle();
      _plot.Plot.XAxis.Label(_settings.XAxisName ?? "", size: 16);
      _plot.Plot.YAxis.Label(_settings.YAxisName ?? "", size: 16);
      _xAxisValues = CalculateXAxisIntervals();
      _yAxisValues = CalculateYAxisValues();
      AddGraphBars();
      _plot.Plot.XTicks(_xAxisPositions, _xAxisLabels);
      _plot.Plot.Legend(true, Alignment.UpperLeft);
      _plot.Refresh();
    }

    protected override List<DateRange> CalculateXAxisIntervals()
    {
      var currentDate = DateTime.Today;
      List<string> xAxisLabels = new();
      var rangesToReturn = new List<DateRange>();
      var iterationDate = currentDate;

      switch (_settings.TimeScope)
      {
        //This pattern repeats in all cases
        case TimeRanges.week:

          //Determine DateRange
          do { iterationDate = iterationDate.AddDays(-1); }
          while (iterationDate.DayOfWeek != DayOfWeek.Sunday);
          rangesToReturn = DateIntervalDivider.DivideByDay(new DateRange(iterationDate.AddDays(1), currentDate));

          //Build and format array of xAxis labels
          foreach (var day in rangesToReturn) xAxisLabels.Add(day.StartDate.DayOfWeek.ToString());
          break;

        case TimeRanges.weekExact:
          rangesToReturn = DateIntervalDivider.DivideByDay(new DateRange(currentDate.AddDays(-6), currentDate));
          foreach (var day in rangesToReturn) xAxisLabels.Add(day.StartDate.ToString("dd.MM"));
          break;

        case TimeRanges.month:
          do { iterationDate = iterationDate.AddDays(-1); }
          while (iterationDate.Month == currentDate.Month);
          rangesToReturn = DateIntervalDivider.DivideByWeek(new DateRange(iterationDate.AddDays(1), currentDate));
          int i = 1;
          foreach (var week in rangesToReturn) xAxisLabels.Add($"Week {i++}");
          break;

        case TimeRanges.monthExact:
          rangesToReturn = DateIntervalDivider.DivideByWeek(new DateRange(currentDate.AddDays(-29), currentDate));
          foreach (var week in rangesToReturn) xAxisLabels.Add($"{week.StartDate:dd.MM} - {week.EndDate:dd.MM}");
          break;

        case TimeRanges.year:
          do { iterationDate = iterationDate.AddDays(-1); }
          while (iterationDate.Year == currentDate.Year);
          rangesToReturn = DateIntervalDivider.DivideByMonth(new DateRange(iterationDate.AddDays(1), currentDate));
          foreach (var month in rangesToReturn) xAxisLabels.Add(month.StartDate.Month.ToString());
          break;

        case TimeRanges.yearExact:
          rangesToReturn = DateIntervalDivider.DivideByMonth(new DateRange(currentDate.AddDays(-364), currentDate));
          foreach (var month in rangesToReturn) xAxisLabels.Add(month.StartDate.ToString("MM.yyyy"));
          break;

        case TimeRanges.custom:
          if (_settings.UserTimeScope != null && _settings.TimeDivisor != null)
          {
            rangesToReturn = CalculateCustomXAxisIntervals(_settings.UserTimeScope, (TimeDivisionIntervals)_settings.TimeDivisor);
            xAxisLabels = _xAxisLabels.ToList();
          }
          else xAxisLabels = new();
          break;
      }
      _xAxisLabels = xAxisLabels.ToArray();
      _xAxisPositions = Enumerable.Range(0, rangesToReturn.Count).Select(x => (double)x).ToArray();
      return rangesToReturn;
    }
    /// <summary>
    /// Based on graph type calculates ranges to be shown on graph
    /// </summary>
    /// <param name="dateRange">User defined DateRange</param>
    /// <param name="interval">Division specifier</param>
    /// <returns>List of DateRange</returns>
    private List<DateRange> CalculateCustomXAxisIntervals(DateRange dateRange, TimeDivisionIntervals interval)
    {
      var rangesToReturn = new List<DateRange>();
      var labelsToReturn = new List<string>();
      switch (interval)
      {
        //This pattern repeats in all cases
        case TimeDivisionIntervals.Day:

          //Determine DateRange
          rangesToReturn = DateIntervalDivider.DivideByDay(dateRange);

          //Check if DateRange is more than one year and then build and format accordingly array of xAxis labels 
          if (dateRange.StartDate.Year == dateRange.EndDate.Year) foreach (var day in rangesToReturn) labelsToReturn.Add(day.StartDate.ToString("dd.MM"));
          else foreach (var day in rangesToReturn) labelsToReturn.Add(day.StartDate.ToString("dd.MM.yyyy"));
          break;

        case TimeDivisionIntervals.Week:
          rangesToReturn = DateIntervalDivider.DivideByWeek(dateRange);

          if (dateRange.StartDate.Year == dateRange.EndDate.Year) foreach (var week in rangesToReturn) labelsToReturn.Add($"{week.StartDate:dd.MM} - {week.EndDate:dd.MM}");
          // labelsToReturn.Add(DateIntervalDivider.WeekNumber(week.StartDate).ToString());
          else foreach (var week in rangesToReturn) labelsToReturn.Add($"{week.StartDate:dd.MM.yyyy} - {week.EndDate:dd.MM.yyyy}");
          break;

        case TimeDivisionIntervals.Month:
          rangesToReturn = DateIntervalDivider.DivideByMonth(dateRange);

          if (dateRange.StartDate.Year == dateRange.EndDate.Year) foreach (var month in rangesToReturn) labelsToReturn.Add(month.StartDate.ToString("MM"));
          else foreach (var day in rangesToReturn) labelsToReturn.Add(day.StartDate.ToString("MM.yyyy"));
          break;

        case TimeDivisionIntervals.Year:
          rangesToReturn = DateIntervalDivider.DivideByYear(dateRange);

          foreach (var year in rangesToReturn) labelsToReturn.Add(year.StartDate.ToString("yyyy"));
          break;
      }
      _xAxisLabels = labelsToReturn.ToArray();
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

            //Adapt values to be shown on graph
            if (_settings.ValuesRelativeType)
            {
              if (incomeValue == 0) data[i, 0] = 0;
              else data[i, 0] = (double)(((incomeValue - expenseValue)));
              data[i, 1] = null;
              legendLabels = new string[1] { "Balance" };
            }
            else
            {
              data[i, 0] = (double)incomeValue;
              data[i, 1] = (double)-expenseValue;
            }
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
            var sumOfCategories = categoryValues.Sum() == 0 ? 1 : categoryValues.Sum();

            for (int j = 0; j < categoryValues.Count; j++)
            {
              if (_settings.ValuesRelativeType) data[i, j] = (double)(categoryValues.Take(j + 1).Sum() / sumOfCategories * 100);
              else data[i, j] = (double)(categoryValues.Take(j + 1).Sum());
            }
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
            var sumOfRecurrences = recurrencesValues.Sum() == 0 ? 1 : recurrencesValues.Sum();

            for (int j = 0; j < recurrencesValues.Count; j++)
            {
              if (_settings.ValuesRelativeType) data[i, j] = (double)(recurrencesValues.Take(j + 1).Sum() / sumOfRecurrences * 100);
              else data[i, j] = (double)(recurrencesValues.Take(j + 1).Sum());
            }
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

            if (_settings.ValuesRelativeType)
            {
              if (recValue == 0 && nonRecValue == 0)
              {
                data[i, 0] = 0;
                data[i, 1] = 0;
              }
              else
              {
                data[i, 0] = (double)((recValue) / (recValue + nonRecValue) * 100);
                data[i, 1] = (double)((recValue + nonRecValue) / (recValue + nonRecValue) * 100);
              }
            }
            else
            {
              data[i, 0] = (double)recValue;
              data[i, 1] = (double)-nonRecValue;
            }
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
            var sumOfSubCategories = subCategoryValues.Sum() == 0 ? 1 : subCategoryValues.Sum();

            for (int j = 0; j < subCategoryValues.Count; j++)
            {
              if (_settings.ValuesRelativeType) data[i, j] = (double)(subCategoryValues.Take(j + 1).Sum() / sumOfSubCategories * 100);
              else data[i, j] = (double)(subCategoryValues.Take(j + 1).Sum());
            }
          }
          break;
      }

      //Transpose array and return
      var transposedData = new double?[data.GetLength(1), data.GetLength(0)];
      for (int i = 0; i < data.GetLength(0); i++)
      {
        for (int j = 0; j < data.GetLength(1); j++)
        {
          transposedData[j, i] = data[i, j];
        }
      }

      _legendLabels = legendLabels;
      return transposedData;
    }

    private void AddGraphBars()
    {
      //Plot last first, because they are higher
      for (int i = _yAxisValues.GetLength(0) - 1; i >= 0; i--)
      {
        //One row is one data set
        double?[] row = new double?[_yAxisValues.GetLength(1)];

        for (int j = 0; j < _yAxisValues.GetLength(1); j++) row[j] = _yAxisValues[i, j];

        if (!row.Contains(null) && !row.IsNullOrEmpty())
        {
          var bar = _plot.Plot.AddBar(row.Select(r => (double)r).ToArray(), _xAxisPositions);
          bar.Label = _legendLabels[i];
          bar.FillColorNegative = bar.FillColor;
        }
      }
    }
  }
}