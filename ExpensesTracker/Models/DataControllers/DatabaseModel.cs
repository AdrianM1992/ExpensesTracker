using ExpensesTracker.DataTypes;
using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.Views.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ExpensesTracker.Models.DataControllers
{
  /// <summary>
  /// Class governing interaction with database
  /// </summary>
  public static class DatabaseModel
  {
    /// <summary>
    /// Event notifying whenever database is changed
    /// </summary>
    public static event EventHandler<EventArgs>? SubtablesChanged;
    public static event EventHandler<EventArgs>? DataChanged;

    /// <summary>
    /// Methods that affects directly database
    /// </summary>
    #region Editing

    public static void AddEditDBRecord(Expense expense, bool editMode)
    {
      using var db = new ExpensesContext();
      if (editMode) db.Entry(expense).State = EntityState.Modified;
      else db.Add(expense);
      db.SaveChanges();
      DataChanged?.Invoke(null, new EventArgs());
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
      DataChanged?.Invoke(null, new EventArgs());
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
        SubtablesChanged?.Invoke(null, new EventArgs());
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
        var newSubCategory = new Subcategory { Name = subcategoryName, CategoryId = categoryId };
        db.Subcategories.Add(newSubCategory);
        db.SaveChanges();
        SubtablesChanged?.Invoke(null, new EventArgs());
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
        SubtablesChanged?.Invoke(null, new EventArgs());
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
        SubtablesChanged?.Invoke(null, new EventArgs());
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
        SubtablesChanged?.Invoke(null, new EventArgs());
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
        SubtablesChanged?.Invoke(null, new EventArgs());
        return true;
      }
      else MessageBox.Show("No recurrence to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
      return false;
    }
    #endregion

    /// <summary>
    /// Methods that returns names or IDs values stored in subtables
    /// </summary>
    #region Names and IDs

    /// <summary>
    /// Opens Categories table and returns its content
    /// </summary>
    /// <returns>List of category names</returns>
    public static List<string> GetCategoriesNames()
    {
      using var db = new ExpensesContext();

      var categories = (from c in db.Categories
                        select c.Name).ToList();
      return categories;
    }
    /// <summary>
    /// Gets name of category of passed Expanse instance
    /// </summary>
    /// <param name="expense">Expanse instance</param>
    /// <returns>List of category names</returns>
    public static List<string> GetCategoriesNames(Expense expense)
    {
      using var db = new ExpensesContext();

      var categories = (from c in db.Categories
                        where c.Id == expense.CategoryId
                        select c.Name).ToList();
      return categories;
    }
    /// <summary>
    /// Gets names of categories of passed category ID list
    /// </summary>
    /// <param name="IDs">List of categories IDs</param>
    /// <returns>List of category names</returns>
    public static List<string> GetCategoriesNames(List<short> IDs)
    {
      using var db = new ExpensesContext();

      var categories = (from c in db.Categories
                        where IDs.Contains(c.Id)
                        select c.Name).ToList();
      return categories;
    }
    /// <summary>
    /// Opens Categories table and searches matches in names on category in provided DatabaseViews list
    /// </summary>
    /// <param name="databaseViews">List of DatabaseView to return IDs from</param>
    /// <returns>List of all matched categories IDs</returns>
    public static List<short> GetCategoriesIds(List<DatabaseView> databaseViews)
    {
      using var db = new ExpensesContext();
      List<short> categoriesIds = new();
      foreach (var view in databaseViews)
      {
        categoriesIds.AddRange((from c in db.Categories
                                where c.Name == view.Category
                                select c.Id).ToList());
      }
      return categoriesIds;
    }

    /// <summary>
    /// Opens Subcategories table and returns its content
    /// </summary>
    /// <returns>List of subcategories</returns>
    public static List<string> GetSubcategoriesNames()
    {
      using var db = new ExpensesContext();

      var subcategories = (from s in db.Subcategories
                           select s.Name).ToList();
      return subcategories;
    }
    /// <summary>
    /// Gets name of subcategory of passed Expanse instance
    /// </summary>
    /// <param name="expense">Optional Expanse instance</param>
    /// <returns>List of subcategories</returns>
    public static List<string> GetSubcategoriesNames(Expense expense)
    {
      using var db = new ExpensesContext();

      var subcategories = (from s in db.Subcategories
                           where s.Id == expense.SubcategoryId
                           select s.Name).ToList();
      return subcategories;
    }
    /// <summary>
    /// Gets names of subcategories of passed category name
    /// </summary>
    /// <param name="category">Category to search for subcategories </param>
    /// <returns>List of subcategories</returns>
    public static List<string> GetSubcategoriesNames(string category = "None")
    {
      using var db = new ExpensesContext();

      var subcategories = (from c in db.Categories
                           where c.Name == category
                           join s in db.Subcategories on c.Id equals s.CategoryId
                           select s.Name).ToList();
      return subcategories;
    }
    /// <summary>
    /// Gets names of subcategories of passed category ID list
    /// </summary>
    /// <param name="IDs">List of subcategories IDs</param>
    /// <returns>List of subcategories</returns>
    public static List<string> GetSubcategoriesNames(List<int?> IDs)
    {
      using var db = new ExpensesContext();

      var subcategories = (from s in db.Subcategories
                           where IDs.Contains(s.Id)
                           select s.Name).ToList();
      return subcategories;
    }
    /// <summary>
    /// Opens Subcategories table and searches matches in names on subcategory in provided DatabaseViews list
    /// </summary>
    /// <param name="databaseViews">List of DatabaseView to return IDs from</param>
    /// <returns>List of all matched subcategories IDs</returns>
    public static List<int> GetSubcategoriesIds(List<DatabaseView> databaseViews)
    {
      using var db = new ExpensesContext();
      List<int> subcategoriesIds = new();
      foreach (var view in databaseViews)
      {
        subcategoriesIds.AddRange((from c in db.Subcategories
                                   where c.Name == view.Subcategory
                                   select c.Id).ToList());
      }
      return subcategoriesIds;
    }

    /// <summary>
    /// Opens Recurrings table and returns its content
    /// </summary>
    /// <returns>List of recurrences names</returns>
    public static List<string> GetRecurringNames()
    {
      using var db = new ExpensesContext();

      var recurrences = (from r in db.Recurrings
                         select r.Name).ToList();
      return recurrences;

    }
    /// <summary>
    /// Gets name of recurrence of passed Expanse instance
    /// </summary>
    /// <param name="expense">Expanse instance</param>
    /// <returns>List of recurrences names</returns>
    public static List<string> GetRecurringNames(Expense expense)
    {
      using var db = new ExpensesContext();

      var recurrences = (from r in db.Recurrings
                         where r.Id == expense.RecurringId
                         select r.Name).ToList();
      return recurrences;
    }
    /// <summary>
    /// Gets names of recurrences of passed recurrences ID list
    /// </summary>
    /// <param name="IDs">List of recurrences IDs</param>
    /// <returns>List of recurrences names</returns>
    public static List<string> GetRecurringNames(List<short?> IDs)
    {
      using var db = new ExpensesContext();

      var recurrences = (from r in db.Recurrings
                         where IDs.Contains(r.Id)
                         select r.Name).ToList();
      return recurrences;
    }
    /// <summary>
    /// Opens Recurrings table and searches matches in names on recurringId in provided DatabaseViews list
    /// </summary>
    /// <param name="databaseViews">List of DatabaseView to return IDs from</param>
    /// <returns>List of all matched recurring IDs</returns>
    public static List<short> GetRecurringIds(List<DatabaseView> databaseViews)
    {
      using var db = new ExpensesContext();
      List<short> recurringIds = new();
      foreach (var view in databaseViews)
      {
        recurringIds.AddRange((from c in db.Recurrings
                               where c.Name == view.RecurringId
                               select c.Id).ToList());
      }
      return recurringIds;
    }
    #endregion

    /// <summary>
    /// All available filters that can be applied to filter database
    /// Output of every filter can be input to another filter
    /// </summary>
    #region Filters

    public static List<Expense> FilterByRange(List<Expense>? expenses, bool first, int count = 10)
    {
      List<Expense> listToReturn;
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        if (count == -1 || db.Expenses.Count() < count) count = db.Expenses.Count();
        listToReturn = first ? db.Expenses.Take(count).ToList() : db.Expenses.TakeLast(count).Reverse().ToList();
        return listToReturn;
      }
      else
      {
        if (count == -1 || expenses.Count < count) count = expenses.Count;
        listToReturn = first ? expenses.Take(count).ToList() : expenses.TakeLast(count).Reverse().ToList();
        return listToReturn;
      }
    }
    public static List<Expense> FilterByName(List<Expense>? expenses, string name)
    {
      List<Expense> listToReturn;
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        listToReturn = (from expense in db.Expenses
                        where expense.Name.Contains(name)
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.Name.Contains(name)
                        select expense).ToList();
        return listToReturn;
      }
    }
    public static List<Expense> FilterByIncome(List<Expense>? expenses, bool income)
    {
      List<Expense> listToReturn;
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        listToReturn = (from expense in db.Expenses
                        where expense.Income == income
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.Income == income
                        select expense).ToList();
        return listToReturn;
      }
    }
    public static List<Expense> FilterByRecurrence(List<Expense>? expenses, bool recurring)
    {
      List<Expense> listToReturn;
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        listToReturn = (from expense in db.Expenses
                        where expense.Recurring == recurring
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.Recurring == recurring
                        select expense).ToList();
        return listToReturn;
      }
    }
    public static List<Expense> FilterByPrice(List<Expense>? expenses, DecimalRange priceRange)
    {
      List<Expense> listToReturn;
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        listToReturn = (from expense in db.Expenses
                        where expense.Price > priceRange.NumberMin && expense.Price < priceRange.NumberMax
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.Price > priceRange.NumberMin && expense.Price < priceRange.NumberMax
                        select expense).ToList();
        return listToReturn;
      }
    }
    public static List<Expense> FilterByQuantity(List<Expense>? expenses, DecimalRange quantityRange)
    {
      List<Expense> listToReturn;
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        listToReturn = (from expense in db.Expenses
                        where expense.Quantity > quantityRange.NumberMin && expense.Quantity < quantityRange.NumberMax
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.Quantity > quantityRange.NumberMin && expense.Quantity < quantityRange.NumberMax
                        select expense).ToList();
        return listToReturn;
      }
    }
    public static List<Expense> FilterByTotal(List<Expense>? expenses, DecimalRange totalRange)
    {
      List<Expense> listToReturn;
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        listToReturn = (from expense in db.Expenses
                        where expense.Total > totalRange.NumberMin && expense.Price < totalRange.NumberMax
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.Total > totalRange.NumberMin && expense.Price < totalRange.NumberMax
                        select expense).ToList();
        return listToReturn;
      }
    }
    public static List<Expense> FilterBySubmitDate(List<Expense>? expenses, DateRange submitDateRange)
    {
      List<Expense> listToReturn;
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        listToReturn = (from expense in db.Expenses
                        where expense.DateOfEntry > submitDateRange.StartDate && expense.DateOfEntry < submitDateRange.EndDate
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.DateOfEntry > submitDateRange.StartDate && expense.DateOfEntry < submitDateRange.EndDate
                        select expense).ToList();
        return listToReturn;
      }
    }
    public static List<Expense> FilterByUpdateDate(List<Expense>? expenses, DateRange updateDateRange)
    {
      List<Expense> listToReturn;
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        listToReturn = (from expense in db.Expenses
                        where expense.LastUpdate > updateDateRange.StartDate && expense.LastUpdate < updateDateRange.EndDate
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.LastUpdate > updateDateRange.StartDate && expense.LastUpdate < updateDateRange.EndDate
                        select expense).ToList();
        return listToReturn;
      }
    }
    public static List<Expense> FilterByUserDate(List<Expense>? expenses, DateRange userDateRange)
    {
      List<Expense> listToReturn;
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        listToReturn = (from expense in db.Expenses
                        where expense.Date > userDateRange.StartDate && expense.Date < userDateRange.EndDate
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.Date > userDateRange.StartDate && expense.Date < userDateRange.EndDate
                        select expense).ToList();
        return listToReturn;
      }
    }
    public static List<Expense> FilterByCategories(List<Expense>? expenses, List<string> categories)
    {
      List<Expense> listToReturn = new();
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        foreach (var category in categories)
        {
          var categoryRecords = (from expense in db.Expenses
                                 join cat in db.Categories
                                 on expense.CategoryId equals cat.Id
                                 where cat.Name == category
                                 select expense).ToList();
          listToReturn.AddRange(categoryRecords);
        }
        return listToReturn;
      }
      else
      {
        foreach (var category in categories)
        {
          var categoryRecords = (from expense in expenses
                                 join cat in db.Categories
                                 on expense.CategoryId equals cat.Id
                                 where cat.Name == category
                                 select expense).ToList();
          listToReturn.AddRange(categoryRecords);
        }
        return listToReturn;
      }
    }
    public static List<Expense> FilterBySubcategories(List<Expense>? expenses, List<string> subcategories)
    {
      List<Expense> listToReturn = new();
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        foreach (var subcategory in subcategories)
        {
          var subcategoryRecords = (from expense in db.Expenses
                                    join subcat in db.Subcategories
                                    on expense.SubcategoryId equals subcat.Id
                                    where subcat.Name == subcategory
                                    select expense).ToList();
          listToReturn.AddRange(subcategoryRecords);
        }
        return listToReturn;
      }
      else
      {
        foreach (var subcategory in subcategories)
        {
          var subcategoryRecords = (from expense in expenses
                                    join subcat in db.Subcategories
                                    on expense.SubcategoryId equals subcat.Id
                                    where subcat.Name == subcategory
                                    select expense).ToList();
          listToReturn.AddRange(subcategoryRecords);
        }
        return listToReturn;
      }
    }
    public static List<Expense> FilterByRecurrenceNames(List<Expense>? expenses, List<string> recurringNames)
    {
      List<Expense> listToReturn = new();
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        foreach (var recurring in recurringNames)
        {
          var recurringRecords = (from expense in db.Expenses
                                  join rec in db.Recurrings
                                  on expense.RecurringId equals rec.Id
                                  where rec.Name == recurring
                                  select expense).ToList();
          listToReturn.AddRange(recurringRecords);
        }
        return listToReturn;
      }
      else
      {
        foreach (var recurring in recurringNames)
        {
          var recurringRecords = (from expense in expenses
                                  join rec in db.Recurrings
                                  on expense.RecurringId equals rec.Id
                                  where rec.Name == recurring
                                  select expense).ToList();
          listToReturn.AddRange(recurringRecords);
        }
        return listToReturn;
      }
    }
    #endregion
  }
}