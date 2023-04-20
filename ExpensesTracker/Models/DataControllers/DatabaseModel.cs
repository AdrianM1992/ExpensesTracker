using ExpensesTracker.Models.DataProviders;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace ExpensesTracker.Models.DataControllers
{
  public static class DatabaseModel
  {
    public static void AddEditDBRecord(Expense expense, bool editMode)
    {
      using var db = new ExpensesContext();
      if (editMode) db.Entry(expense).State = EntityState.Modified;
      else db.Add(expense);
      db.SaveChanges();
    }
    public static void DeleteDBRecord(Expense expense)
    {
      using var db = new ExpensesContext();
      if (expense != null)
      {
        var records = from e in db.Expenses.ToList()
                      where e.Id == expense.Id
                      select e;

        Expense record = records.First();
        db.Remove(record);
      }
      db.SaveChanges();
    }

    public static void AddCategory(string categoryName)
    {
      using var db = new ExpensesContext();
      var categories = from e in db.Categories.ToList()
                       where e.Name == categoryName
                       select e;

      if (!categories.Any())
      {
        var newCategory = new Category { Name = categoryName, Fixed = false };
        db.Categories.Add(newCategory);
        db.SaveChanges();
      }
      else MessageBox.Show("Category with this name already exist.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
    public static void AddSubcategory(string subcategoryName, int categoryId)
    {
      using var db = new ExpensesContext();
      var subcategories = from s in db.Subcategories.ToList()
                          where s.CategoryId == categoryId
                          where s.Name == subcategoryName
                          select s;

      if (!subcategories.Any())
      {
        var newSubategory = new Subcategory { Name = subcategoryName, CategoryId = categoryId };
        db.Subcategories.Add(newSubategory);
        db.SaveChanges();
      }
      else MessageBox.Show("Subcategory with this name already exist.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
    public static void AddRecurrence(string recurringName)
    {
      using var db = new ExpensesContext();
      var recurrences = from r in db.Recurrings.ToList()
                        where r.Name == recurringName
                        select r;
      if (!recurrences.Any())
      {
        var newRecurrence = new Recurring { Name = recurringName };
        db.Recurrings.Add(newRecurrence);
        db.SaveChanges();
      }
      else MessageBox.Show("Recurrence with this name already exist.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    /// <summary>
    /// Deletes category form database
    /// </summary>
    /// <param name="categoryToDelete">Name of category to delete</param>
    /// <returns>True, if category was deleted</returns>
    public static bool DeleteCategory(string categoryToDelete)
    {
      using var db = new ExpensesContext();
      var categories = from c in db.Categories.ToList()
                       where c.Name == categoryToDelete && c.Fixed != true
                       select c;

      if (categories.Any())
      {
        //Before deleting category find all items with this category and change it to default
        var itemsInCategory = from i in db.Expenses
                              where i.CategoryId == categories.First().Id
                              select i;

        foreach (var item in itemsInCategory) item.CategoryId = 1;
        db.Entry(categories.First()).State = EntityState.Deleted;
        db.SaveChanges();
        return true;
      }
      else MessageBox.Show("No category to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }
    /// <summary>
    /// Deletes subcategory from database
    /// </summary>
    /// <param name="subcategoryToDelete">Name of subcategory to delete</param>
    /// <returns>True, if subcategory was deleted</returns>
    public static bool DeleteSubcategory(string subcategoryToDelete)
    {
      using var db = new ExpensesContext();
      var subcategories = from s in db.Subcategories.ToList()
                          where s.Name == subcategoryToDelete
                          select s;

      if (subcategories.Any())
      {
        //Before deleting subcategory find all items with this category and change it to default
        var itemsInSubcategory = from i in db.Expenses
                                 where i.SubcategoryId == subcategories.First().Id
                                 select i;

        foreach (var item in itemsInSubcategory) item.SubcategoryId = null;
        db.Entry(subcategories.First()).State = EntityState.Deleted;
        db.SaveChanges();
        return true;
      }
      else MessageBox.Show("No subcategory to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }
    /// <summary>
    /// Deletes recurrence from database
    /// </summary>
    /// <param name="recurrenceToDelete">Name of recurrence to delete</param>
    /// <returns>True, if recurrence was deleted</returns>
    public static bool DeleteRecurring(string recurrenceToDelete)
    {
      using var db = new ExpensesContext();
      var recurrences = from r in db.Recurrings.ToList()
                        where r.Name == recurrenceToDelete
                        select r;

      if (recurrences.Any())
      {
        //Before deleting subcategory find all items with this category and change it to default
        var itemsWithRecurrence = from i in db.Expenses
                                  where i.RecurringId == recurrences.First().Id
                                  select i;

        foreach (var item in itemsWithRecurrence) item.RecurringId = null;
        db.Entry(recurrences.First()).State = EntityState.Deleted;
        db.SaveChanges();
        return true;
      }
      else MessageBox.Show("No recurrence to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }
  }
}
