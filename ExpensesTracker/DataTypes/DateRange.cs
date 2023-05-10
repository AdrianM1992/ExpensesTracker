using System;

namespace ExpensesTracker.DataTypes
{
  public class DateRange
  {
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public DateRange(DateTime? startDate = null, DateTime? endDate = null)
    {
      if (startDate != null && endDate != null)
      {
        if (startDate < endDate)
        {
          StartDate = startDate;
          EndDate = endDate;
        }
        else
        {
          StartDate = endDate;
          EndDate = startDate;
        }
      }
      else if (startDate == null)
      {
        StartDate = DateTime.MinValue;
        EndDate = endDate;
      }
      else if (endDate == null)
      {
        EndDate = DateTime.Now;
        StartDate = startDate;
      }
    }
  }
}


