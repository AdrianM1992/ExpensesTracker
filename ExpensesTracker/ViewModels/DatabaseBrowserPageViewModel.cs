using ExpensesTracker.Models.DataControllers;
using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.Models.Interfaces;
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
    private readonly IMainSettings _mainSettings;
    private int _itemsToShow = 10;
    private bool _allItems = false;
    private List<Expense> _expensesItems;
    private readonly FilterSortController _filterSortController = new();

    public ObservableCollection<DatabaseView> DatabaseViewItems { get; private set; } = new();

    private DatabaseBrowserPageViewModel(DatabaseBrowserPage page, IMainSettings mainSettings)
    {
      _databaseBrowserPage = page;
      _mainSettings = mainSettings;
      _mainSettings.PropertyChanged += MainSettings_PropertyChanged;
      _expensesItems = _mainSettings.DatabaseRecords;
      _databaseBrowserPage.FilterCluster.SetFilterControllerRef(_filterSortController);
      _filterSortController.PropertyChanged += FilterSortController_PropertyChanged;
      ShowRecords();
    }

    private void MainSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      _expensesItems = _mainSettings.DatabaseRecords;
      if (_allItems)
      {
        _itemsToShow = _expensesItems.Count;
        string textToShow = $"All ({_itemsToShow})";
        _databaseBrowserPage.NumberOfItems.Text = textToShow;
      }
      ShowRecords();
    }

    /// <summary>
    /// Implementation of singleton pattern
    /// </summary>
    /// <param name="page">DatabaseBrowserPage reference</param>
    /// <returns>Reference to DatabaseBrowserPageViewModel</returns>
    public static DatabaseBrowserPageViewModel GetDatabaseBrowserPageViewModelRef(DatabaseBrowserPage page, IMainSettings mainSettings)
    {
      if (_instance == null) return _instance = new DatabaseBrowserPageViewModel(page, mainSettings);
      else return _instance;
    }

    public void RemoveRecord(DatabaseView itemToDelete) => DatabaseModel.DeleteDBRecord(itemToDelete.ReturnExpense());


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
        _allItems = true;
        textToShow = $"All ({_itemsToShow})";
      }
      else
      {
        textToShow = _itemsToShow.ToString();
        _allItems = false;
      }

      _databaseBrowserPage.NumberOfItems.Text = textToShow;
      ShowRecords();
    }

    private void FilterSortController_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) => ShowRecords();
  }
}