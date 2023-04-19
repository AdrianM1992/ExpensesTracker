using ExpensesTracker.Models.DataControllers;
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

    public ObservableCollection<DatabaseView> ExpensesItems { get; private set; }


    public DatabaseBrowserPageViewModel(Page page)
    {
      _databaseBrowserPage = (DatabaseBrowserPage)page;
      ExpensesItems = GenerateListOfDataviews(false, 100);
    }

    public void RemoveRecord(DatabaseView itemToDelete)
    {
      DatabaseModel.DeleteDBRecord(itemToDelete.ReturnExpense());
    }

    private ObservableCollection<DatabaseView> GenerateListOfDataviews(bool wholeDB, int numberOfItems = 1)
    {
      var listToReturn = new ObservableCollection<DatabaseView>();
      using var db = new ExpensesContext();

      if (wholeDB || db.Expenses.Count() < numberOfItems) numberOfItems = db.Expenses.Count();

      for (int i = 0; i < numberOfItems; i++) listToReturn.Add(new DatabaseView(db.Expenses.ToArray()[i]));

      return listToReturn;
    }

    public void RefreshView()
    {
      ExpensesItems = GenerateListOfDataviews(false, 100);
      _databaseBrowserPage.DatabaseView.ItemsSource = ExpensesItems;
    }
  }
}
