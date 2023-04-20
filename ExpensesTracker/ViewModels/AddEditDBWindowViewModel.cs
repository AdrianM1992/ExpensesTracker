using ExpensesTracker.Models.DataControllers;
using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.Views.Classes;
using ExpensesTracker.Views.Windows.AddEditDB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExpensesTracker.ViewModels
{
  class AddEditDBWindowViewModel
  {
    private readonly IMainSettings _mainSettings;
    private readonly AddEditDBWindow _myWindow;
    private DatabaseView _databaseView;

    public AddEditDBWindowViewModel(IMainSettings mainSettings, AddEditDBWindow addEditDBWindow, DatabaseView? databaseView = null)
    {
      _myWindow = addEditDBWindow;
      _mainSettings = mainSettings;
      if (databaseView == null) this._databaseView = new DatabaseView();
      else this._databaseView = databaseView;
    }

    /// <summary>
    /// Convert _databaseView to Expense and pass database
    /// </summary>
    /// <param name="editMode">Specifies if record is edited or new</param>
    public void CommitChanges(bool editMode)
    {
      DatabaseModel.AddEditDBRecord(_databaseView.ReturnExpense(), editMode);
    }

    /// <summary>
    /// Fills _myWindow controls content according to selected to edit record
    /// </summary>
    public void SetControlsValues()
    {
      _myWindow.RecordName.Text = _databaseView.Name;
      _myWindow.Price.Text = _databaseView.Price.ToString("C", new CultureInfo(_mainSettings.Currency));
      _myWindow.Quantity.Text = _databaseView.Quantity.ToString("0.0");
      _myWindow.Total.Text = (_databaseView.Price * _databaseView.Quantity).ToString("C", new CultureInfo(_mainSettings.Currency));
      _myWindow.RecurringId.Text = _databaseView.RecurringId;
      _myWindow.Description.Text = _databaseView.Description;
      _myWindow.Date.Text = _databaseView.Date.ToString();
      _myWindow.Recurring.IsChecked = _databaseView.Recurring;
      if (_databaseView.Income) _myWindow.Income.IsChecked = true;
      else _myWindow.Expense.IsChecked = true;
      SetCategoryValue(null);
    }

    #region Setting properties
    /// <summary>
    /// Sets properties related to input of various TextBoxes
    /// </summary>
    public void HandleTextboxInput(TextBox textBox)
    {
      switch (textBox.Name)
      {
        case "RecordName":
          _databaseView.Name = textBox.Text;
          break;
        case "Price":
          SetPriceValue(textBox.Text);
          break;
        case "Quantity":
          SetQuantityValue(textBox.Text);
          break;
        case "Description":
          _databaseView.Description = textBox.Text;
          break;
        default: break;
      }
    }

    /// <summary>
    /// Sets DatabaseView Quantity property or shows warning if property can't be set
    /// </summary>
    /// <param name="quantity">String containing quantity</param>
    private void SetQuantityValue(string quantity)
    {
      decimal? number = ExtractNumberFromString(quantity);
      if (number != null && number != 0)
      {
        _databaseView.Quantity = (decimal)number;
        _myWindow.Quantity.Text = string.Format("{0}", number);

      }
      else
      {
        MessageBox.Show("Quantity field value is 0 or does not contain any number at all!\nDefault value of 1 will be used instead.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        _databaseView.Quantity = 1M;
        _myWindow.Quantity.Text = string.Format("{0}", 1M);
      }

      _myWindow.Total.Text = string.Format("{0:C}", _databaseView.Total);
    }

    /// <summary>
    /// Sets DatabaseView Price property or shows warning if property can't be set
    /// </summary>
    /// <param name="price">String containing price</param>
    private void SetPriceValue(string price)
    {
      decimal? number = ExtractNumberFromString(price);
      if (number != null && number != 0)
      {
        _databaseView.Price = (decimal)number;
        _myWindow.Price.Text = string.Format("{0:C}", number);
      }
      else
      {
        MessageBox.Show("Value field value is 0 or does not contain any number at all!\nDefault value of 1 will be used instead.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        _databaseView.Price = 1M;
        _myWindow.Price.Text = string.Format("{0:C}", 1M);
      }
      _myWindow.Total.Text = string.Format("{0:C}", _databaseView.Total);
    }

    /// <summary>
    /// Takes input string and searches for every number and first decimal point
    /// </summary>
    /// <param name="input">String to extract number from</param>
    /// <returns>Extracted number or null, if input did not contain any number</returns>
    private decimal? ExtractNumberFromString(string input)
    {
      bool decimalPointFlag = false;
      List<char> inputChars = input.ToList();
      List<char> numberChars = new();
      foreach (var c in inputChars)
      {
        if (char.IsNumber(c)) numberChars.Add(c);
        else if ((!decimalPointFlag && (c == '.' || c == ',')))
        {
          numberChars.Add(',');
          decimalPointFlag = true;
        }
      }
      if (numberChars.Count > 0) return decimal.Parse(new string(numberChars.ToArray()));
      else return null;
    }

    /// <summary>
    /// Sets DatabaseView Category or populates Category ComboBox
    /// </summary>
    /// <param name="newCategory">New value of Category property</param>
    public void SetCategoryValue(string? newCategory)
    {
      if (newCategory != null) _databaseView.Category = newCategory;
      else
      {
        //Populate drop down menu with available categories
        using var db = new ExpensesContext();
        var categories = from c in db.Categories.ToList()
                         select c.Name;
        _myWindow.Category.ItemsSource = categories;
        //Find index of databaseView.Subcategory in drop down menu.
        var index = 0;
        foreach (var category in categories)
        {
          if (category == _databaseView.Category) break;
          index++;
        }
        _myWindow.Category.SelectedIndex = index;
      }
      //Always update subcategory ComboBox when category is changed
      SetSubcategoryValue(null);
    }

    /// <summary>
    /// Sets DatabaseView Subcategory or populates Subcategory ComboBox
    /// </summary>
    /// <param name="newSubcategory">New value of Subcategory property</param>
    public void SetSubcategoryValue(string? newSubcategory)
    {
      if (newSubcategory != null) _databaseView.Subcategory = newSubcategory;
      else
      {
        //Search DB for all subcategories of selected category
        using var db = new ExpensesContext();
        var subcategories = from c in db.Categories
                            where c.Name == _databaseView.Category
                            join s in db.Subcategories on c.Id equals s.CategoryId
                            select s.Name;

        List<string> temp = subcategories.ToList();
        if (!subcategories.Contains("None")) temp.Insert(0, "None");
        _myWindow.Subcategory.ItemsSource = temp;
        //Index must be always defined (not null)
        _myWindow.Subcategory.SelectedIndex = 0;

        //Find index of databaseView.Subcategory in drop down menu.
        var index = 0;
        foreach (var subcategory in temp)
        {
          if (subcategory == _databaseView.Subcategory) break;
          index++;
        }
        _myWindow.Subcategory.SelectedIndex = index;
      }
    }

    /// <summary>
    /// Sets DatabaseView Recurring property
    /// </summary>
    /// <param name="recurring">Specifies if record is recurring</param>
    public void SetRecurring(bool recurring)
    {
      _databaseView.Recurring = recurring;
      if (recurring) SetRecurringId(null);
    }

    /// <summary>
    /// Sets DatabaseView RecurringId property or populates RecurringId ComboBox
    /// </summary>
    /// <param name="newRecurringId">New value of RecurringId property</param>
    public void SetRecurringId(string? newRecurringId)
    {
      if (newRecurringId != null) _databaseView.RecurringId = newRecurringId;
      else
      {
        //Populate drop down menu with available recurring names
        using var db = new ExpensesContext();
        var recurrings = from r in db.Recurrings.ToList()
                         select r.Name;

        List<string> temp = recurrings.ToList();
        if (!recurrings.Contains("None")) temp.Insert(0, "None");
        _myWindow.RecurringId.ItemsSource = temp;
        //Index must be always defined (not null)
        _myWindow.RecurringId.SelectedIndex = 0;
        //Find index of databaseView.Subcategory in drop down menu.
        var index = 0;
        foreach (var recurring in recurrings)
        {
          if (recurring == _databaseView.RecurringId) break;
          index++;
        }
        _myWindow.RecurringId.SelectedIndex = index;
      }
    }

    /// <summary>
    /// Sets Income property of current databaseView
    /// </summary>
    /// <param name="income">Specifies if record is income or expense</param>
    public void SetIncome(bool income)
    {
      _databaseView.Income = income;
    }

    /// <summary>
    /// Sets Date property of current databaseView
    /// </summary>
    /// <param name="dateTime">Specifies date when record occurred</param>
    public void SetDate(DateTime? dateTime)
    {
      _databaseView.Date = dateTime;
    }
    #endregion

    #region Adding entries in subtables
    /// <summary>
    /// Adds new category to database
    /// </summary>
    /// <param name="categoryName">Name of category to add</param>
    public void AddNewCategory(string categoryName)
    {
      DatabaseModel.AddCategory(categoryName);
      _databaseView.Category = categoryName;
      SetCategoryValue(null);
    }
    /// <summary>
    /// Adds new subcategory to database
    /// </summary>
    /// <param name="subcategoryName">Name of subcategory to add</param>
    public void AddNewSubcategory(string subcategoryName)
    {
      DatabaseModel.AddSubcategory(subcategoryName, _databaseView.ReturnExpense().CategoryId);
      _databaseView.Subcategory = subcategoryName;
      SetSubcategoryValue(null);
    }
    /// <summary>
    /// Adds new recurrence to database
    /// </summary>
    /// <param name="recurringName">Recurrence to add</param>
    public void AddNewRecurrence(string recurringName)
    {
      DatabaseModel.AddRecurrence(recurringName);
      _databaseView.RecurringId = recurringName;
      SetRecurringId(null);
    }
    #endregion

    #region Deleting entries in subtables
    /// <summary>
    /// Removes category from database
    /// </summary>
    /// <param name="categoryToDelete">Name of category to delete</param>
    public void RemoveCategory(string categoryToDelete)
    {
      var deleted = DatabaseModel.DeleteCategory(categoryToDelete);
      if (deleted)
      {
        _databaseView.Category = "None";
        SetCategoryValue(null);
      }
    }
    /// <summary>
    /// Removes subcategory from database
    /// </summary>
    /// <param name="subcategoryToDelete">Name of subcategory to delete</param>
    public void RemoveSubcategory(string subcategoryToDelete)
    {
      var deleted = DatabaseModel.DeleteSubcategory(subcategoryToDelete);
      if (deleted)
      {
        _databaseView.Subcategory = null;
        SetSubcategoryValue(null);
      }
    }
    /// <summary>
    /// Removes recurrence from database
    /// </summary>
    /// <param name="recurrenceToDelete">Recurrence to delete</param>
    public void RemoveRecurrence(string recurrenceToDelete)
    {
      var deleted = DatabaseModel.DeleteRecurring(recurrenceToDelete);
      if (deleted)
      {
        _databaseView.RecurringId = null;
        SetRecurringId(null);
      }
    }
    #endregion
  }
}
