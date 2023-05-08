using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.ViewModels;
using ExpensesTracker.Views.Controls;
using System.Windows;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Pages.Graphs
{
  /// <summary>
  /// 
  /// </summary>
  public partial class GraphsPage : Page
  {
    private readonly GraphsPageViewModel _viewModel;
    private string? _currentTabName;
    public GraphsPage()
    {
      _viewModel = GraphsPageViewModel.GetGraphsPageViewModelRef(this, ModifyContainers);
      InitializeComponent();
    }

    private void AddDuplicateButton_Click(object sender, RoutedEventArgs e)
    {
      var button = (Button)sender;

      if (button.Name == "DuplicateButton" && _currentTabName != null) _viewModel.DuplicateTab(_currentTabName);
      else _viewModel.AddNewTab();
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      if (_currentTabName != null) _viewModel.DeleteTab(_currentTabName);
    }
    private void ModifyContainers(CustomTabControl? graphTab, GraphTabActions? tabAction, GraphControl? graph, string? newCurrentTabName)
    {
      if (tabAction == GraphTabActions.Delete) GraphsTabs.Children.Remove(graphTab);
      else if (tabAction == GraphTabActions.Add) GraphsTabs.Children.Add(graphTab);
      GraphsContainer.Children.Clear();
      if (graph != null) GraphsContainer.Children.Add(graph);
      _currentTabName = newCurrentTabName;
    }
  }
}
