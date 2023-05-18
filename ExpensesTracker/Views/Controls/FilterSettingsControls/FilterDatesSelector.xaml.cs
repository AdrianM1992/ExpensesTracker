using ExpensesTracker.DataTypes;
using ExpensesTracker.Models.Settings;
using ExpensesTracker.Views.Interfaces;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
namespace ExpensesTracker.Views.Controls.FilterSettingsControls
{
  /// <summary>
  /// Dates database filter control
  /// </summary>
  public partial class FilterDatesSelector : UserControl, INotifyPropertyChanged, IFilterSettings
  {
    private FilterSettings _filterSettings = new();
    private bool _initFlag = false;

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

    public FilterDatesSelector() => InitializeComponent();

    #region IFilterSettings implementation
    public void ClearAll() => ClearButtonDates_MouseLeftButtonDown(ClearDates, null);
    public void SetDefaultValues()
    {
      SubmitDateMin.SelectedDate = null;
      SubmitDateMax.SelectedDate = null;
      UpdateDateMin.SelectedDate = null;
      UpdateDateMax.SelectedDate = null;
      UserDateMin.SelectedDate = null;
      UserDateMax.SelectedDate = null;
    }
    public void SetExistingFilterSettingsRef(FilterSettings filterSettings) => _filterSettings = filterSettings;
    public void SetFilterSettingsRef(FilterSettings filterSettings)
    {
      _initFlag = true;

      _filterSettings = filterSettings;
      if (_filterSettings.SubmitDateRange != null)
      {
        SubmitDateMin.SelectedDate = _filterSettings.SubmitDateRange.StartDate;
        SubmitDateMax.SelectedDate = _filterSettings.SubmitDateRange.EndDate;
      }
      if (_filterSettings.UpdateDateRange != null)
      {
        UpdateDateMin.SelectedDate = _filterSettings.UpdateDateRange.StartDate;
        UpdateDateMax.SelectedDate = _filterSettings.UpdateDateRange.EndDate;
      }
      if (_filterSettings.UserDateRange != null)
      {
        UserDateMin.SelectedDate = _filterSettings.UserDateRange.StartDate;
        UserDateMax.SelectedDate = _filterSettings.UserDateRange.EndDate;
      }

      _initFlag = false;
    }
    #endregion

    #region Front panel events
    private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      ClearAllVisible = true;
      ClearDates.Visibility = Visibility.Visible;
      var datePicker = (DatePicker)sender;
      switch (datePicker.Name)
      {
        case "SubmitDateMin":
          ClearDateAdded.Visibility = Visibility.Visible;
          if (_filterSettings.SubmitDateRange == null) _filterSettings.SubmitDateRange = new DateRange(datePicker.SelectedDate);
          else _filterSettings.SubmitDateRange = new DateRange(datePicker.SelectedDate, _filterSettings.SubmitDateRange.EndDate);
          break;
        case "UpdateDateMin":
          ClearDateUpdated.Visibility = Visibility.Visible;
          if (_filterSettings.UpdateDateRange == null) _filterSettings.UpdateDateRange = new DateRange(datePicker.SelectedDate);
          else _filterSettings.UpdateDateRange = new DateRange(datePicker.SelectedDate, _filterSettings.UpdateDateRange.EndDate);
          break;
        case "UserDateMin":
          ClearDateOccurred.Visibility = Visibility.Visible;
          if (_filterSettings.UserDateRange == null) _filterSettings.UserDateRange = new DateRange(datePicker.SelectedDate);
          else _filterSettings.UserDateRange = new DateRange(datePicker.SelectedDate, _filterSettings.UserDateRange.EndDate);
          break;
        case "SubmitDateMax":
          ClearDateAdded.Visibility = Visibility.Visible;
          if (_filterSettings.SubmitDateRange == null) _filterSettings.SubmitDateRange = new DateRange(endDate: datePicker.SelectedDate);
          else _filterSettings.SubmitDateRange = new DateRange(_filterSettings.SubmitDateRange.StartDate, datePicker.SelectedDate);
          break;
        case "UpdateDateMax":
          ClearDateUpdated.Visibility = Visibility.Visible;
          if (_filterSettings.UpdateDateRange == null) _filterSettings.UpdateDateRange = new DateRange(endDate: datePicker.SelectedDate);
          else _filterSettings.UpdateDateRange = new DateRange(_filterSettings.UpdateDateRange.StartDate, datePicker.SelectedDate);
          break;
        case "UserDateMax":
          ClearDateOccurred.Visibility = Visibility.Visible;
          if (_filterSettings.UserDateRange == null) _filterSettings.UserDateRange = new DateRange(endDate: datePicker.SelectedDate);
          else _filterSettings.UserDateRange = new DateRange(_filterSettings.UserDateRange.StartDate, datePicker.SelectedDate);
          break;
      }
    }
    private void ClearButtonDates_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs? e)
    {
      var clearButton = sender as ClearButton;

      if (clearButton.Name == "ClearDateAdded" || clearButton.Name == "ClearDates")
      {
        SubmitDateMin.SelectedDate = null;
        SubmitDateMax.SelectedDate = null;
        ClearDateAdded.Visibility = Visibility.Hidden;
        _filterSettings.SubmitDateRange = null;
      }
      if (clearButton.Name == "ClearDateUpdated" || clearButton.Name == "ClearDates")
      {
        UpdateDateMin.SelectedDate = null;
        UpdateDateMax.SelectedDate = null;
        ClearDateUpdated.Visibility = Visibility.Hidden;
        _filterSettings.UpdateDateRange = null;
      }
      if (clearButton.Name == "ClearDateOccurred" || clearButton.Name == "ClearDates")
      {
        UserDateMin.SelectedDate = null;
        UserDateMax.SelectedDate = null;
        ClearDateOccurred.Visibility = Visibility.Hidden;
        _filterSettings.UserDateRange = null;
      }

      clearButton.Visibility = Visibility.Hidden;
    }
    #endregion
  }
}