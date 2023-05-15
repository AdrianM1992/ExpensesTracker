using ExpensesTracker.Models.DataControllers.FileOperations;
using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.Models.Settings;
using ExpensesTracker.ViewModels;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// GraphControl
  /// </summary>
  public partial class GraphControl : UserControl
  {
    private GraphControlViewModel _viewModel;
    private readonly IMainSettings _mainSettings;
    public GraphControl(IMainSettings mainSettings)
    {
      InitializeComponent();
      _mainSettings = mainSettings;
      _viewModel = new GraphControlViewModel(this, _mainSettings);
    }
    public void SaveGraphSettings()
    {
      GraphViewSettings graphViewSettings = new()
      {
        GraphSettings = _viewModel.GetGraphSettings(),
        FilterSettings = _viewModel.GetFilterSettings()
      };
      ClassSerializer.SaveClass(graphViewSettings);
    }
    public void LoadGraphSetting()
    {
      var graphViewSettings = (GraphViewSettings?)ClassSerializer.LoadClassFromFile(typeof(GraphViewSettings));
      _viewModel = graphViewSettings != null ? new GraphControlViewModel(this, _mainSettings, graphViewSettings) : _viewModel;
    }
    private void Expander_Expanded(object sender, System.Windows.RoutedEventArgs e)
    {
      var expander = (Expander)sender;
      if (expander.Name == "FilterExpander") SelectorsExpander.IsExpanded = false;
      else FilterExpander.IsExpanded = false;
    }
  }
}