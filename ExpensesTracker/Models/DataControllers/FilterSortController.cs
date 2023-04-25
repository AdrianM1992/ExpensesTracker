using ExpensesTracker.DataTypes;
using ExpensesTracker.Models.DataProviders;
using System.Collections.Generic;

namespace ExpensesTracker.Models.DataControllers
{
  class FilterSortController
  {
    private string? _name = null;
    private bool? _income = null;
    private bool? _recurring = null;
    private DecimalRange? _priceRange = null;
    private DecimalRange? _quantityRange = null;
    private DecimalRange? _totalRange = null;
    private DateRange? _submitDateRange = null;
    private DateRange? _updateDateRange = null;
    private DateRange? _userDateRange = null;
    private List<int>? _categories = null;
    private List<int>? _subcategories = null;
    private List<int>? _recurrances = null;

    public string? Name
    {
      get { return _name; }
      set { _name = value; }
    }
    public bool? Income
    {
      get { return _income; }
      set { _income = value; }
    }
    public bool? Recurring
    {
      get { return _recurring; }
      set { _recurring = value; }
    }
    public DecimalRange? PriceRange
    {
      get { return _priceRange; }
      set { _priceRange = value; }
    }
    public DecimalRange? QuantityRange
    {
      get { return _quantityRange; }
      set { _quantityRange = value; }
    }
    public DecimalRange? TotalRange
    {
      get { return _totalRange; }
      set { _totalRange = value; }
    }
    public DateRange? SubmitDateRange
    {
      get { return _submitDateRange; }
      set { _submitDateRange = value; }
    }
    public DateRange? UpdateDateRange
    {
      get { return _updateDateRange; }
      set { _updateDateRange = value; }
    }
    public DateRange? UserDateRange
    {
      get { return _userDateRange; }
      set { _userDateRange = value; }
    }
    public List<int>? Categories
    {
      get { return _categories; }
      set { _categories = value; }
    }
    public List<int>? Subcategories
    {
      get { return _subcategories; }
      set { _subcategories = value; }
    }
    public List<int>? Recurrances
    {
      get { return _recurrances; }
      set { _recurrances = value; }
    }

    public List<Expense> ApplyFilterCriteria(List<Expense> expensesToFilter, int nuberOfRecordsToShow)
    {
      if (_name != null && _name != string.Empty) expensesToFilter = DatabaseModel.FilterByName(expensesToFilter, _name);
      if (_income != null) expensesToFilter = DatabaseModel.FilterByIncome(expensesToFilter, (bool)_income);
      if (_recurring != null) expensesToFilter = DatabaseModel.FilterByRecurrance(expensesToFilter, (bool)_recurring);
      if (_priceRange != null) expensesToFilter = DatabaseModel.FilterByPrice(expensesToFilter, _priceRange);
      if (_quantityRange != null) expensesToFilter = DatabaseModel.FilterByQuantity(expensesToFilter, _quantityRange);
      if (_totalRange != null) expensesToFilter = DatabaseModel.FilterByTotal(expensesToFilter, _totalRange);
      if (_submitDateRange != null) expensesToFilter = DatabaseModel.FilterBySubmitDate(expensesToFilter, _submitDateRange);
      if (_updateDateRange != null) expensesToFilter = DatabaseModel.FilterByUpdateDate(expensesToFilter, _updateDateRange);
      if (_userDateRange != null) expensesToFilter = DatabaseModel.FilterByUserDate(expensesToFilter, _userDateRange);
      if (_categories != null) expensesToFilter = DatabaseModel.FilterByCategories(expensesToFilter, _categories);
      if (_subcategories != null) expensesToFilter = DatabaseModel.FilterBySubcategories(expensesToFilter, _subcategories);
      if (_recurrances != null) expensesToFilter = DatabaseModel.FilterByRecurranceNames(expensesToFilter, _recurrances);
      expensesToFilter = DatabaseModel.FilterByRange(expensesToFilter, true, nuberOfRecordsToShow);
      return expensesToFilter;
    }
  }
}

