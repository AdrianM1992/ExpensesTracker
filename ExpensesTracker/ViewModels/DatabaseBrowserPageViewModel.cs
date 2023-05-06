using ExpensesTracker.Models.DataControllers;
using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.Views.Classes;
using ExpensesTracker.Views.Pages.DatabaseBrowser;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExpensesTracker.ViewModels
{
  class DatabaseBrowserPageViewModel
  {
    private readonly DatabaseBrowserPage _databaseBrowserPage;
    private static DatabaseBrowserPageViewModel? _instance;
    private int _itemsToShow = 10;
    private List<Expense> _expensesItems;
    private readonly FilterSortController _filterSortController = new();

    public ObservableCollection<DatabaseView> DatabaseViewItems { get; private set; } = new();

    private DatabaseBrowserPageViewModel(DatabaseBrowserPage page)
    {

      _databaseBrowserPage = page;
      _expensesItems = DatabaseModel.FilterByRange(null, true, -1);
      _databaseBrowserPage.FilterCluster.SetFilterControllerRef(_filterSortController);
      _filterSortController.PropertyChanged += FilterSortController_PropertyChanged;
      DatabaseModel.SubtablesChanged += DatabaseModel_SubtablesChanged;
      ShowRecords();
    }

    /// <summary>
    /// Implementation of singleton pattern
    /// </summary>
    /// <param name="page">DatabaseBrowserPage reference</param>
    /// <returns>Reference to DatabaseBrowserPageViewModel</returns>
    public static DatabaseBrowserPageViewModel GetDatabaseBrowserPageViewModelRef(DatabaseBrowserPage page)
    {
      if (_instance == null) return _instance = new DatabaseBrowserPageViewModel(page);
      else return _instance;
    }

    public void AddedRecord()
    {
      _expensesItems = DatabaseModel.FilterByRange(null, true, -1);
      ShowRecords();
    }
    public void RemoveRecord(DatabaseView itemToDelete)
    {
      DatabaseModel.DeleteDBRecord(itemToDelete.ReturnExpense());
      _expensesItems = DatabaseModel.FilterByRange(null, true, -1);
      ShowRecords();
    }

    /// <summary>
    /// Updates database view by applying all selected filters to list of records
    /// </summary>
    public void ShowRecords()
    {
      var listOfItems = new ObservableCollection<DatabaseView>();
      foreach (var item in _filterSortController.ApplyFilterCriteria(_expensesItems, _itemsToShow)) listOfItems.Add(new DatabaseView(item, true));

      DatabaseViewItems = listOfItems;
      _databaseBrowserPage.DatabaseView.ItemsSource = DatabaseViewItems;
    }

    /// <summary>
    /// Changes number of visible records in database view
    /// </summary>
    public void LoadMoreRecords()
    {
      if (_itemsToShow == _expensesItems.Count && _itemsToShow != 10 && _itemsToShow != 100 && _itemsToShow != 1000) _itemsToShow = 1;
      _itemsToShow *= 10;
      string? textToShow;
      if (_itemsToShow > 1000)
      {
        _itemsToShow = _expensesItems.Count;
        textToShow = $"All ({_itemsToShow})";
      }
      else textToShow = _itemsToShow.ToString();

      _databaseBrowserPage.NumberOfItems.Text = textToShow;
      ShowRecords();
    }

    private void DatabaseModel_SubtablesChanged(object? sender, System.EventArgs e)
    {
      _expensesItems = DatabaseModel.FilterByRange(null, true, -1);
    }
    private void FilterSortController_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) => ShowRecords();
  }
}