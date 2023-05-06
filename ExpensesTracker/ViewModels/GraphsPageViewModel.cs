using ExpensesTracker.Views.Pages.Graphs;

namespace ExpensesTracker.ViewModels
{
  class GraphsPageViewModel
  {
    private readonly GraphsPage _graphsPage;
    private static GraphsPageViewModel? _instance;

    private GraphsPageViewModel(GraphsPage page)
    {
      _graphsPage = page;
    }

    /// <summary>
    /// Implementation of singleton pattern
    /// </summary>
    /// <param name="page">GraphsPage reference</param>
    /// <returns>Reference to GraphsPageViewModel</returns>
    public static GraphsPageViewModel GetGraphsPageViewModelRef(GraphsPage page)
    {
      if (_instance == null) return _instance = new GraphsPageViewModel(page);
      else return _instance;
    }
  }
}
