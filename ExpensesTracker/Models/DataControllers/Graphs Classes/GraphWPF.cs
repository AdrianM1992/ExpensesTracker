using ExpensesTracker.DataTypes;
using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.DataProviders;
using System.Collections.Generic;

namespace ExpensesTracker.Models.DataControllers.Graphs_Classes
{
  abstract class GraphWPF : ScottPlot.WpfPlot
  {
    protected readonly GraphSettings _settings;
    protected List<Expense>? _data;
    protected List<DateRange> _xAxisValues = new();
    protected decimal?[,] _yAxisValues = new decimal?[0, 0];

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
    /// <summary>
    /// Based on graph type, calculates ranges to be shown on graph
    /// </summary>
    /// <returns>List of DateRange</returns>
    protected abstract List<DateRange> CalculateXAxisIntervals();
    /// <summary>
    /// Based on graph type calculates ranges to be shown on graph
    /// </summary>
    /// <param name="dateRange">User defined DateRange</param>
    /// <param name="interval">Division specifier</param>
    /// <returns>List of DateRange</returns>
    protected abstract List<DateRange> CalculateCustomXAxisIntervals(DateRange dateRange, TimeDivisionIntervals interval);
    /// <summary>
    /// Calculates YAxisValues based on graphSettings
    /// </summary>
    /// <returns>Array of values, where columns refer to ranges</returns>
    protected abstract decimal?[,] CalculateYAxisValues();

    protected void SetTitle() => Plot.Title(_settings.GraphName, size: 16);
  }
}
