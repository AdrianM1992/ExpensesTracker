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
    private readonly GraphDataController _graphDataController = new();
    private List<Expense> _data;

    private GraphWPF _graph;
    public GraphControlViewModel(GraphControl graphControl, IMainSettings mainSettings)
    {

      _graphControl = graphControl;
      _mainSettings = mainSettings;
      _graphControl.GraphFilterCluster.SetFilterControllerRef(_filterSortController);
      _graphControl.GraphControlSelectros.SetGraphSettingsReference(_graphSettings);
      _data = _mainSettings.DatabaseRecords;
      _mainSettings.PropertyChanged += MainSettings_PropertyChanged;
      _filterSortController.PropertyChanged += FilterSortController_PropertyChanged;
      _graphSettings.PropertyChanged += GraphSettings_PropertyChanged;
      Test();
    }
    //maybe use only one handler?
    private void MainSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      _data = _filterSortController.ApplyFilterCriteria(_mainSettings.DatabaseRecords, -1);
      _graph.UpdateGraph(_data);
    }

    private void FilterSortController_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      _data = _filterSortController.ApplyFilterCriteria(_mainSettings.DatabaseRecords, -1);
      _graph.UpdateGraph(_data);
    }

    private void GraphSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      _graph.UpdateGraph(_data);
      _graphControl.Graph = _graph;
    }

    public void Test()
    {
      _graph = new BarGraph(_graphSettings, _data);
      _graphControl.Graph = _graph;
    }
  }
}
