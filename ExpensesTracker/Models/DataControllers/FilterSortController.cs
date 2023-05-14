using ExpensesTracker.DataTypes;
using ExpensesTracker.Models.DataProviders;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExpensesTracker.Models.DataControllers
{
  public class FilterSortController : INotifyPropertyChanged
  {
    private bool _newFlag = false;

    private string? _name = null;
    private bool? _income = null;
    private bool? _recurring = null;
    private DecimalRange? _priceRange = null;
    private DecimalRange? _quantityRange = null;
    private DecimalRange? _totalRange = null;
    private DateRange? _submitDateRange = null;
    private DateRange? _updateDateRange = null;
    private DateRange? _userDateRange = null;
    private List<string>? _categories = null;
    private List<string>? _subcategories = null;
    private List<string>? _recurrances = null;

    public string? Name
    {
      get { return _name; }
      set { _name = value; OnPropertyChanged(nameof(Name)); }
    }
    public bool? Income
    {
      get { return _income; }
      set { _income = value; OnPropertyChanged(nameof(Income)); }
    }
    public bool? Recurring
    {
      get { return _recurring; }
      set { _recurring = value; OnPropertyChanged(nameof(Recurring)); }
    }
    public DecimalRange? PriceRange
    {
      get { return _priceRange; }
      set { _priceRange = value; OnPropertyChanged(nameof(PriceRange)); }
    }
    public DecimalRange? QuantityRange
    {
      get { return _quantityRange; }
      set { _quantityRange = value; OnPropertyChanged(nameof(QuantityRange)); }
    }
    public DecimalRange? TotalRange
    {
      get { return _totalRange; }
      set { _totalRange = value; OnPropertyChanged(nameof(TotalRange)); }
    }
    public DateRange? SubmitDateRange
    {
      get { return _submitDateRange; }
      set { _submitDateRange = value; OnPropertyChanged(nameof(SubmitDateRange)); }
    }
    public DateRange? UpdateDateRange
    {
      get { return _updateDateRange; }
      set { _updateDateRange = value; OnPropertyChanged(nameof(UpdateDateRange)); }
    }
    public DateRange? UserDateRange
    {
      get { return _userDateRange; }
      set { _userDateRange = value; OnPropertyChanged(nameof(UserDateRange)); }
    }
    public List<string>? Categories
    {
      get { return _categories; }
      set { _categories = value; OnPropertyChanged(nameof(Categories)); }
    }
    public List<string>? Subcategories
    {
      get { return _subcategories; }
      set { _subcategories = value; OnPropertyChanged(nameof(Subcategories)); }
    }
    public List<string>? Recurrances
    {
      get { return _recurrances; }
      set { _recurrances = value; OnPropertyChanged(nameof(Recurrances)); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Apply all active filters to list of Expenses
    /// </summary>
    /// <param name="expensesToFilter">List of records to filter</param>
    /// <param name="nuberOfRecordsToShow">Number of records in filtered list</param>
    /// <returns></returns>
    public List<Expense> ApplyFilterCriteria(List<Expense> expensesToFilter, int nuberOfRecordsToShow)
    {
      if (_name != null && _name != string.Empty) expensesToFilter = DatabaseModel.FilterByName(expensesToFilter, _name);
      if (_income != null) expensesToFilter = DatabaseModel.FilterByIncome(expensesToFilter, (bool)_income);
      if (_recurring != null) expensesToFilter = DatabaseModel.FilterByRecurrence(expensesToFilter, (bool)_recurring);
      if (_priceRange != null) expensesToFilter = DatabaseModel.FilterByPrice(expensesToFilter, _priceRange);
      if (_quantityRange != null) expensesToFilter = DatabaseModel.FilterByQuantity(expensesToFilter, _quantityRange);
      if (_totalRange != null) expensesToFilter = DatabaseModel.FilterByTotal(expensesToFilter, _totalRange);
      if (_submitDateRange != null) expensesToFilter = DatabaseModel.FilterBySubmitDate(expensesToFilter, _submitDateRange);
      if (_updateDateRange != null) expensesToFilter = DatabaseModel.FilterByUpdateDate(expensesToFilter, _updateDateRange);
      if (_userDateRange != null) expensesToFilter = DatabaseModel.FilterByUserDate(expensesToFilter, _userDateRange);
      if (_categories != null) expensesToFilter = DatabaseModel.FilterByCategories(expensesToFilter, _categories);
      if (_subcategories != null) expensesToFilter = DatabaseModel.FilterBySubcategories(expensesToFilter, _subcategories);
      if (_recurrances != null) expensesToFilter = DatabaseModel.FilterByRecurrenceNames(expensesToFilter, _recurrances);
      expensesToFilter = DatabaseModel.FilterByRange(expensesToFilter, true, nuberOfRecordsToShow);
      return expensesToFilter;
    }

    /// <summary>
    /// Clear all active filters and trigger view update, by setting last property without newFlag
    /// </summary>
    public void ClearAllFilters()
    {
      _newFlag = true;
      Name = null;
      Income = null;
      Recurring = null;
      PriceRange = null;
      QuantityRange = null;
      TotalRange = null;
      SubmitDateRange = null;
      UpdateDateRange = null;
      UserDateRange = null;
      Categories = null;
      Subcategories = null;
      //Only one update is needed
      _newFlag = false;
      Recurrances = null;

    }

    /// <summary>
    /// Notify about property changed, except of clearing filters
    /// </summary>
    /// <param name="propertyName">Changed property</param>
    private void OnPropertyChanged(string propertyName)
    {
      if (!_newFlag) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}