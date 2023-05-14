using ExpensesTracker.Models.DataControllers;
using ExpensesTracker.Models.DataControllers.Graphs_Classes;
using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.Views.Controls;
using System.Collections.Generic;

namespace ExpensesTracker.ViewModels
{

  class GraphControlViewModel
  {
    private readonly GraphControl _graphControl;
    private readonly IMainSettings _mainSettings;
    private readonly FilterSortController _filterSortController = new();
    private readonly GraphSettings _graphSettings = new();
    private List<Expense> _data;

    private GraphWPF _graph;
    public GraphControlViewModel(GraphControl graphControl, IMainSettings mainSettings)
    {
      _graphControl = graphControl;
      _mainSettings = mainSettings;
      _graphControl.GraphFilterCluster.SetFilterControllerRef(_filterSortController);
      _graphControl.GraphControlSelectros.SetGraphSettingsReference(_graphSettings);
      _data = _mainSettings.DatabaseRecords;
      _mainSettings.PropertyChanged += UpdateGraph;
      _filterSortController.PropertyChanged += UpdateGraph;
      _graphSettings.PropertyChanged += UpdateGraph;
      _mainSettings.PropertyChanged += UpdateGraph;
      _graph = new BarGraph(_graphSettings, _data, _graphControl.Graph);
    }

    private void UpdateGraph(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "GraphType") ChangeGraphType();
      _data = _filterSortController.ApplyFilterCriteria(_mainSettings.DatabaseRecords, -1);
      _graph.UpdateGraph(_data);
    }

    private void ChangeGraphType()
    {
      _graphControl.Graph.Reset();
      switch (_graphSettings.GraphType)
      {
        case DataTypes.Enums.GraphTypes.BarGraph:
          _graph = _graph = new BarGraph(_graphSettings, _data, _graphControl.Graph);
          break;
        case DataTypes.Enums.GraphTypes.PieGraph:
          _graph = _graph = new PieGraph(_graphSettings, _data, _graphControl.Graph);
          break;
        default:
          break;
      }
    }
  }
}
