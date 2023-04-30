namespace ExpensesTracker.ViewModels
{
  internal class MainWindowViewModel
  {
    private readonly MainWindow _mainWindow;
    /// <summary>
    /// Singleton pattern implementation
    /// </summary>
    private static MainWindowViewModel? _instance;
    private MainWindowViewModel(MainWindow window)
    {
      _mainWindow = window;
    }

    /// <summary>
    /// Implementation of singleton pattern
    /// </summary>
    /// <param name="window">MainWindow reference</param>
    /// <returns>Reference to MainWindowViewModel</returns>
    public static MainWindowViewModel GetMainWindowViewModel(MainWindow window)
    {
      if (_instance == null) return _instance = new MainWindowViewModel(window);
      else return _instance;
    }
  }
}
