using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.DataControllers;
using System.ComponentModel;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Graph general configuration control
  /// </summary>
  public partial class GraphTypeSelector : UserControl, INotifyPropertyChanged
  {
    private GraphSettings _graphSettings = new();
    private bool _nameGraph;
    private bool NameGraph
    {
      get { return _nameGraph; }
      set { _nameGraph = value; }
    }

    #region Notifying properties

    private GraphTypes _graphType;
    private string? _graphName;

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

    #endregion

    public event PropertyChangedEventHandler? PropertyChanged;
    public GraphTypeSelector()
    {
      InitializeComponent();
    }
    public void SetGraphSettingsReference(GraphSettings graphSettings)
    {
      _graphSettings = graphSettings;
      _graphSettings.GraphType = GraphType;
      _graphSettings.GraphName = GraphName;
    }

    private void OnPropertyChanged(string propertyName)
    {
      switch (propertyName)
      {
        case "GraphName":
          _graphSettings.GraphName = GraphName;
          break;
        case "GraphType":
          _graphSettings.GraphType = GraphType;
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
        GraphDescription.IsEnabled = NameGraph = checkBox.IsChecked.Value;
        GraphName = NameGraph ? GraphDescription.Text : null;
      }
    }
    private void GraphDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
      var textBox = (TextBox)sender;
      GraphName = textBox.Text;
    }
    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var listBox = (ListBox)sender;
      GraphType = (GraphTypes)listBox.SelectedIndex;
    }
  }
}
