using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.Interfaces;
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
    private readonly IMainSettings _mainSettings;
    private string? _currentTabName;
    public GraphsPage(IMainSettings mainSettings)
    {
      _viewModel = GraphsPageViewModel.GetGraphsPageViewModelRef(this, ModifyContainers, mainSettings);
      _mainSettings = mainSettings;
      InitializeComponent();
    }

    private void AddDuplicateButton_Click(object sender, RoutedEventArgs e)
    {
      var button = (Button)sender;
      //For future implementation
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

    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
      _viewModel.LoadGraphSettings(_currentTabName);
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      _viewModel.SaveGraphSettings(_currentTabName);
    }
  }
}
