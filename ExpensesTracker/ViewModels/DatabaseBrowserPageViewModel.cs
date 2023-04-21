using ExpensesTracker.Models.DataControllers;
using ExpensesTracker.Views.Classes;
using ExpensesTracker.Views.Pages.DatabaseBrowser;
using System.Collections.ObjectModel;
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
      RefreshView(10);
    }

    public void RemoveRecord(DatabaseView itemToDelete) => DatabaseModel.DeleteDBRecord(itemToDelete.ReturnExpense());

    public void RefreshView(int numberOfItems = 1)
    {
      var listOfItems = new ObservableCollection<DatabaseView>();
      ;
      foreach (var item in DatabaseModel.ShowRange(null, true, numberOfItems)) listOfItems.Add(new DatabaseView(item));

      ExpensesItems = listOfItems;
      _databaseBrowserPage.DatabaseView.ItemsSource = ExpensesItems;
    }

    public void LoadMoreRecords()
    {
      _itemsToShow *= 10;
      if (_itemsToShow > 1000)
      {
        RefreshView(-1);
        _itemsToShow = 1;
        return;
      }
      RefreshView(_itemsToShow);
    }

    public void SearchRecord(string? name)
    {

    }
  }
}
