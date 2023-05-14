using ExpensesTracker.Models.DataControllers;
using ExpensesTracker.Models.DataProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
    private readonly Expense? _expense = null;

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
    public DatabaseView(Expense expense, bool edit)
    {
      if (edit)
      {
        _expense = expense;
        ID = expense.Id;
        DateOfEntry = expense.DateOfEntry;
      }
      else DateOfEntry = DateTime.Now;
      Name = expense.Name;
      Price = expense.Price;
      Quantity = expense.Quantity;
      Total = expense.Total;
      LastUpdate = expense.LastUpdate;
      Date = expense.Date;
      Subcategory = GetSubCategoryName(expense);
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
        _expense.SubcategoryId = GetSubCategoryId();
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
          SubcategoryId = GetSubCategoryId(),
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
    private static string GetCategoryName(Expense expense)
    {
      var categoryNames = DatabaseModel.GetCategoriesNames(expense);
      if (categoryNames.IsNullOrEmpty()) return "None";
      else return categoryNames.First();
    }

    /// <summary>
    /// Gets subcategory name of expense or income
    /// </summary>
    /// <param name="expense">Record with reference to categoryId</param>
    /// <returns>Name of category expense or income</returns>
    private static string? GetSubCategoryName(Expense expense)
    {
      var subcategoryNames = DatabaseModel.GetSubcategoriesNames(expense: expense);
      if (subcategoryNames.IsNullOrEmpty()) return null;
      else return subcategoryNames.First();
    }

    /// <summary>
    /// Gets recurring name of expense or income
    /// </summary>
    /// <param name="expense">Record with reference to recurringId</param>
    /// <returns>Name of recurring expense or income</returns>
    private static string? GetRecurringName(Expense expense)
    {
      if (expense.Recurring)
      {
        var recurringNames = DatabaseModel.GetRecurringNames(expense: expense);
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
      var categoryIds = DatabaseModel.GetCategoriesIds(new List<DatabaseView> { this });
      return categoryIds.First();
    }

    /// <summary>
    /// Gets subcategory ID of expense or income
    /// </summary>
    /// <returns>Name of category expense or income</returns>
    private int? GetSubCategoryId()
    {
      var subcategoryIds = DatabaseModel.GetSubcategoriesIds(new List<DatabaseView> { this });
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
        var recurringIds = DatabaseModel.GetRecurringIds(new List<DatabaseView> { this });
        if (recurringIds.IsNullOrEmpty()) return null;
        else return recurringIds.First();
      }
      else return null;
    }
  }
}