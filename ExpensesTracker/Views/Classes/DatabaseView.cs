using ExpensesTracker.Models.DataProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;

namespace ExpensesTracker.Views.Classes
{
  /// <summary>
  /// Class with properties adapted to be displayed
  /// </summary>
  public class DatabaseView
  {
    private decimal _price;
    private decimal _quantity;
    private bool _recurring;
    private Expense? _expense = null;

    public int ID { get; private set; }
    public string Name { get; set; } = "";
    public decimal Price { get => _price; set { Total = value * Quantity; _price = value; } }
    public decimal Quantity { get => _quantity; set { Total = Price * value; _quantity = value; } }
    public decimal? Total { get; private set; } = 1M;
    public DateTime DateOfEntry { get; private set; }
    public DateTime LastUpdate { get; private set; }
    public DateTime? Date { get; set; } = null;
    public string Category { get; set; } = "None";
    public string? Subcategory { get; set; } = null;
    public bool Income { get; set; } = true;
    public bool Recurring { get => _recurring; set { if (!value) RecurringId = null; _recurring = value; } }
    public string? RecurringId { get; set; } = null;
    public string? Description { get; set; } = null;


    public DatabaseView()
    {
      DateOfEntry = DateTime.Now;
      LastUpdate = DateTime.Now;
      Price = 1M;
      Quantity = 1M;
      Recurring = false;
    }
    public DatabaseView(Expense expense)
    {
      _expense = expense;
      ID = expense.Id;
      Name = expense.Name;
      Price = expense.Price;
      Quantity = expense.Quantity;
      Total = expense.Total;
      DateOfEntry = expense.DateOfEntry;
      LastUpdate = expense.LastUpdate;
      Date = expense.Date;
      Subcategory = GetSubategoryName(expense);
      Income = expense.Income;
      Recurring = expense.Recurring;
      Description = expense.Description;
      Category = GetCategoryName(expense);
      RecurringId = GetRecurringName(expense);
    }

    /// <summary>
    /// Converts this instance to Expanse instance
    /// </summary>
    /// <returns>Expanse instance</returns>
    public Expense ReturnExpense()
    {
      if (_expense != null)
      {
        _expense.Name = Name;
        _expense.Price = Price;
        _expense.Quantity = Quantity;
        _expense.Total = Total;
        _expense.DateOfEntry = DateOfEntry;
        _expense.LastUpdate = DateTime.Now;
        _expense.Date = Date;
        _expense.SubcategoryId = GetSubategoryId();
        _expense.Income = Income;
        _expense.Recurring = Recurring;
        _expense.Description = Description;
        _expense.CategoryId = GetCategoryId();
        _expense.RecurringId = GetRecurringId();
        return _expense;
      }
      else
      {
        return new Expense()
        {
          Name = Name,
          Price = Price,
          Quantity = Quantity,
          Total = Total,
          DateOfEntry = DateOfEntry,
          LastUpdate = DateTime.Now,
          Date = Date,
          SubcategoryId = GetSubategoryId(),
          Income = Income,
          Recurring = Recurring,
          Description = Description,
          CategoryId = GetCategoryId(),
          RecurringId = GetRecurringId()
        };
      }
    }

    /// <summary>
    /// Gets category name of expense or income
    /// </summary>
    /// <param name="expense">Record with reference to categoryId</param>
    /// <returns>Name of category expense or income</returns>
    private string GetCategoryName(Expense expense)
    {
      using var cat = new ExpensesContext();
      var categoryNames = from c in cat.Categories.ToList()
                          where c.Id == expense.CategoryId
                          select c.Name;
      return categoryNames.First();
    }

    /// <summary>
    /// Gets subcategory name of expense or income
    /// </summary>
    /// <param name="expense">Record with reference to categoryId</param>
    /// <returns>Name of category expense or income</returns>
    private string? GetSubategoryName(Expense expense)
    {
      using var subcat = new ExpensesContext();
      var subcategoryNames = from s in subcat.Subcategories.ToList()
                             where s.Id == expense.SubcategoryId
                             select s.Name;
      if (subcategoryNames.IsNullOrEmpty()) return null;
      else return subcategoryNames.First();
    }

    /// <summary>
    /// Gets recurring name of expense or income
    /// </summary>
    /// <param name="expense">Record with reference to recurringId</param>
    /// <returns>Name of recurring expense or income</returns>
    private string? GetRecurringName(Expense expense)
    {
      if (expense.Recurring)
      {
        using var rec = new ExpensesContext();
        var recurringNames = from r in rec.Recurrings.ToList()
                             where r.Id == expense.RecurringId
                             select r.Name;
        if (recurringNames.IsNullOrEmpty()) return "";
        else return recurringNames.First();
      }
      else return "";
    }

    /// <summary>
    /// Gets category ID of expense or income
    /// </summary>
    /// <returns>Name of category expense or income</returns>
    private short GetCategoryId()
    {
      using var cat = new ExpensesContext();
      var categoryIds = from c in cat.Categories.ToList()
                        where c.Name == Category
                        select c.Id;
      return categoryIds.First();
    }

    /// <summary>
    /// Gets subcategory ID of expense or income
    /// </summary>
    /// <returns>Name of category expense or income</returns>
    private int? GetSubategoryId()
    {
      using var subcat = new ExpensesContext();
      var subcategoryIds = from s in subcat.Subcategories.ToList()
                           where s.Name == Subcategory
                           select s.Id;
      if (subcategoryIds.IsNullOrEmpty()) return null;
      else return subcategoryIds.First();
    }

    /// <summary>
    /// Gets recurring ID of expense or income
    /// </summary>
    /// <returns>Name of recurring expense or income</returns>
    private short? GetRecurringId()
    {
      if (Recurring)
      {
        using var rec = new ExpensesContext();
        var recurringIds = from r in rec.Recurrings.ToList()
                           where r.Name == RecurringId
                           select r.Id;
        if (recurringIds.IsNullOrEmpty()) return null;
        else return recurringIds.First();
      }
      else return null;
    }
  }
}
