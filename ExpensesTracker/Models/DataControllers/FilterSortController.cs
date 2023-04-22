using ExpensesTracker.DataTypes;
using System.Collections.Generic;

namespace ExpensesTracker.Models.DataControllers
{
  class FilterSortController
  {
    private string _name;
    private bool _income;
    private bool _recurring;
    private DecimalRange _priceRange;
    private DecimalRange _quantityRange;
    private List<int> _categories;
    private List<int> _subcategories;
    private DecimalRange _totalRange;
    private DateRange _submitDateRange;
    private DateRange _updateDateRange;
    private DateRange _userDateRange;

    public bool Income
    {
      get { return _income; }
      set { _income = value; }
    }
    public bool Recurring
    {
      get { return _recurring; }
      set { _recurring = value; }
    }
    public DecimalRange PriceRange
    {
      get { return _priceRange; }
      set { _priceRange = value; }
    }
    public List<int> Categories
    {
      get { return _categories; }
      set { _categories = value; }
    }
    public List<int> Subcategories
    {
      get { return _subcategories; }
      set { _subcategories = value; }
    }
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
    public DecimalRange QuantityRange
    {
      get { return _quantityRange; }
      set { _quantityRange = value; }
    }
    public DecimalRange TotalRange
    {
      get { return _totalRange; }
      set { _totalRange = value; }
    }
    public DateRange SubmitDateRange
    {
      get { return _submitDateRange; }
      set { _submitDateRange = value; }
    }
    public DateRange UpdateDateRange
    {
      get { return _updateDateRange; }
      set { _updateDateRange = value; }
    }
    public DateRange UserDateRange
    {
      get { return _userDateRange; }
      set { _userDateRange = value; }
    }
  }

}

