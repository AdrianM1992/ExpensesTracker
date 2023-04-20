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
    private int _itemsToShow = 10;
    public ObservableCollection<DatabaseView> ExpensesItems { get; private set; }


    public DatabaseBrowserPageViewModel(Page page)
    {
      _databaseBrowserPage = (DatabaseBrowserPage)page;
      RefreshView(false, 10);
    }

    public void RemoveRecord(DatabaseView itemToDelete) => DatabaseModel.DeleteDBRecord(itemToDelete.ReturnExpense());

    public void RefreshView(bool wholeDB = false, int numberOfItems = 1)
    {
      var listToReturn = new ObservableCollection<DatabaseView>();
      using var db = new ExpensesContext();

      if (wholeDB || db.Expenses.Count() < numberOfItems) numberOfItems = db.Expenses.Count();
      var lastItems = db.Expenses.ToList().TakeLast(numberOfItems).Reverse();
      foreach (var item in lastItems) listToReturn.Add(new DatabaseView(item));

      ExpensesItems = listToReturn;
      _databaseBrowserPage.DatabaseView.ItemsSource = ExpensesItems;
    }

    public void LoadMoreRecords()
    {
      _itemsToShow *= 10;
      if (_itemsToShow > 1000)
      {
        RefreshView(true);
        _itemsToShow = 1;
        return;
      }
      RefreshView(false, _itemsToShow);
    }
  }
}
