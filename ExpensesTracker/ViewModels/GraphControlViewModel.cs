﻿using ExpensesTracker.Models.DataControllers.Graphs_Classes;
using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.Models.Settings;
using ExpensesTracker.Views.Controls;
using System.Collections.Generic;

namespace ExpensesTracker.ViewModels
{

  class GraphControlViewModel
  {
    private readonly GraphControl _graphControl;
    private readonly IMainSettings _mainSettings;
    private readonly FilterSettings _filterSettings = new();
    private readonly GraphSettings _graphSettings = new();

    private List<Expense> _data;
    private GraphWPF? _graph;

    public GraphControlViewModel(GraphControl graphControl, IMainSettings mainSettings)
    {
      _graphControl = graphControl;
      _mainSettings = mainSettings;
      _graphControl.GraphFilterCluster.SetNewSettingsRef(_filterSettings);
      _graphControl.GraphSettingsCluster.SetNewSettingsRef(_graphSettings);
      _data = _mainSettings.DatabaseRecords;
      _mainSettings.PropertyChanged += UpdateGraph;
      _filterSettings.PropertyChanged += UpdateGraph;
      _graphSettings.PropertyChanged += UpdateGraph;
      _mainSettings.PropertyChanged += UpdateGraph;
      ChangeGraphType();
    }
    public GraphControlViewModel(GraphControl graphControl, IMainSettings mainSettings, GraphViewSettings graphViewSettings)
    {
      _graphControl = graphControl;
      _mainSettings = mainSettings;
      if (graphViewSettings.GraphSettings != null) _graphSettings = graphViewSettings.GraphSettings;
      if (graphViewSettings.FilterSettings != null) _filterSettings = graphViewSettings.FilterSettings;
      _graphControl.GraphFilterCluster.SetExistingSettingsRef(_filterSettings);
      _graphControl.GraphSettingsCluster.SetExistingSettingsRef(_graphSettings);
      _data = _mainSettings.DatabaseRecords;
      _data = _filterSettings.ApplyFilterCriteria(_mainSettings.DatabaseRecords, -1);
      _mainSettings.PropertyChanged += UpdateGraph;
      _filterSettings.PropertyChanged += UpdateGraph;
      _graphSettings.PropertyChanged += UpdateGraph;
      _mainSettings.PropertyChanged += UpdateGraph;
      ChangeGraphType();
    }
    public GraphViewSettings GetGraphSettings() => new() { FilterSettings = _filterSettings, GraphSettings = _graphSettings };

    private void UpdateGraph(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "GraphType") ChangeGraphType();
      _data = _filterSettings.ApplyFilterCriteria(_mainSettings.DatabaseRecords, -1);
      _graph.UpdateGraph(_data);
    }
    private void ChangeGraphType()
    {
      _graphControl.Graph.Reset();
      switch (_graphSettings.GraphType)
      {
        case DataTypes.Enums.GraphTypes.BarGraph:
          _graph = new BarGraph(_graphSettings, _data, _graphControl.Graph);
          break;
        case DataTypes.Enums.GraphTypes.PieGraph:
          _graph = new PieGraph(_graphSettings, _data, _graphControl.Graph);
          break;
        default:
          break;
      }
    }
  }
}
