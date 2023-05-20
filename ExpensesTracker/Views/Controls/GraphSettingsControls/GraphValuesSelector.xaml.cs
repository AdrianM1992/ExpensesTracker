using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.Settings;
using ExpensesTracker.Views.Interfaces;
using System.ComponentModel;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Graph values configuration control
  /// </summary>
  public partial class GraphValuesSelector : UserControl, INotifyPropertyChanged, ISettingsSetter
  {
    private bool _initFlag = false;
    private GraphSettings _graphSettings = new();

    private bool _nameYAxis;
    private bool NameYAxis
    {
      get { return _nameYAxis; }
      set { _nameYAxis = value; }
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

    public GraphValuesSelector()
    {
      InitializeComponent();
    }

    #region Front panel events
    private void CheckBox_CheckedChanged(object sender, System.Windows.RoutedEventArgs e)
    {
      var checkBox = (CheckBox)sender;
      if (checkBox.IsChecked != null)
      {
        YAxisDescription.IsEnabled = NameYAxis = checkBox.IsChecked.Value;
        _graphSettings.YAxisName = NameYAxis ? YAxisDescription.Text : null;
        ClearAllVisible = true;
      }
    }
    private void YAxisDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
      var textBox = (TextBox)sender;
      _graphSettings.YAxisName = textBox.Text;
      ClearAllVisible = true;
    }
    private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
    {
      var radioButton = (RadioButton)sender;
      _graphSettings.ValuesRelativeType = radioButton.Name == "RelativeRadioButton";
      ClearAllVisible = true;
    }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var comboBox = (ComboBox)sender;
      _graphSettings.ValuesScope = (ValuesScopes)comboBox.SelectedIndex;
      ClearAllVisible = true;
    }
    #endregion

    #region ISettingsSetter implementation
    public void SetDefaultValues()
    {
      _initFlag = true;

      YAxisNamed.IsChecked = false;
      YAxisDescription.IsEnabled = false;
      YAxisDescription.Text = string.Empty;
      RelativeRadioButton.IsChecked = true;
      ValuesScopeCombo.SelectedIndex = 0;

      _initFlag = false;
    }
    public void SetNewSettingsRef(object graphSettings) => _graphSettings = (GraphSettings)graphSettings;
    public void SetExistingSettingsRef(object graphSettings)
    {
      _initFlag = true;

      _graphSettings = (GraphSettings)graphSettings;
      YAxisDescription.IsEnabled = NameYAxis = _graphSettings.YAxisName != null;
      YAxisDescription.Text = _graphSettings.YAxisName;
      YAxisNamed.IsChecked = NameYAxis;
      if (_graphSettings.ValuesRelativeType) RelativeRadioButton.IsChecked = true;
      else AbsoluteRadioButton.IsChecked = true;
      ValuesScopeCombo.SelectedIndex = (int)_graphSettings.ValuesScope;

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