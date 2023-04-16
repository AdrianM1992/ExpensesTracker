using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.Views.Classes;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace ExpensesTracker.ViewModels.Converters
{
  public static class DatabaseViewConverters
  {
    /// <summary>
    /// Gets category name of expense or income
    /// </summary>
    /// <param name="expense">Record with reference to categoryId</param>
    /// <returns>Name of category expense or income</returns>
    static string GetCategoryName(Expense expense)
    {
      using (var cat = new ExpensesContext())
      {
        var categoryNames = from c in cat.Categories.ToList()
                            where c.Id == expense.CategoryId
                            select c.Name;
        return categoryNames.First();
      }
    }

    /// <summary>
    /// Gets subcategory name of expense or income
    /// </summary>
    /// <param name="expense">Record with reference to categoryId</param>
    /// <returns>Name of category expense or income</returns>
    static string? GetSubategoryName(Expense expense)
    {
      using (var subcat = new ExpensesContext())
      {
        var subcategoryNames = from s in subcat.Subcategories.ToList()
                               where s.Id == expense.SubcategoryId
                               select s.Name;
        if (subcategoryNames.IsNullOrEmpty()) return null;
        else return subcategoryNames.First();
      }
    }

    /// <summary>
    /// Gets recurring name of expense or income
    /// </summary>
    /// <param name="expense">Record with reference to recurringId</param>
    /// <returns>Name of recurring expense or income</returns>
    static string? GetRecurringName(Expense expense)
    {
      if (expense.Recurring)
      {
        using (var rec = new ExpensesContext())
        {
          var recurringNames = from r in rec.Recurrings.ToList()
                               where r.Id == expense.RecurringId
                               select r.Name;
          if (recurringNames.IsNullOrEmpty()) return "";
          else return recurringNames.First();
        }
      }
      else return "";
    }

    /// <summary>
    /// Gets category ID of expense or income
    /// </summary>
    /// <param name="databaseView">Record with reference to categoryId</param>
    /// <returns>Name of category expense or income</returns>
    static short GetCategoryId(DatabaseView databaseView)
    {
      using (var cat = new ExpensesContext())
      {
        var categoryIds = from c in cat.Categories.ToList()
                          where c.Name == databaseView.Category
                          select c.Id;
        return categoryIds.First();
      }
    }

    /// <summary>
    /// Gets subcategory ID of expense or income
    /// </summary>
    /// <param name="databaseView">Record with reference to categoryId</param>
    /// <returns>Name of category expense or income</returns>
    static int? GetSubategoryId(DatabaseView databaseView)
    {
      using (var subcat = new ExpensesContext())
      {
        var subcategoryIds = from s in subcat.Subcategories.ToList()
                             where s.Name == databaseView.Subcategory
                             select s.Id;
        if (subcategoryIds.IsNullOrEmpty()) return null;
        else return subcategoryIds.First();
      }
    }

    /// <summary>
    /// Gets recurring ID of expense or income
    /// </summary>
    /// <param name="databaseView">Record with reference to recurringId</param>
    /// <returns>Name of recurring expense or income</returns>
    static short? GetRecurringId(DatabaseView databaseView)
    {
      if (databaseView.Recurring)
      {
        using (var rec = new ExpensesContext())
        {
          var recurringIds = from r in rec.Recurrings.ToList()
                             where r.Name == databaseView.RecurringId
                             select r.Id;
          if (recurringIds.IsNullOrEmpty()) return null;
          else return recurringIds.First();
        }
      }
      else return null;
    }

    /// <summary>
    /// Converts database records to DatabaseView
    /// </summary>
    /// <param name="expenses">List of records to convert</param>
    /// <returns>DatabaseView list</returns>
    public static List<DatabaseView> DbToViewConverter(List<Expense> expenses)
    {
      var list = new List<DatabaseView>();
      foreach (var expense in expenses)
      {
        var itemToAdd = new DatabaseView
        {
          Name = expense.Name,
          Price = expense.Price,
          Quantity = expense.Quantity,
          Total = expense.Total,
          DateOfEntry = expense.DateOfEntry,
          LastUpdate = expense.LastUpdate,
          Date = expense.Date,
          Subcategory = GetSubategoryName(expense),
          Income = expense.Income,
          Recurring = expense.Recurring,
          Description = expense.Description,
          Category = GetCategoryName(expense),
          RecurringId = GetRecurringName(expense)
        };
        list.Add(itemToAdd);
      }
      return list;
    }

    /// <summary>
    /// Converts DatabaseViews to database records
    /// </summary>
    /// <param name="databaseViews">List of databaseViews to convert</param>
    /// <returns>Expenses list</returns>
    public static List<Expense> DbToViewConverter(List<DatabaseView> databaseViews)
    {
      var list = new List<Expense>();
      foreach (var databaseView in databaseViews)
      {
        var itemToAdd = new Expense
        {
          Name = databaseView.Name,
          Price = databaseView.Price,
          Quantity = databaseView.Quantity,
          Total = databaseView.Total,
          DateOfEntry = databaseView.DateOfEntry,
          LastUpdate = databaseView.LastUpdate,
          Date = databaseView.Date,
          SubcategoryId = GetSubategoryId(databaseView),
          Income = databaseView.Income,
          Recurring = databaseView.Recurring,
          Description = databaseView.Description,
          CategoryId = GetCategoryId(databaseView),
          RecurringId = GetRecurringId(databaseView)
        };
        list.Add(itemToAdd);
      }
      return list;
    }
  }
}
