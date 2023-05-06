using ExpensesTracker.ViewModels;
using ExpensesTracker.Views.Controls;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
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
    private Dictionary<string, CustomTabControl> _tabs = new();
    private Dictionary<string, GraphControl> _graphs = new();
    private string? _currentTabName;
    public GraphsPage()
    {
      _viewModel = GraphsPageViewModel.GetGraphsPageViewModelRef(this);
      InitializeComponent();
    }

    private void AddDuplicateButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      string currentTabName = GetNewGraphName();
      var button = (Button)sender;

      if (button.Name == "DuplicateButton" && _currentTabName != null)
      {

      }
      else
      {

        _graphs.Add(currentTabName, new GraphControl());
      }
      _currentTabName = currentTabName;
      var tabToAdd = new CustomTabControl() { TabName = _currentTabName, IsHeaderEditable = true };
      tabToAdd.CustomTabEvent += CustomTabEventHandler;
      _tabs.Add(_currentTabName, tabToAdd);
      GraphsTabs.Children.Add(tabToAdd);
      SwapGraphTab(_currentTabName);
      GraphsContainer.Children.Clear();
      GraphsContainer.Children.Add(_graphs[_currentTabName]);
    }

    private void CustomTabEventHandler(string tabName, DataTypes.Enums.CustomTabEnums customTabEvent, string oldTabName = "")
    {
      string? currentTabName = tabName;
      switch (customTabEvent)
      {
        case DataTypes.Enums.CustomTabEnums.Clicked:
          break;
        case DataTypes.Enums.CustomTabEnums.NameChanged:
          currentTabName = GetNewGraphName(currentTabName);
          _tabs.Add(currentTabName, _tabs[oldTabName]);
          _tabs.Remove(oldTabName);
          _graphs.Add(currentTabName, _graphs[oldTabName]);
          _graphs.Remove(oldTabName);
          break;
        case DataTypes.Enums.CustomTabEnums.Closed:
          GraphsTabs.Children.Remove(_tabs[currentTabName]);
          _tabs.Remove(currentTabName);
          _graphs.Remove(currentTabName);
          currentTabName = !_tabs.IsNullOrEmpty() ? _tabs.Last().Key : null;
          break;
        default:
          break;
      }
      _currentTabName = currentTabName;
      GraphsContainer.Children.Clear();
      if (_currentTabName != null)
      {
        GraphsContainer.Children.Add(_graphs[_currentTabName]);
        SwapGraphTab(_currentTabName);
      }
    }

    private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      if (_currentTabName != null)
      {
        GraphsContainer.Children.Clear();
        GraphsTabs.Children.Remove(_tabs[_currentTabName]);
        _tabs.Remove(_currentTabName);
        _graphs.Remove(_currentTabName);
        _currentTabName = !_tabs.IsNullOrEmpty() ? _tabs.Last().Key : null;
        if (_currentTabName != null)
        {
          GraphsContainer.Children.Add(_graphs[_currentTabName]);
          SwapGraphTab(_currentTabName);
        }
      }
    }
    private void SwapGraphTab(string tabName)
    {
      //Grays out all opened tabs
      foreach (var customTabControl in _tabs.Values) customTabControl.BackgroundTabColor = SystemColors.MenuBarBrush;

      _tabs[tabName].BackgroundTabColor = SystemColors.ActiveCaptionBrush;
    }
    private string GetNewGraphName(string graphName = "New Graph")
    {
      string newGraphName = graphName;
      int tryCount = 1;
      while (_tabs.ContainsKey(newGraphName)) newGraphName = graphName + "_" + tryCount++;
      return newGraphName;
    }
  }
}
