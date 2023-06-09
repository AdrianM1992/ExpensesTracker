﻿using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.ViewModels;
using ExpensesTracker.Views.Classes;
using ExpensesTracker.Views.Windows.NewElementWindowInput;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExpensesTracker.Views.Windows.AddEditDB
{
  /// <summary>
  /// Window for adding new records to database or editing existing ones
  /// </summary>
  public partial class AddEditDBWindow : Window
  {
    private readonly AddEditDBWindowViewModel _viewModel;
    private readonly IMainSettings _mainSettings;
    private readonly bool editMode;
    private readonly Dictionary<TextBox, string> _textBoxes;
    private bool _savedFlag = false;

    public AddEditDBWindow(IMainSettings mainSettings, DatabaseBrowserEnum operationType, DatabaseView? databaseView = null)
    {
      _mainSettings = mainSettings;
      _viewModel = new AddEditDBWindowViewModel(_mainSettings, this, operationType, databaseView);
      InitializeComponent();
      _textBoxes = CreateTextBoxesDictionary();
      _viewModel.SetControlsValues();
      if (operationType == DatabaseBrowserEnum.Edit) editMode = true;
      else editMode = false;
    }

    private void TitleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      var textBox = (TextBox)sender;
      if (_textBoxes[textBox] == textBox.Text) textBox.Text = string.Empty;
      textBox.FontStyle = FontStyles.Normal;
      textBox.Foreground = Brushes.Black;
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      var textBox = (TextBox)sender;
      if (textBox.Text == "")
      {
        textBox.Text = _textBoxes[textBox];
        textBox.FontStyle = FontStyles.Italic;
        textBox.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x5F, 0x5F, 0x5F));
      }
      else _viewModel.HandleTextBoxInput(textBox);
    }

    /// <summary>
    /// Searches controls list and picks TextBoxes to create dictionary keys; values are default text
    /// </summary>
    private Dictionary<TextBox, string> CreateTextBoxesDictionary()
    {
      var textBoxes = new Dictionary<TextBox, string>();
      foreach (var textBox in GetAllControls(this).OfType<TextBox>())
      {
        textBoxes.Add(textBox, textBox.Text);
      }
      return textBoxes;
    }

    /// <summary>
    /// Creates list of all controls in parent hierarchy tree
    /// </summary>
    /// <param name="parent">Reference object to list children"</param>
    /// <returns>List of all children controls</returns>
    private List<Control> GetAllControls(DependencyObject parent)
    {
      List<Control> controls = new();

      foreach (var child in LogicalTreeHelper.GetChildren(parent))
      {
        if (child is Control control) controls.Add(control);
        if (child is DependencyObject childDependencyObject) controls.AddRange(GetAllControls(childDependencyObject));
      }
      return controls;
    }

    private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      var date = (DatePicker)sender;
      if (date.SelectedDate.HasValue) _viewModel.SetDate(date.SelectedDate.Value);
      else _viewModel.SetDate(null);
    }

    /// <summary>
    /// DropDownClosed is chosen over SelectionChanged, because SelectionChanged fires when viewModel sets SelectedIndex
    /// </summary>
    #region ComboBox events
    private void Subcategory_DropDownClosed(object sender, System.EventArgs e)
    {
      var subcategory = (ComboBox)sender;
      _viewModel.SetSubcategoryValue(subcategory.SelectedValue.ToString());
    }

    private void Category_DropDownClosed(object sender, System.EventArgs e)
    {
      var category = (ComboBox)sender;
      _viewModel.SetCategoryValue(category.SelectedValue.ToString());
    }

    private void RecurringId_DropDownClosed(object sender, System.EventArgs e)
    {
      var recurringId = (ComboBox)sender;
      _viewModel.SetRecurringId(recurringId.SelectedValue.ToString());
    }
    #endregion

    private void Recurring_Checked(object sender, RoutedEventArgs e)
    {
      var checkBox = (CheckBox)sender;
      RecurringList.IsEnabled = checkBox.IsChecked == true;
      RecurringId.ItemsSource = null;
      if (checkBox.IsChecked.HasValue) _viewModel.SetRecurring(checkBox.IsChecked.Value);
    }
    private void Income_Checked(object sender, RoutedEventArgs e)
    {
      var income = sender as RadioButton;
      if (income.Name == "Income") _viewModel.SetIncome(true);
      else _viewModel.SetIncome(false);
    }

    #region Click events
    private void AddRecord_Click(object sender, RoutedEventArgs e)
    {
      _savedFlag = true;
      _viewModel.CommitChanges(editMode);
      Close();
    }
    private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
    {
      var dialog = new NewElementWindow { Header = "Name new category:" };
      bool? result = dialog.ShowDialog();
      if (result == true) _viewModel.AddNewCategory(dialog.NewElementName);

      dialog.Close();
    }
    private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
    {
      var result = MessageBox.Show("Are you sure?\nThis cannot be undone.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (result == MessageBoxResult.Yes)
      {
        var cat = Category.SelectedItem?.ToString();
        if (cat != null) _viewModel.RemoveCategory(cat);
      }
    }
    private void AddSubCategoryButton_Click(object sender, RoutedEventArgs e)
    {
      var dialog = new NewElementWindow { Header = "Name new subcategory:" };
      bool? result = dialog.ShowDialog();
      if (result == true) _viewModel.AddNewSubcategory(dialog.NewElementName);
      dialog.Close();
    }
    private void DeleteSubCategoryButton_Click(object sender, RoutedEventArgs e)
    {
      var result = MessageBox.Show("Are you sure?\nThis cannot be undone.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (result == MessageBoxResult.Yes && Subcategory.SelectedItem.ToString() != "None")
      {
        var subCat = Subcategory.SelectedItem?.ToString();
        if (subCat != null) _viewModel.RemoveSubcategory(subCat);
      }
    }
    private void AddRecurringButton_Click(object sender, RoutedEventArgs e)
    {
      var dialog = new NewElementWindow { Header = "Name new recurrence:" };
      bool? result = dialog.ShowDialog();
      if (result == true) _viewModel.AddNewRecurrence(dialog.NewElementName);
      dialog.Close();
    }
    private void DeleteRecurringButton_Click(object sender, RoutedEventArgs e)
    {
      var result = MessageBox.Show("Are you sure?\nThis cannot be undone.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (result == MessageBoxResult.Yes && RecurringId.SelectedItem.ToString() != "None")
      {
        var rec = RecurringId.SelectedItem.ToString();
        if (rec != null) _viewModel.RemoveRecurrence(rec);
      }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => Close();
    #endregion

    private void AddEditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_savedFlag)
      {
        MessageBoxResult result = MessageBox.Show("Changes are not saved. \nExit anyway?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.No) e.Cancel = true;
      }
    }
  }
}