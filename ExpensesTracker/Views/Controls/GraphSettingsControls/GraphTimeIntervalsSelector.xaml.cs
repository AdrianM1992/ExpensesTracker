using ExpensesTracker.DataTypes;
using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.Settings;
using ExpensesTracker.Views.Interfaces;
using System.ComponentModel;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  ///  Graph time scope configuration control
  /// </summary>
  public partial class GraphTimeIntervalsSelector : UserControl, INotifyPropertyChanged, ISettingsSetter
  {
    private bool _initFlag = true;
    private GraphSettings _graphSettings = new();

    private bool _nameXAxis;
    private bool NameXAxis
    {
      get { return _nameXAxis; }
      set { _nameXAxis = value; }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
      if (!_initFlag) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #region Notifying properties
    private bool? _clearAll;

    public bool? ClearAllVisible
    {
      get { return _clearAll; }
      set { _clearAll = value; OnPropertyChanged(nameof(ClearAll)); }
    }
    #endregion

    public GraphTimeIntervalsSelector()
    {
      InitializeComponent();
      _initFlag = false;
    }

    #region Front panel events
    private void CheckBox_CheckedChanged(object sender, System.Windows.RoutedEventArgs e)
    {
      var checkBox = (CheckBox)sender;
      if (checkBox.IsChecked != null)
      {
        XAxisDescription.IsEnabled = NameXAxis = checkBox.IsChecked.Value;
        _graphSettings.XAxisName = NameXAxis ? XAxisDescription.Text : null;
        ClearAllVisible = true;
      }
    }
    private void XAxisDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
      var textBox = (TextBox)sender;
      _graphSettings.XAxisName = textBox.Text;
      ClearAllVisible = true;
    }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var comboBox = (ComboBox)sender;
      _graphSettings.TimeScope = (TimeRanges)comboBox.SelectedIndex;

      if (!_initFlag)
      {
        if (_graphSettings.TimeScope == TimeRanges.custom)
        {
          CustomTimeSettings.IsEnabled = true;
          _graphSettings.TimeDivisor = (TimeDivisionIntervals)TimeDivisorComboBox.SelectedIndex;
        }
        else
        {
          CustomTimeSettings.IsEnabled = false;
          _graphSettings.UserTimeScope = null;
          _graphSettings.TimeDivisor = null;
        }
      }

      ClearAllVisible = true;
    }
    private void TimeDivisorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var comboBox = (ComboBox)sender;
      _graphSettings.TimeDivisor = (TimeDivisionIntervals)comboBox.SelectedIndex;
      ClearAllVisible = true;
    }
    private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      _graphSettings.UserTimeScope = new DateRange(StartDate.SelectedDate, EndDate.SelectedDate);
      ClearAllVisible = true;
    }
    #endregion

    #region ISettingsSetter implementation
    public void SetDefaultValues()
    {
      _initFlag = true;

      XAxisNamed.IsChecked = false;
      XAxisDescription.IsEnabled = false;
      XAxisDescription.Text = string.Empty;
      TimeScopeCombo.SelectedIndex = 2;
      TimeDivisorComboBox.SelectedIndex = 0;
      StartDate.SelectedDate = null;
      EndDate.SelectedDate = null;
      CustomTimeSettings.IsEnabled = false;

      _initFlag = false;
    }
    public void SetNewSettingsRef(object graphSettings) => _graphSettings = (GraphSettings)graphSettings;
    public void SetExistingSettingsRef(object graphSettings)
    {
      _initFlag = true;

      _graphSettings = (GraphSettings)graphSettings;
      XAxisDescription.IsEnabled = NameXAxis = _graphSettings.XAxisName != null;
      XAxisDescription.Text = _graphSettings.XAxisName;
      XAxisNamed.IsChecked = NameXAxis;
      TimeScopeCombo.SelectedIndex = (int)_graphSettings.TimeScope;
      CustomTimeSettings.IsEnabled = _graphSettings.TimeScope == TimeRanges.custom;
      StartDate.SelectedDate = _graphSettings.UserTimeScope?.StartDate;
      EndDate.SelectedDate = _graphSettings.UserTimeScope?.EndDate;
      TimeDivisorComboBox.SelectedIndex = _graphSettings.TimeDivisor == null ? 0 : (int)_graphSettings.TimeDivisor;

      _initFlag = false;
    }
    public void ClearAll()
    {
      _initFlag = true;

      ClearAllVisible = false;

      _initFlag = false;
    }
    #endregion
  }
}
