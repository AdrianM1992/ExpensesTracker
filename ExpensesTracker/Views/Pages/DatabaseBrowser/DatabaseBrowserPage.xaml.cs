using ExpensesTracker.ViewModels;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Pages.DatabaseBrowser
{
  /// <summary>
  /// Logic for DatabaseBrowserPage.xaml
  /// </summary>
  public partial class DatabaseBrowserPage : Page
  {
    DatabaseBrowserPageViewModel _viewModel;

    public DatabaseBrowserPage()
    {
      InitializeComponent();
      _viewModel = new DatabaseBrowserPageViewModel();
      DataContext = _viewModel;
    }
  }
}
