using ExpensesTracker.Models.DataProviders;
using System.Collections.Generic;

namespace ExpensesTracker.Models.DataControllers.Graphs_Classes
{
  abstract class GraphWPF : ScottPlot.WpfPlot
  {
    protected readonly GraphSettings _settings;
    protected List<Expense>? _data;

    public List<Expense>? Data
    {
      get { return _data; }
      private set { _data = value; }
    }

    public GraphWPF(GraphSettings settings, List<Expense>? data)
    {
      _settings = settings;
      Data = data;
      PlotGraph();
    }

    public void UpdateGraph(List<Expense> newData)
    {
      Data = newData;
      PlotGraph();
    }
    protected abstract void PlotGraph();
    protected abstract double[] CalculateYAxisValues();

    protected void SetTitle() => Plot.Title(_settings.GraphName, size: 16);
  }
}
