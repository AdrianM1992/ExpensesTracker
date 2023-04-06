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
    public ObservableCollection<DatabaseView> Expenses { get; set; }

    public DatabaseBrowserPageViewModel(Page page)
    {
      _databaseBrowserPage = page as DatabaseBrowserPage;
      using (var db = new ExpensesContext())
      {
        var count = db.Expenses.Count();
        if (count >= 100)
        {
          Expenses = new ObservableCollection<DatabaseView>(DbToViewConvert(db.Expenses.TakeLast(100).ToList()));
        }
        else
        {
          Expenses = new ObservableCollection<DatabaseView>(DbToViewConvert(db.Expenses.Take(count).ToList()));
        }
      }
    }

    /// <summary>
    /// Gets category name of expense or income
    /// </summary>
    /// <param name="expense">Record with reference to categoryId</param>
    /// <returns>Name of category expense or income</returns>
    private string GetCategoryName(Expense expense)
    {
      string category;
      using (var cat = new ExpensesContext())
      {
        var categoryNames = from c in cat.Categories.ToList()
                            where c.Id == expense.CategoryId
                            select c.Name;
        category = categoryNames.First();
      }
      return category;
    }

    /// <summary>
    /// Gets recurring expense or income name
    /// </summary>
    /// <param name="expense">Record with reference to recurringId</param>
    /// <returns>Name of recurring expense or income</returns>
    private string GetRecurringName(Expense expense)
    {
      if (expense.Recurring)
      {
        using (var rec = new ExpensesContext())
        {
          var categoryNames = from r in rec.Recurrings.ToList()
                              where r.Id == expense.RecurringId
                              select r.Name;
          return categoryNames.First();
        }
      }
      else return null;
    }

    /// <summary>
    /// Converts database records to DatabaseView
    /// </summary>
    /// <param name="expenses">List of records to convert</param>
    /// <returns>DatabaseView list</returns>
    private List<DatabaseView> DbToViewConvert(List<Expense> expenses)
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
          Subcategory = expense.Subcategory,
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

  }
}
