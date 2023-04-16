using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.Views.Classes;
using ExpensesTracker.Views.Windows.AddEditDB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExpensesTracker.ViewModels
{
  class AddEditDBWindowViewModel
  {
    private readonly IMainSettings _mainSettings;
    private readonly AddEditDBWindow _myWindow;
    public DatabaseView databaseView;

    public AddEditDBWindowViewModel(IMainSettings mainSettings, AddEditDBWindow addEditDBWindow)
    {
      _myWindow = addEditDBWindow;
      _mainSettings = mainSettings;
      databaseView = new DatabaseView();
    }
    public AddEditDBWindowViewModel(IMainSettings mainSettings, DatabaseView databaseView, AddEditDBWindow addEditDBWindow)
    {
      _myWindow = addEditDBWindow;
      _mainSettings = mainSettings;
      this.databaseView = databaseView;
    }

    /// <summary>
    /// Fills _myWindow controls content according to selected to edit record
    /// </summary>
    public void SetControlsValues()
    {
      _myWindow.Name.Text = databaseView.Name;
      _myWindow.Price.Text = databaseView.Price.ToString("C", new CultureInfo(_mainSettings.Currency));
      _myWindow.Quantity.Text = databaseView.Quantity.ToString("0.0");
      _myWindow.Total.Text = (databaseView.Price * databaseView.Quantity).ToString("C", new CultureInfo(_mainSettings.Currency));
      _myWindow.RecurringId.Text = databaseView.RecurringId;
      _myWindow.Description.Text = databaseView.Description;
      _myWindow.Date.Text = databaseView.Date.ToString();
      if (databaseView.Recurring) _myWindow.Recurring.IsChecked = true;
      if (databaseView.Income) _myWindow.Income.IsChecked = true;
      else _myWindow.Expense.IsChecked = true;
      SetCategoryValue(null);
    }

    /// <summary>
    /// Sets DatabaseView Category or populates Category ComboBox
    /// </summary>
    /// <param name="newCategory">New value of Category property</param>
    public void SetCategoryValue(string? newCategory)
    {
      if (newCategory != null) databaseView.Category = newCategory;
      else
      {
        using (var db = new ExpensesContext())
        {
          var categories = from c in db.Categories.ToList()
                           select c.Name;
          _myWindow.Category.ItemsSource = categories;
          var index = 0;
          foreach (var category in categories)
          {
            if (category == databaseView.Category) break;
            index++;
          }
          _myWindow.Category.SelectedIndex = index;
        }
      }
      SetSubcategoryValue(null);
    }

    /// <summary>
    /// Sets DatabaseView Subcategory or populates Subcategory ComboBox
    /// </summary>
    /// <param name="newSubcategory">New value of Subcategory property</param>
    public void SetSubcategoryValue(string? newSubcategory)
    {
      if (newSubcategory != null) databaseView.Subcategory = newSubcategory;
      else
      {
        using (var db = new ExpensesContext())
        {
          var subcategories = from c in db.Categories
                              where c.Name == databaseView.Category
                              join s in db.Subcategories on c.Id equals s.CategoryId
                              select s.Name;

          List<string> temp = subcategories.ToList();
          temp.Insert(0, "None");
          _myWindow.Subcategory.ItemsSource = temp;

          var index = 0;
          foreach (var subcategory in temp)
          {
            if (subcategory == databaseView.Subcategory) break;
            index++;
          }
          _myWindow.Subcategory.SelectedIndex = index;
        }
      }
    }

    public void SetIncome(bool income)
    {
      databaseView.Income = income;
    }

    public void SetDate(DateTime dateTime)
    {
      databaseView.Date = dateTime;
    }
  }
}
