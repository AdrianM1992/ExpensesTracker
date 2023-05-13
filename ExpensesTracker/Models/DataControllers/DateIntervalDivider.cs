using ExpensesTracker.DataTypes;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExpensesTracker.Models.DataControllers
{
  public static class DateIntervalDivider
  {
    /// <summary>
    /// Divides DateRange to day intervals and converts it into List<DateRange>
    /// </summary>
    /// <param name="dateRange">Date to convert</param>
    /// <returns>List of day intervals</returns>
    public static List<DateRange> DivideByDay(DateRange dateRange)
    {
      List<DateRange> rangesToReturn = new();
      var iterationDate = dateRange.StartDate.Date;
      do
      {
        rangesToReturn.Add(new DateRange(iterationDate, iterationDate.AddDays(1)));
        iterationDate = iterationDate.AddDays(1);

      } while (iterationDate <= dateRange.EndDate.Date);
      return rangesToReturn;
    }
    /// <summary>
    /// Divides DateRange to week intervals and converts it into List<DateRange>
    /// </summary>
    /// <param name="dateRange">Date to convert</param>
    /// <returns>List of week intervals</returns>
    public static List<DateRange> DivideByWeek(DateRange dateRange)
    {
      List<DateRange> rangesToReturn = new();
      var currentWeekNumber = WeekNumber(dateRange.StartDate);
      var iterationDate = dateRange.StartDate.Date;
      var firstDayOfWeek = dateRange.StartDate.Date;
      do
      {
        if (currentWeekNumber != WeekNumber(iterationDate))
        {
          rangesToReturn.Add(new DateRange(firstDayOfWeek, iterationDate));
          firstDayOfWeek = iterationDate;
          currentWeekNumber = WeekNumber(iterationDate);
        }
        else iterationDate = iterationDate.AddDays(1);

      } while (iterationDate <= dateRange.EndDate);

      rangesToReturn.Add(new DateRange(firstDayOfWeek, iterationDate));
      return rangesToReturn;
    }
    /// <summary>
    /// Divides DateRange to month intervals and converts it into List<DateRange>
    /// </summary>
    /// <param name="dateRange">Date to convert</param>
    /// <returns>List of month intervals</returns>
    public static List<DateRange> DivideByMonth(DateRange dateRange)
    {
      List<DateRange> rangesToReturn = new();
      var iterationDate = dateRange.StartDate.Date;
      var firstDayOfMonth = dateRange.StartDate.Date;
      do
      {
        if (firstDayOfMonth.Month != iterationDate.Month)
        {
          rangesToReturn.Add(new DateRange(firstDayOfMonth, iterationDate));
          firstDayOfMonth = iterationDate;
        }
        else iterationDate = iterationDate.AddDays(1);

      } while (iterationDate <= dateRange.EndDate);

      rangesToReturn.Add(new DateRange(firstDayOfMonth, iterationDate));
      return rangesToReturn;
    }
    /// <summary>
    /// Divides DateRange to year intervals and converts it into List<DateRange>
    /// </summary>
    /// <param name="dateRange">Date to convert</param>
    /// <returns>List of year intervals</returns>
    public static List<DateRange> DivideByYear(DateRange dateRange)
    {
      List<DateRange> rangesToReturn = new();
      var iterationDate = dateRange.StartDate.Date;
      var firstDayOfYear = dateRange.StartDate.Date;
      do
      {
        if (firstDayOfYear.Year != iterationDate.Year)
        {
          rangesToReturn.Add(new DateRange(firstDayOfYear, iterationDate));
          firstDayOfYear = iterationDate;
        }
        else iterationDate = iterationDate.AddDays(1);

      } while (iterationDate <= dateRange.EndDate);

      rangesToReturn.Add(new DateRange(firstDayOfYear, iterationDate));
      return rangesToReturn;
    }

    /// <summary>
    /// Converts date into week of the year number
    /// </summary>
    /// <param name="date">Date to convert</param>
    /// <returns>Week number</returns>
    public static int WeekNumber(DateTime date)
    {
      return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }
  }
}
