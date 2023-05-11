using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.DataProviders;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExpensesTracker.Models.DataControllers.Graphs_Classes
{
  internal class BarGraph : GraphWPF
  {
    public BarGraph(GraphSettings settings, List<Expense>? data) : base(settings, data) { }

    protected override double[] CalculateYAxisValues()
    {
      return new double[0];
    }

    protected override void PlotGraph()
    {
      SetTitle();
      Plot.XAxis.Label(_settings.XAxisName, size: 12);
      Plot.YAxis.Label(_settings.YAxisName, size: 12);
      DetermineXAxisLabels();
    }
    private string[] DetermineXAxisLabels()
    {
      var currentDate = DateTime.Now;
      var tempDate = DateTime.Now;
      var currentWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(tempDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
      var weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(tempDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
      int weeks = 1;
      var XLabels = new List<string>();
      switch (_settings.TimeScope)
      {
        case TimeRanges.custom:
          break;

        case TimeRanges.week:
          XLabels.Add(tempDate.ToString("dddd"));
          do
          {
            tempDate = tempDate.AddDays(-1);
            XLabels.Add(tempDate.ToString("dddd"));
          } while (tempDate.DayOfWeek != DayOfWeek.Monday);
          XLabels.Reverse();
          break;

        case TimeRanges.weekExact:
          for (int i = 0; i > -7; i--)
          {
            XLabels.Add(tempDate.DayOfWeek.ToString());
            tempDate = tempDate.AddDays(-1);
          }
          XLabels.Reverse();
          break;

        case TimeRanges.month:
          do
          {
            tempDate = tempDate.AddDays(-1);
            weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(tempDate.AddDays(-1), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            if (currentWeekNumber != weekNumber)
            {
              currentWeekNumber = weekNumber;
              weeks++;
            }
          } while (tempDate.Month == currentDate.Month);
          for (int i = 1; i < weeks + 1; i++) XLabels.Add(i.ToString());
          break;

        case TimeRanges.monthExact:
          for (int i = 0; i > -30; i--)
          {
            tempDate = tempDate.AddDays(-1);
            weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(tempDate.AddDays(-1), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            if (currentWeekNumber != weekNumber)
            {
              currentWeekNumber = weekNumber;
              weeks++;
            }
          }
          for (int i = 1; i < weeks + 1; i++) XLabels.Add(i.ToString());
          break;

        case TimeRanges.year:
          XLabels.Add(tempDate.ToString("MMMM"));
          do
          {
            tempDate = tempDate.AddMonths(-1);
            XLabels.Add(tempDate.ToString("MMMM"));
          } while (tempDate.Month != 1);
          XLabels.Reverse();
          break;

        case TimeRanges.yearExact:
          for (int i = 0; i > -12; i--)
          {
            XLabels.Add(tempDate.ToString("MMMM"));
            tempDate = tempDate.AddMonths(-1);
          }
          XLabels.Reverse();
          break;

        default:
          break;

      }
      return XLabels.ToArray();
    }
  }
}

