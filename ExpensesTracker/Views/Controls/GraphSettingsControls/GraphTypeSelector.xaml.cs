using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.Settings;
using ExpensesTracker.Views.Interfaces;
using System.ComponentModel;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Graph general configuration control
  /// </summary>
  public partial class GraphTypeSelector : UserControl, INotifyPropertyChanged, ISettingsSetter
  {
    private bool _initFlag = false;
    private GraphSettings _graphSettings = new();

    private bool _nameGraph;
    private bool NameGraph
    {
      get { return _nameGraph; }
      set { _nameGraph = value; }
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

    public GraphTypeSelector()
    {
      InitializeComponent();
    }

    #region Front panel events

    private void CheckBox_CheckedChanged(object sender, System.Windows.RoutedEventArgs e)
    {
      var checkBox = (CheckBox)sender;
      if (checkBox.IsChecked != null)
      {
        GraphDescription.IsEnabled = NameGraph = checkBox.IsChecked.Value;
        _graphSettings.GraphName = NameGraph ? GraphDescription.Text : null;
        ClearAllVisible = true;
      }
    }
    private void GraphDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
      var textBox = (TextBox)sender;
      _graphSettings.GraphName = textBox.Text;
      ClearAllVisible = true;
    }
    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var listBox = (ListBox)sender;
      _graphSettings.GraphType = (GraphTypes)listBox.SelectedIndex;
      ClearAllVisible = true;
    }
    #endregion

    #region ISettingsSetter implementation
    public void SetDefaultValues()
    {
      _initFlag = true;

      GraphNamed.IsChecked = false;
      GraphDescription.IsEnabled = false;
      GraphDescription.Text = string.Empty;
      GraphTypeList.SelectedIndex = 0;

      _initFlag = false;
    }
    public void SetNewSettingsRef(object graphSettings) => _graphSettings = (GraphSettings)graphSettings;
    public void SetExistingSettingsRef(object graphSettings)
    {
      _initFlag = true;

      _graphSettings = (GraphSettings)graphSettings;
      GraphNamed.IsChecked = NameGraph = _graphSettings.GraphName == null;
      GraphDescription.IsEnabled = NameGraph;
      GraphTypeList.SelectedIndex = (int)_graphSettings.GraphType;

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