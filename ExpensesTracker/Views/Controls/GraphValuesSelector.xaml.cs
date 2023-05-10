using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.DataControllers;
using System.ComponentModel;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Graph values configuration control
  /// </summary>
  public partial class GraphValuesSelector : UserControl, INotifyPropertyChanged
  {
    private GraphSettings _graphSettings = new();
    private bool _nameYAxis;
    private bool NameYAxis
    {
      get { return _nameYAxis; }
      set { _nameYAxis = value; }
    }

    #region Notifying properties

    private string? _yAxisName;
    private bool _valuesRelativeType;
    private ValuesScopes _valuesScope;

    public string? YAxisName
    {
      get { return _yAxisName; }
      set { _yAxisName = value; OnPropertyChanged(nameof(YAxisName)); }
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

    #endregion

    public GraphValuesSelector()
    {
      InitializeComponent();
    }
    public void SetGraphSettingsReference(GraphSettings graphSettings)
    {
      _graphSettings = graphSettings;
      _graphSettings.YAxisName = YAxisName;
      _graphSettings.ValuesRelativeType = ValuesRelativeType;
      _graphSettings.ValuesScope = ValuesScope;
    }

    private void OnPropertyChanged(string propertyName)
    {
      switch (propertyName)
      {
        case "YAxisName":
          _graphSettings.YAxisName = YAxisName;
          break;
        case "ValuesScope":
          _graphSettings.ValuesScope = ValuesScope;
          break;
        case "ValuesRelativeType":
          _graphSettings.ValuesRelativeType = ValuesRelativeType;
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
        YAxisDescription.IsEnabled = NameYAxis = checkBox.IsChecked.Value;
        YAxisName = NameYAxis ? YAxisDescription.Text : null;
      }
    }
    private void YAxisDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
      var textBox = (TextBox)sender;
      YAxisName = textBox.Text;
    }
    private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
    {
      var radioButton = (RadioButton)sender;
      ValuesRelativeType = radioButton.Name == "RelativeRadioButton";
    }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var comboBox = (ComboBox)sender;
      ValuesScope = (ValuesScopes)comboBox.SelectedIndex;
    }
  }
}
