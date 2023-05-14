using ExpensesTracker.DataTypes;
using ExpensesTracker.Models.DataProviders;
using System;
using System.Collections.Generic;

namespace ExpensesTracker.Models.DataControllers.Graphs_Classes
{
  abstract class GraphWPF
  {
    protected readonly GraphSettings _settings;
    protected List<Expense>? _data;
    protected ScottPlot.WpfPlot _plot;
    protected List<DateRange> _xAxisValues = new();
    protected double?[,] _yAxisValues = new double?[0, 0];
    protected string[] _legendLabels = Array.Empty<string>();
    public List<Expense>? Data
    {
      get { return _data; }
      private set { _data = value; }
    }

    public GraphWPF(GraphSettings settings, List<Expense>? data, ScottPlot.WpfPlot plot)
    {
      _settings = settings;
      Data = data;
      _plot = plot;
      PlotGraph();
    }

    public void UpdateGraph(List<Expense> newData)
    {
      Data = newData;
      PlotGraph();
    }
    protected abstract void PlotGraph();
    /// <summary>
    /// Based on graph type, calculates ranges to be shown on graph
    /// </summary>
    /// <returns>List of DateRange</returns>
    protected abstract List<DateRange> CalculateXAxisIntervals();
    /// <summary>
    /// Calculates YAxisValues based on graphSettings
    /// </summary>
    /// <returns>Array of values, where columns refer to ranges</returns>
    protected abstract double?[,] CalculateYAxisValues();
    protected void SetTitle() => _plot.Plot.Title(_settings.GraphName, size: 24);
  }
}
