using ExpensesTracker.Models.DataControllers;
using ExpensesTracker.Views.Controls;

namespace ExpensesTracker.ViewModels
{

  class GraphControlViewModel
  {
    private readonly GraphControl _graphControl;
    private readonly FilterSortController _filterSortController = new();
    private readonly GraphSettings _graphSettings = new();
    private readonly GraphDataController _graphDataController = new();
    public GraphControlViewModel(GraphControl graphControl)
    {
      _graphControl = graphControl;
      _graphControl.GraphFilterCluster.SetFilterControllerRef(_filterSortController);
      _graphControl.GraphControlSelectros.SetGraphSettingsReference(_graphSettings);
      _filterSortController.PropertyChanged += FilterSortController_PropertyChanged;
      _graphSettings.PropertyChanged += GraphSettings_PropertyChanged;
    }

    private void FilterSortController_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      throw new System.NotImplementedException();
    }

    private void GraphSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      throw new System.NotImplementedException();
    }
  }
}
