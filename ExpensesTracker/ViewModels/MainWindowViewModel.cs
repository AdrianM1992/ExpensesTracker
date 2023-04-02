namespace ExpensesTracker.ViewModels
{
  internal class MainWindowViewModel
  {
    MainWindow _mainWindow;

    /// <summary>
    /// Singleton pattern implementation
    /// </summary>
    private static MainWindowViewModel _instance;
    private MainWindowViewModel(MainWindow window)
    {
      _mainWindow = window;
    }
    public static MainWindowViewModel GetMainWindowViewModel(MainWindow window)
    {
      if (_instance == null)
      {
        _instance = new MainWindowViewModel(window);
        return _instance;
      }
      else return _instance;
    }
    public void Set()
    {
      _mainWindow.MinMaxClose.Width = _mainWindow.TopBar.ActualWidth - _mainWindow.Tabs.ActualWidth;
    }
  }
}
