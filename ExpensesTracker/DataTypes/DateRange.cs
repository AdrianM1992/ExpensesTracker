using System;

namespace ExpensesTracker.DataTypes
{
  /// <summary>
  /// Time span represented by StartDate and EndDate
  /// </summary>
  public class DateRange
  {
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    ///<summary>
    ///New DateRange
    /// </summary>
    /// <param name="startDate">If null == DateTime.MinValue</param>
    /// <param name="endDate">If null == value DateTime.Now</param>
    public DateRange(DateTime? startDate = null, DateTime? endDate = null)
    {
      if (startDate != null && endDate != null)
      {
        //Check if dates are not swapped
        if (startDate < endDate)
        {
          StartDate = (DateTime)startDate;
          EndDate = (DateTime)endDate;
        }
        else
        {
          StartDate = (DateTime)endDate;
          EndDate = (DateTime)startDate;
        }
      }
      else
      {
        //In case any of dates is null assign default value
        StartDate = startDate == null ? DateTime.MinValue : (DateTime)startDate;
        EndDate = endDate == null ? DateTime.Now : (DateTime)endDate;
      }
    }

    public override string ToString() => $"{StartDate:g} - {EndDate:g}";
  }
}


