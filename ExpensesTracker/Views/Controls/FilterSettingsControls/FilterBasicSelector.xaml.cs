using ExpensesTracker.Models.Settings;
using ExpensesTracker.Views.Interfaces;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExpensesTracker.Views.Controls.FilterSettingsControls
{
  /// <summary>
  /// Basic database filter control
  /// </summary>
  public partial class FilterBasicSelector : UserControl, INotifyPropertyChanged, ISettingsSetter
  {
    private bool _initFlag = false;
    private FilterSettings _filterSettings = new();

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

    public FilterBasicSelector() => InitializeComponent();

    #region IFilterSettings implementation
    public void SetNewSettingsRef(object filterSettings) => _filterSettings = (FilterSettings)filterSettings;
    public void SetExistingSettingsRef(object filterSettings)
    {
      _initFlag = true;

      _filterSettings = (FilterSettings)filterSettings;
      if (_filterSettings.Name != null) SearchBox.Text = _filterSettings.Name;
      if (_filterSettings.Income != null)
      {
        if (_filterSettings.Income == true) IncomeT.IsChecked = true;
        else IncomeF.IsChecked = true;
      }
      if (_filterSettings.Recurring != null)
      {
        if (_filterSettings.Recurring == true) RecurringT.IsChecked = true;
        else RecurringF.IsChecked = true;
      }

      _initFlag = false;
    }
    public void SetDefaultValues()
    {
      _initFlag = true;

      SearchBox.Text = "Search database...";
      IncomeT.IsChecked = false;
      IncomeF.IsChecked = false;
      RecurringT.IsChecked = false;
      RecurringF.IsChecked = false;

      _initFlag = false;
    }
    public void ClearAll()
    {
      _initFlag = true;

      ClearAllVisible = false;
      ClearButtonBasic_MouseLeftButtonDown(ClearBasic, null);

      _initFlag = false;
    }
    #endregion

    #region Front panel events
    private void SearchBar_GotFocus(object sender, RoutedEventArgs e)
    {
      if (sender is TextBox textBox)
      {
        if (textBox.Text == "Search database...")
        {
          textBox.Foreground = Brushes.Black;
          textBox.FontStyle = FontStyles.Normal;
          textBox.Text = "";
        }
      }
    }
    private void SearchBar_LostFocus(object sender, RoutedEventArgs e)
    {
      if (sender is TextBox textBox)
      {
        if (textBox.Text == "")
        {
          textBox.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x5F, 0x5F, 0x5F));
          textBox.FontStyle = FontStyles.Italic;
          textBox.Text = "Search database...";
        }
      }
    }
    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (sender is TextBox searchPhrase)
      {
        if (searchPhrase.Text != "Search database...")
        {
          _filterSettings.Name = searchPhrase.Text;
          ClearBasic.Visibility = Visibility.Visible;
          ClearAllVisible = true;
        }
      }
    }
    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
      ClearAllVisible = true;
      ClearBasic.Visibility = Visibility.Visible;
      var radioButton = (RadioButton)sender;
      if (radioButton.GroupName == "Income")
      {
        ClearType.Visibility = Visibility.Visible;
        if (radioButton.Content.ToString() == "Income") _filterSettings.Income = true;
        else _filterSettings.Income = false;
      }
      else
      {
        ClearRecurrence.Visibility = Visibility.Visible;
        if (radioButton.Content.ToString() == "Recurring") _filterSettings.Recurring = true;
        else _filterSettings.Recurring = false;
      }
    }
    private void ClearButtonBasic_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs? e)
    {
      var clearButton = sender as ClearButton;

      if (clearButton.Name == "ClearBasic" || clearButton.Name == "ClearType")
      {
        ClearType.Visibility = Visibility.Hidden;
        IncomeT.IsChecked = false;
        IncomeF.IsChecked = false;
        _filterSettings.Income = null;
      }
      if (clearButton.Name == "ClearBasic" || clearButton.Name == "ClearRecurrence")
      {
        ClearRecurrence.Visibility = Visibility.Hidden;
        RecurringT.IsChecked = false;
        RecurringF.IsChecked = false;
        _filterSettings.Recurring = null;
      }
      if (clearButton.Name == "ClearBasic")
      {
        SearchBox.Text = "Search database...";
        _filterSettings.Name = null;
      }
      clearButton.Visibility = Visibility.Hidden;
    }
    #endregion
  }
}