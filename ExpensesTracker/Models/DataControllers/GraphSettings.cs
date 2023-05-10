using ExpensesTracker.DataTypes;
using ExpensesTracker.DataTypes.Enums;
using System.ComponentModel;

namespace ExpensesTracker.Models.DataControllers
{
  public class GraphSettings : INotifyPropertyChanged
  {
    private GraphTypes _graphType;
    private string? _graphName;
    private string? _xAxisName;
    private string? _yAxisName;
    private TimeRanges _timeScope;
    private DateRange? _userTimeScope;
    private TimeDivisionIntervals? _timeDivisor;
    private bool _valuesRelativeType;
    private ValuesScopes _valuesScope;

    public GraphTypes GraphType
    {
      get { return _graphType; }
      set { _graphType = value; OnPropertyChanged(nameof(GraphType)); }
    }
    public string? GraphName
    {
      get { return _graphName; }
      set { _graphName = value; OnPropertyChanged(nameof(GraphName)); }
    }
    public string? XAxisName
    {
      get { return _xAxisName; }
      set { _xAxisName = value; OnPropertyChanged(nameof(XAxisName)); }
    }
    public string? YAxisName
    {
      get { return _yAxisName; }
      set { _yAxisName = value; OnPropertyChanged(nameof(YAxisName)); }
    }
    public TimeRanges TimeScope
    {
      get { return _timeScope; }
      set { _timeScope = value; OnPropertyChanged(nameof(TimeScope)); }
    }
    public DateRange? UserTimeScope
    {
      get { return _userTimeScope; }
      set { _userTimeScope = value; OnPropertyChanged(nameof(UserTimeScope)); }
    }
    public TimeDivisionIntervals? TimeDivisor
    {
      get { return _timeDivisor; }
      set { _timeDivisor = value; OnPropertyChanged(nameof(TimeDivisor)); }
    }
    public bool ValuesRelativeType
    {
      get { return _valuesRelativeType; }
      set { _valuesRelativeType = value; OnPropertyChanged(nameof(ValuesRelativeType)); }
    }
    public ValuesScopes ValuesScope
    {
      get { return _valuesScope; }
      set { _valuesScope = value; OnPropertyChanged(nameof(ValuesScope)); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
