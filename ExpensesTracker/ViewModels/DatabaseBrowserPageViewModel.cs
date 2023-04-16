using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.ViewModels.Converters;
using ExpensesTracker.Views.Classes;
using ExpensesTracker.Views.Pages.DatabaseBrowser;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace ExpensesTracker.ViewModels
{
  class DatabaseBrowserPageViewModel
  {
    private readonly DatabaseBrowserPage _databaseBrowserPage;
    public ObservableCollection<DatabaseView> Expenses { get; set; }

    public DatabaseBrowserPageViewModel(Page page)
    {
      _databaseBrowserPage = page as DatabaseBrowserPage;
      using (var db = new ExpensesContext())
      {
        var count = db.Expenses.Count();
        if (count >= 100)
        {
          Expenses = new ObservableCollection<DatabaseView>(DatabaseViewConverters.DbToViewConverter(db.Expenses.TakeLast(100).ToList()));
        }
        else
        {
          Expenses = new ObservableCollection<DatabaseView>(DatabaseViewConverters.DbToViewConverter(db.Expenses.Take(count).ToList()));
        }
      }
    }
  }
}
