using System;

namespace ExpensesTracker.Models.Settings
{
  /// <summary>
  /// Class made for serialization all graph settings in one file
  /// </summary>
  [Serializable]
  public class GraphViewSettings
  {
    private GraphSettings? _graphSettings;
    private FilterSettings? _filterSettings;
    public GraphSettings? GraphSettings
    {
      get { return _graphSettings; }
      set { _graphSettings = value; }
    }
    public FilterSettings? FilterSettings
    {
      get { return _filterSettings; }
      set { _filterSettings = value; }
    }

  }
}
