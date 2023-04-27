﻿using ExpensesTracker.DataTypes;
using ExpensesTracker.Models.DataProviders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

    public static List<Expense> FilterByRange(List<Expense>? expenses, bool first, int count = 0)
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
        listToReturn = (from expense in db.Expenses.ToList()
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
        listToReturn = (from expense in db.Expenses.ToList()
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
    public static List<Expense> FilterByRecurrance(List<Expense>? expenses, bool recurring)
    {
      List<Expense> listToReturn;
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        listToReturn = (from expense in db.Expenses.ToList()
                        where expense.Recurring == recurring
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.Income == recurring
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
        listToReturn = (from expense in db.Expenses.ToList()
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
        listToReturn = (from expense in db.Expenses.ToList()
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
        listToReturn = (from expense in db.Expenses.ToList()
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
        listToReturn = (from expense in db.Expenses.ToList()
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
        listToReturn = (from expense in db.Expenses.ToList()
                        where expense.LastUpdate > updateDateRange.StartDate && expense.DateOfEntry < updateDateRange.EndDate
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.LastUpdate > updateDateRange.StartDate && expense.DateOfEntry < updateDateRange.EndDate
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
        listToReturn = (from expense in db.Expenses.ToList()
                        where expense.Date > userDateRange.StartDate && expense.DateOfEntry < userDateRange.EndDate
                        select expense).ToList();
        return listToReturn;
      }
      else
      {
        listToReturn = (from expense in expenses
                        where expense.Date > userDateRange.StartDate && expense.DateOfEntry < userDateRange.EndDate
                        select expense).ToList();
        return listToReturn;
      }
    }
    public static List<Expense> FilterByCategories(List<Expense>? expenses, List<int> categories)
    {
      List<Expense> listToReturn = new();
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        foreach (var category in categories)
        {
          var categoryRecords = (from expense in db.Expenses.ToList()
                                 where expense.CategoryId == category
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
                                 where expense.CategoryId == category
                                 select expense).ToList();
          listToReturn.AddRange(categoryRecords);
        }
        return listToReturn;
      }
    }
    public static List<Expense> FilterBySubcategories(List<Expense>? expenses, List<int> subcategories)
    {
      List<Expense> listToReturn = new();
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        foreach (var subcategory in subcategories)
        {
          var subcategoryRecords = (from expense in db.Expenses.ToList()
                                    where expense.SubcategoryId == subcategory
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
                                    where expense.SubcategoryId == subcategory
                                    select expense).ToList();
          listToReturn.AddRange(subcategoryRecords);
        }
        return listToReturn;
      }
    }
    public static List<Expense> FilterByRecurranceNames(List<Expense>? expenses, List<int> recurringNames)
    {
      List<Expense> listToReturn = new();
      using var db = new ExpensesContext();
      if (expenses == null)
      {
        foreach (var recurring in recurringNames)
        {
          var recurringRecords = (from expense in db.Expenses.ToList()
                                  where expense.RecurringId == recurring
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
                                  where expense.RecurringId == recurring
                                  select expense).ToList();
          listToReturn.AddRange(recurringRecords);
        }
        return listToReturn;
      }
    }
  }
}