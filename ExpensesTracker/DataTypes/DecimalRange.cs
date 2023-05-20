using System;
using System.Text.Json.Serialization;

namespace ExpensesTracker.DataTypes
{
  [Serializable]
  public class DecimalRange
  {
    public decimal NumberMin { get; private set; }
    public decimal NumberMax { get; private set; }

    [JsonConstructorAttribute]
    public DecimalRange(decimal numberMin, decimal numberMax)
    {
      NumberMin = numberMin;
      NumberMax = numberMax;
    }
    public DecimalRange(decimal? min = 0M, decimal? max = decimal.MaxValue)
    {
      if (min != null && max != null)
      {
        if (min > max)
        {
          NumberMax = (decimal)min;
          NumberMin = (decimal)max;
        }
        else
        {
          NumberMin = (decimal)min;
          NumberMax = (decimal)max;
        }
      }
      else
      {
        //In case any of dates is null assign default value
        NumberMin = min == null ? 0M : (decimal)min;
        NumberMax = max == null ? decimal.MaxValue : (decimal)max;
      }
    }
  }
}
