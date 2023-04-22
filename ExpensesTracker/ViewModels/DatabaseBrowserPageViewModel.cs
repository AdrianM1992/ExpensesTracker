using ExpensesTracker.Models.DataControllers;
using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.Views.Classes;
using ExpensesTracker.Views.Pages.DatabaseBrowser;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace ExpensesTracker.ViewModels
{
  class DatabaseBrowserPageViewModel
  {
    private readonly DatabaseBrowserPage _databaseBrowserPage;
    private int _itemsToShow = 10;
    private List<Expense> _expensesItems;
    public ObservableCollection<DatabaseView> DatabaseViewItems { get; private set; }


    public DatabaseBrowserPageViewModel(Page page)
    {
      _databaseBrowserPage = (DatabaseBrowserPage)page;
      _expensesItems = DatabaseModel.SearchByRange(null, true, -1);
      ShowRecords();
    }
    public void AddedRecord()
    {
      _expensesItems = DatabaseModel.SearchByRange(null, true, -1);
      ShowRecords();
    }
    public void RemoveRecord(DatabaseView itemToDelete)
    {
      DatabaseModel.DeleteDBRecord(itemToDelete.ReturnExpense());
      _expensesItems = DatabaseModel.SearchByRange(null, true, -1);
      ShowRecords();
    }

    public void ShowRecords()
    {
      var listOfItems = new ObservableCollection<DatabaseView>();
      foreach (var item in _expensesItems.Take(_itemsToShow)) listOfItems.Add(new DatabaseView(item));

      DatabaseViewItems = listOfItems;
      _databaseBrowserPage.DatabaseView.ItemsSource = DatabaseViewItems;
    }
    public void LoadMoreRecords()
    {
      if (_itemsToShow == _expensesItems.Count && _itemsToShow != 10 && _itemsToShow != 100 && _itemsToShow != 1000) _itemsToShow = 1;
      _itemsToShow *= 10;
      if (_itemsToShow > 1000) _itemsToShow = _expensesItems.Count;
      ShowRecords();
    }

    public void SearchRecord(string name)
    {
      _expensesItems = DatabaseModel.SearchByName(null, name);
      ShowRecords();
    }
  }
}
