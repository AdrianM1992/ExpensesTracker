using ExpensesTracker.Models.DataProviders;
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
      _databaseBrowserPage = (DatabaseBrowserPage)page;
      using var db = new ExpensesContext();
      var count = db.Expenses.Count();
      var databaseViews = new ObservableCollection<DatabaseView>();
      if (count >= 100)
      {
        foreach (var expense in db.Expenses.TakeLast(100).ToList())
        {
          databaseViews.Add(new DatabaseView(expense));
        }
      }
      else
      {
        foreach (var expense in db.Expenses)
        {
          databaseViews.Add(new DatabaseView(expense));
        }
      }

      Expenses = databaseViews;
    }
  }
}
