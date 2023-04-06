namespace ExpensesTracker.ViewModels
{
  internal class MainWindowViewModel
  {
    MainWindow _mainWindow;

    /// <summary>
    /// Singleton pattern implementation
    /// </summary>
    private static MainWindowViewModel _instance;
    private MainWindowViewModel()
    {

    }

    public static MainWindowViewModel GetMainWindowViewModel()
    {
      if (_instance == null)
      {
        _instance = new MainWindowViewModel();
        return _instance;
      }
      else return _instance;
    }
  }
}
