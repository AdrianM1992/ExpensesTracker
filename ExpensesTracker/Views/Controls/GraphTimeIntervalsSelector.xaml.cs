using ExpensesTracker.DataTypes;
using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.DataControllers;
using System.ComponentModel;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  ///  Graph time scope configuration control
  /// </summary>
  public partial class GraphTimeIntervalsSelector : UserControl, INotifyPropertyChanged
  {
    private readonly bool _initFlag = true;
    private GraphSettings _graphSettings = new();

    private bool _nameXAxis;
    private bool NameXAxis
    {
      get { return _nameXAxis; }
      set { _nameXAxis = value; }
    }

    #region Notifying properties

    private string? _xAxisName;
    private TimeRanges _timeScope;
    private TimeDivisionIntervals? _timeDivisor;
    private DateRange? _userTimeScope;

    public string? XAxisName
    {
      get { return _xAxisName; }
      set { _xAxisName = value; OnPropertyChanged(nameof(XAxisName)); }
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

    #endregion

    public event PropertyChangedEventHandler? PropertyChanged;
    public GraphTimeIntervalsSelector()
    {
      InitializeComponent();
      _initFlag = false;
    }
    public void SetGraphSettingsReference(GraphSettings graphSettings)
    {
      _graphSettings = graphSettings;
      _graphSettings.TimeScope = TimeScope;
      _graphSettings.UserTimeScope = UserTimeScope;
      _graphSettings.TimeDivisor = TimeDivisor;
      _graphSettings.XAxisName = XAxisName;
    }

    private void OnPropertyChanged(string propertyName)
    {
      switch (propertyName)
      {
        case "XAxisName":
          _graphSettings.XAxisName = XAxisName;
          break;
        case "TimeScope":
          _graphSettings.TimeScope = TimeScope;
          break;
        case "UserTimeScope":
          _graphSettings.UserTimeScope = UserTimeScope;
          break;
        case "TimeDivisor":
          _graphSettings.TimeDivisor = TimeDivisor;
          break;
        default:
          break;
      }
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void CheckBox_CheckedChanged(object sender, System.Windows.RoutedEventArgs e)
    {
      var checkBox = (CheckBox)sender;
      if (checkBox.IsChecked != null)
      {
        XAxisDescription.IsEnabled = NameXAxis = checkBox.IsChecked.Value;
        XAxisName = NameXAxis ? XAxisDescription.Text : null;
      }
    }
    private void XAxisDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
      var textBox = (TextBox)sender;
      XAxisName = textBox.Text;
    }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var comboBox = (ComboBox)sender;
      TimeScope = (TimeRanges)comboBox.SelectedIndex;
      if (!_initFlag)
      {
        if (TimeScope == TimeRanges.custom)
        {
          CustomTimeSettings.IsEnabled = true;
          TimeDivisor = (TimeDivisionIntervals)TimeDivisorComboBox.SelectedIndex;
        }
        else
        {
          CustomTimeSettings.IsEnabled = false;
          UserTimeScope = null;
          TimeDivisor = null;
        }
      }
    }
    private void TimeDivisorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var comboBox = (ComboBox)sender;
      TimeDivisor = (TimeDivisionIntervals)comboBox.SelectedIndex;
    }
    private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      UserTimeScope = new DateRange(StartDate.SelectedDate, EndDate.SelectedDate);
    }
  }
}
