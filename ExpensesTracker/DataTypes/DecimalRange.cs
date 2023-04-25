namespace ExpensesTracker.DataTypes
{
  public class DecimalRange
  {
    public decimal NumberMin { get; private set; }
    public decimal NumberMax { get; private set; }

    public DecimalRange(decimal min = 0M, decimal max = decimal.MaxValue)
    {
      if (min > max)
      {
        NumberMax = min;
        NumberMin = max;
      }
      else
      {
        NumberMin = min;
        NumberMax = max;
      }
    }
  }
}
