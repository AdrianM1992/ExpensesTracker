using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.DataControllers.FileOperations;
using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.Models.Settings;
using ExpensesTracker.Views.Controls;
using ExpensesTracker.Views.Pages.Graphs;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using static ExpensesTracker.Views.Delegates.ViewDelegates;

namespace ExpensesTracker.ViewModels
{
  class GraphsPageViewModel
  {
    private readonly GraphsPage _graphsPage;
    private readonly IMainSettings _mainSettings;
    private readonly Dictionary<string, CustomTabControl> _tabs = new();
    private readonly Dictionary<string, GraphControl> _graphs = new();
    private readonly ModifyContainers _modifyView;

    private static GraphsPageViewModel? _instance;

    private GraphsPageViewModel(GraphsPage page, ModifyContainers modify, IMainSettings mainSettings)
    {
      _graphsPage = page;
      _modifyView = modify;
      _mainSettings = mainSettings;
    }

    /// <summary>
    /// Implementation of singleton pattern
    /// </summary>
    /// <param name="page">GraphsPage reference</param>
    /// <returns>Reference to GraphsPageViewModel</returns>
    public static GraphsPageViewModel GetGraphsPageViewModelRef(GraphsPage page, ModifyContainers modify, IMainSettings mainSettings)
    {
      if (_instance == null) return _instance = new GraphsPageViewModel(page, modify, mainSettings);
      else return _instance;
    }

    /// <summary>
    /// Deletes tab and corresponding graph control
    /// </summary>
    /// <param name="tabName">Name of tab to delete</param>
    public void DeleteTab(string tabName)
    {
      CustomTabControl tabToDelete = _tabs[tabName];
      GraphControl? graphToLoad = null;
      _tabs.Remove(tabName);
      _graphs.Remove(tabName);

      //If there is any tab left, switch it to be new current tab
      string? tabNameToReturn = !_tabs.IsNullOrEmpty() ? _tabs.Last().Key : null;
      if (tabNameToReturn != null)
      {
        SwapGraphTab(tabNameToReturn);
        graphToLoad = _graphs[tabNameToReturn];
      }

      _modifyView(tabToDelete, GraphTabActions.Delete, graphToLoad, tabNameToReturn);
    }

    /// <summary>
    /// Adds new tab and graph control to page
    /// </summary>
    public void AddNewTab(string? basedOn = null)
    {
      string newTabName = basedOn == null ? GetNewGraphName() : GetNewGraphName(basedOn);

      var tabToAdd = new CustomTabControl() { TabName = newTabName, IsHeaderEditable = true };
      tabToAdd.CustomTabEvent += CustomTabEventHandler;
      _tabs.Add(newTabName, tabToAdd);

      //Binding graph control height to graphs container height to prevent bugs during page size changing
      var graphToAdd = new GraphControl(_mainSettings);
      var binding = new Binding("Height") { Source = _graphsPage.GraphsContainer };
      graphToAdd.SetBinding(FrameworkElement.HeightProperty, binding);
      _graphs.Add(newTabName, graphToAdd);

      SwapGraphTab(newTabName);
      _modifyView(tabToAdd, GraphTabActions.Add, graphToAdd, newTabName);
    }
    public void DuplicateTab(string tabName)
    {
      object? settings = ClassSerializer.CopyClass(_graphs[tabName].DuplicateGraph(), typeof(GraphViewSettings));
      if (settings != null)
      {
        AddNewTab(tabName);
        _graphs.Last().Value.LoadGraphSetting((GraphViewSettings)settings);
      }
    }

    public Dictionary<string, GraphControl> Get_graphs()
    {
      return _graphs;
    }

    public void SaveGraphSettings(string? tabName, Dictionary<string, GraphControl> _graphs)
    {
      if (tabName != null && _graphs.TryGetValue(tabName, out GraphControl? tab)) tab.SaveGraphSettings();
    }

    public void LoadGraphSettings(string? tabName)
    {
      if (tabName != null && _graphs.TryGetValue(tabName, out GraphControl? tab)) tab.LoadGraphSetting();
    }

    /// <summary>
    /// Custom tab event handler
    /// </summary>
    /// <param name="tabName">Tab that triggered event</param>
    /// <param name="customTabEvent">Triggering action</param>
    /// <param name="oldTabName">Optional parameter used when tab names has changed</param>
    private void CustomTabEventHandler(string tabName, CustomTabEnums customTabEvent, string oldTabName = "")
    {
      string? currentTabName = tabName;
      switch (customTabEvent)
      {
        case CustomTabEnums.Clicked:
          _modifyView(null, null, _graphs[currentTabName], currentTabName);
          SwapGraphTab(currentTabName);
          break;

        case CustomTabEnums.NameChanged:
          currentTabName = GetNewGraphName(currentTabName);
          _tabs.Add(currentTabName, _tabs[oldTabName]);
          _tabs.Remove(oldTabName);
          _graphs.Add(currentTabName, _graphs[oldTabName]);
          _graphs.Remove(oldTabName);
          _modifyView(null, null, _graphs[currentTabName], currentTabName);
          SwapGraphTab(currentTabName);
          break;

        case CustomTabEnums.Closed:
          CustomTabControl tabToDelete = _tabs[currentTabName];
          GraphControl? graphToLoad = null;
          _tabs.Remove(currentTabName);
          _graphs.Remove(currentTabName);
          currentTabName = !_tabs.IsNullOrEmpty() ? _tabs.Last().Key : null;

          //If there is any tab left, switch it to be new current tab
          if (currentTabName != null)
          {
            graphToLoad = _graphs[currentTabName];
            _modifyView(tabToDelete, GraphTabActions.Delete, graphToLoad, currentTabName);
            SwapGraphTab(currentTabName);
          }
          else _modifyView(tabToDelete, GraphTabActions.Delete, null, currentTabName);
          break;

        default:
          break;
      }
    }

    /// <summary>
    /// Grays out all tabs and highlights only current tab
    /// </summary>
    /// <param name="tabName">Tab to highlight</param>
    private void SwapGraphTab(string tabName)
    {
      foreach (var customTabControl in _tabs.Values) customTabControl.BackgroundTabColor = SystemColors.MenuBarBrush;
      _tabs[tabName].BackgroundTabColor = SystemColors.ActiveCaptionBrush;
    }

    /// <summary>
    /// Iterates to find new available graph name
    /// </summary>
    /// <param name="graphName">Optional base name to create new name from</param>
    /// <returns>Name of new graph</returns>
    private string GetNewGraphName(string graphName = "New Graph")
    {
      string newGraphName = graphName;
      int tryCount = 1;
      while (_tabs.ContainsKey(newGraphName)) newGraphName = graphName + "_" + tryCount++;
      return newGraphName;
    }
  }
}