using ExpensesTracker.DataTypes;
using ExpensesTracker.Models.DataControllers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Database filtering control
  /// </summary>
  public partial class FilterCluster : UserControl
  {
    private FilterSortController _filterController = new();
    private readonly FilterCluster _thisFilterCluster;

    public FilterCluster()
    {
      InitializeComponent();
      SetDeafultValues();
      _thisFilterCluster = this;
      DatabaseModel.SubtablesChanged += DatabaseModel_SubtablesChanged;
    }

    private void DatabaseModel_SubtablesChanged(object? sender, EventArgs e) => ClearAll_MouseLeftButtonDown(ClearAll, null);
    public void SetFilterControllerRef(FilterSortController filterSortController) => _filterController = filterSortController;
    private void ClearAll_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SetDeafultValues();
      _filterController.ClearAllFilters();
      ClearAll.Visibility = Visibility.Hidden;

      //Exclude ClearButton inside ClearAll StackPanel, because it is hidden as whole
      ClearButton? sendersClearButton = null; ;
      foreach (var control in GetChildElements(this)) if (control is ClearButton clearButton) sendersClearButton = control as ClearButton;
      foreach (var control in GetChildElements(_thisFilterCluster))
      {
        if (control is ClearButton clearButton && control != sendersClearButton) clearButton.Visibility = Visibility.Hidden;
      }
    }
    private void SetDeafultValues()
    {
      SearchBox.Text = "Search database...";
      foreach (var control in GetChildElements(BasicGroup)) if (control is RadioButton radioButton) radioButton.IsChecked = false;
      foreach (var control in GetChildElements(PriceGroup)) if (control is NumericUpDown numericUpDown) numericUpDown.Clear();
      foreach (var control in GetChildElements(DateGroup)) if (control is DatePicker datePicker) datePicker.SelectedDate = null;
      foreach (var control in GetChildElements(ListGroup)) if (control is ListBox listBox) listBox.UnselectAll();
      foreach (var control in GetChildElements(ListGroup)) if (control is Expander expander) expander.IsExpanded = false;
      CategoriesList.ItemsSource = DatabaseModel.GetCategoriesNames();
      RecurrencesList.ItemsSource = DatabaseModel.GetRecurringNames();
      Subcategories.IsEnabled = false;
      Recurrences.IsEnabled = false;
    }

    private void SearchBar_GotFocus(object sender, RoutedEventArgs e)
    {
      if (sender is TextBox textBox)
      {
        if (textBox.Text == "Search database...")
        {
          textBox.Foreground = Brushes.Black;
          textBox.FontStyle = FontStyles.Normal;
          textBox.Text = "";
        }
      }
    }
    private void SearchBar_LostFocus(object sender, RoutedEventArgs e)
    {
      if (sender is TextBox textBox)
      {
        if (textBox.Text == "")
        {
          textBox.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x5F, 0x5F, 0x5F));
          textBox.FontStyle = FontStyles.Italic;
          textBox.Text = "Search database...";
        }
      }
    }

    /// <summary>
    /// Methods governing filters events
    /// </summary>
    #region Filters
    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (sender is TextBox searchPhrase)
      {
        if (searchPhrase.Text != "Search database...")
        {
          _filterController.Name = searchPhrase.Text;
          ClearBasic.Visibility = Visibility.Visible;
          ClearAll.Visibility = Visibility.Visible;
        }
      }
    }
    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
      ClearAll.Visibility = Visibility.Visible;
      ClearBasic.Visibility = Visibility.Visible;
      var radioButton = (RadioButton)sender;
      if (radioButton.GroupName == "Income")
      {
        ClearType.Visibility = Visibility.Visible;
        if (radioButton.Content.ToString() == "Income") _filterController.Income = true;
        else _filterController.Income = false;
      }
      else
      {
        ClearRecurrence.Visibility = Visibility.Visible;
        if (radioButton.Content.ToString() == "Recurring")
        {
          Recurrences.IsEnabled = true;
          _filterController.Recurring = true;
        }
        else
        {
          Recurrences.IsEnabled = false;
          _filterController.Recurring = false;
        }
      }
    }
    private void NumericUpDown_NumericValueChanged(object sender, EventArgs e)
    {
      ClearAll.Visibility = Visibility.Visible;
      ClearPrices.Visibility = Visibility.Visible;
      var numericBox = (NumericUpDown)sender;
      switch (numericBox.Name)
      {
        case "PriceMin":
          ClearPrice.Visibility = Visibility.Visible;
          if (_filterController.PriceRange == null) _filterController.PriceRange = new DecimalRange(numericBox.NumericValue);
          else _filterController.PriceRange = new DecimalRange(numericBox.NumericValue, _filterController.PriceRange.NumberMax);
          break;
        case "QuantityMin":
          ClearQuantity.Visibility = Visibility.Visible;
          if (_filterController.QuantityRange == null) _filterController.QuantityRange = new DecimalRange(numericBox.NumericValue);
          else _filterController.QuantityRange = new DecimalRange(numericBox.NumericValue, _filterController.QuantityRange.NumberMax);
          break;
        case "TotalMin":
          ClearTotal.Visibility = Visibility.Visible;
          if (_filterController.TotalRange == null) _filterController.TotalRange = new DecimalRange(numericBox.NumericValue);
          else _filterController.TotalRange = new DecimalRange(numericBox.NumericValue, _filterController.TotalRange.NumberMax);
          break;
        case "PriceMax":
          ClearPrice.Visibility = Visibility.Visible;
          if (_filterController.PriceRange == null) _filterController.PriceRange = new DecimalRange(max: numericBox.NumericValue);
          else _filterController.PriceRange = new DecimalRange(_filterController.PriceRange.NumberMin, numericBox.NumericValue);
          break;
        case "QuantityMax":
          ClearQuantity.Visibility = Visibility.Visible;
          if (_filterController.QuantityRange == null) _filterController.QuantityRange = new DecimalRange(max: numericBox.NumericValue);
          else _filterController.QuantityRange = new DecimalRange(_filterController.QuantityRange.NumberMin, numericBox.NumericValue);
          break;
        case "TotalMax":
          ClearTotal.Visibility = Visibility.Visible;
          if (_filterController.TotalRange == null) _filterController.TotalRange = new DecimalRange(max: numericBox.NumericValue);
          else _filterController.TotalRange = new DecimalRange(_filterController.TotalRange.NumberMin, numericBox.NumericValue);
          break;
      }
    }
    private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      ClearAll.Visibility = Visibility.Visible;
      ClearDates.Visibility = Visibility.Visible;
      var datePicker = (DatePicker)sender;
      switch (datePicker.Name)
      {
        case "SubmitDateMin":
          ClearDateAdded.Visibility = Visibility.Visible;
          if (_filterController.SubmitDateRange == null) _filterController.SubmitDateRange = new DateRange(datePicker.SelectedDate);
          else _filterController.SubmitDateRange = new DateRange(datePicker.SelectedDate, _filterController.SubmitDateRange.EndDate);
          break;
        case "UpdateDateMin":
          ClearDateUpdated.Visibility = Visibility.Visible;
          if (_filterController.UpdateDateRange == null) _filterController.UpdateDateRange = new DateRange(datePicker.SelectedDate);
          else _filterController.UpdateDateRange = new DateRange(datePicker.SelectedDate, _filterController.UpdateDateRange.EndDate);
          break;
        case "UserDateMin":
          ClearDateOccurred.Visibility = Visibility.Visible;
          if (_filterController.UserDateRange == null) _filterController.UserDateRange = new DateRange(datePicker.SelectedDate);
          else _filterController.UserDateRange = new DateRange(datePicker.SelectedDate, _filterController.UserDateRange.EndDate);
          break;
        case "SubmitDateMax":
          ClearDateAdded.Visibility = Visibility.Visible;
          if (_filterController.SubmitDateRange == null) _filterController.SubmitDateRange = new DateRange(endDate: datePicker.SelectedDate);
          else _filterController.SubmitDateRange = new DateRange(_filterController.SubmitDateRange.StartDate, datePicker.SelectedDate);
          break;
        case "UpdateDateMax":
          ClearDateUpdated.Visibility = Visibility.Visible;
          if (_filterController.UpdateDateRange == null) _filterController.UpdateDateRange = new DateRange(endDate: datePicker.SelectedDate);
          else _filterController.UpdateDateRange = new DateRange(_filterController.UpdateDateRange.StartDate, datePicker.SelectedDate);
          break;
        case "UserDateMax":
          ClearDateOccurred.Visibility = Visibility.Visible;
          if (_filterController.UserDateRange == null) _filterController.UserDateRange = new DateRange(endDate: datePicker.SelectedDate);
          else _filterController.UserDateRange = new DateRange(_filterController.UserDateRange.StartDate, datePicker.SelectedDate);
          break;
      }
    }
    #endregion

    /// <summary>
    /// Methods governing SelectionChanged events of individual ListBoxes
    /// </summary>
    #region Lists
    private void CategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      List<string> catList = new();
      List<string> subcatList = new();

      ClearAll.Visibility = Visibility.Visible;
      ClearLists.Visibility = Visibility.Visible;
      ClearCategory.Visibility = Visibility.Visible;
      if (CategoriesList.SelectedItems.Count != 0)
      {

        foreach (var category in CategoriesList.SelectedItems)
        {
#pragma warning disable CS8604 // Możliwy argument odwołania o wartości null.
          catList.Add(category.ToString());
#pragma warning restore CS8604 // Możliwy argument odwołania o wartości null.
#pragma warning disable CS8604 // Możliwy argument odwołania o wartości null.
          subcatList.AddRange(DatabaseModel.GetSubcategoriesNames(category.ToString()));
#pragma warning restore CS8604 // Możliwy argument odwołania o wartości null.
        }
        _filterController.Categories = catList;
      }
      else _filterController.Categories = null;

      if (subcatList.Count == 0)
      {
        _filterController.Subcategories = null;
        SubcategoriesList.ItemsSource = null;
        Subcategories.IsExpanded = false;
        Subcategories.IsEnabled = false;
      }
      else
      {
        _filterController.Subcategories = subcatList;
        SubcategoriesList.ItemsSource = subcatList;
        Subcategories.IsEnabled = true;
      }
    }
    private void SubcategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ClearAll.Visibility = Visibility.Visible;
      ClearLists.Visibility = Visibility.Visible;
      ClearSubcategory.Visibility = Visibility.Visible;
      if (SubcategoriesList.SelectedItems.Count != 0)
      {
        List<string> subcatList = new();
        foreach (var subcategory in SubcategoriesList.SelectedItems)
        {
#pragma warning disable CS8604 // Możliwy argument odwołania o wartości null.
          subcatList.Add(subcategory.ToString());
#pragma warning restore CS8604 // Możliwy argument odwołania o wartości null.
        }
        _filterController.Subcategories = subcatList;
      }
      else
      {
        _filterController.Subcategories = null;
      }
    }
    private void RecurrencesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ClearAll.Visibility = Visibility.Visible;
      ClearLists.Visibility = Visibility.Visible;
      ClearRecurrenceList.Visibility = Visibility.Visible;
      if (RecurrencesList.SelectedItems.Count != 0)
      {
        List<string> recList = new();
        foreach (var recurrence in RecurrencesList.SelectedItems)
        {
#pragma warning disable CS8604 // Możliwy argument odwołania o wartości null.
          recList.Add(recurrence.ToString());
#pragma warning restore CS8604 // Możliwy argument odwołania o wartości null.
        }
        _filterController.Recurrances = recList;
      }
      else
      {
        _filterController.Recurrances = null;
      }
    }
    #endregion

    /// <summary>
    /// When expanding Expander, contract rest of Expanders
    /// </summary>
    private void Expander_Expanded(object sender, RoutedEventArgs e)
    {
      //To save space in control, only one Expander can be expanded
      foreach (var control in GetChildElements(ListGroup))
      {
        if (control is Expander expander && control != sender)
        {
          expander.IsExpanded = false;
        }
      }
    }

    /// <summary>
    /// Methods clearing individual groups of filters
    /// </summary>
    #region ClearButtons
    private void ClearButtonBasic_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var clearButton = sender as ClearButton;

      if (clearButton.Name == "ClearBasic" || clearButton.Name == "ClearType")
      {
        IncomeT.IsChecked = false;
        IncomeF.IsChecked = false;
        _filterController.Income = null;
      }
      if (clearButton.Name == "ClearBasic" || clearButton.Name == "ClearRecurrence")
      {
        RecurringT.IsChecked = false;
        RecurringF.IsChecked = false;
        _filterController.Recurring = null;
      }
      if (clearButton.Name == "ClearBasic")
      {
        SearchBox.Text = "Search database...";
        _filterController.Name = null;
        foreach (var control in GetChildElements(BasicGroup)) if (control is ClearButton button) button.Visibility = Visibility.Hidden;
      }
      clearButton.Visibility = Visibility.Hidden;
    }
    private void ClearButtonPrices_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var clearButton = sender as ClearButton;

      if (clearButton.Name == "ClearPrices" || clearButton.Name == "ClearPrice")
      {
        PriceMin.Clear();
        PriceMax.Clear();
        _filterController.PriceRange = null;
      }
      if (clearButton.Name == "ClearPrices" || clearButton.Name == "ClearQuantity")
      {
        QuantityMin.Clear();
        QuantityMax.Clear();
        _filterController.QuantityRange = null;
      }
      if (clearButton.Name == "ClearPrices" || clearButton.Name == "ClearTotal")
      {
        TotalMin.Clear();
        TotalMax.Clear();
        _filterController.TotalRange = null;
      }
      if (clearButton.Name == "ClearPrices")
      {
        foreach (var control in GetChildElements(PriceGroup)) if (control is ClearButton button) button.Visibility = Visibility.Hidden;
      }
      clearButton.Visibility = Visibility.Hidden;
    }
    private void ClearButtonDates_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var clearButton = sender as ClearButton;

      if (clearButton.Name == "ClearDateAdded" || clearButton.Name == "ClearDates")
      {
        SubmitDateMin.SelectedDate = null;
        SubmitDateMax.SelectedDate = null;
        _filterController.SubmitDateRange = null;
      }
      if (clearButton.Name == "ClearDateUpdated" || clearButton.Name == "ClearDates")
      {
        UpdateDateMin.SelectedDate = null;
        UpdateDateMax.SelectedDate = null;
        _filterController.UpdateDateRange = null;
      }
      if (clearButton.Name == "ClearDateOccurred" || clearButton.Name == "ClearDates")
      {
        UserDateMin.SelectedDate = null;
        UserDateMax.SelectedDate = null;
        _filterController.UserDateRange = null;
      }
      if (clearButton.Name == "ClearDates")
      {
        foreach (var control in GetChildElements(DateGroup)) if (control is ClearButton button) button.Visibility = Visibility.Hidden;
      }
      clearButton.Visibility = Visibility.Hidden;
    }
    private void ClearButtonLists_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var clearButton = sender as ClearButton;

      if (clearButton.Name == "ClearCategory" || clearButton.Name == "ClearLists")
      {
        CategoriesList.UnselectAll();
        _filterController.Categories = null;
      }
      if (clearButton.Name == "ClearSubcategory" || clearButton.Name == "ClearLists")
      {
        SubcategoriesList.UnselectAll();
        _filterController.Subcategories = null;
      }
      if (clearButton.Name == "ClearRecurrenceList" || clearButton.Name == "ClearLists")
      {
        RecurrencesList.UnselectAll();
        _filterController.Recurrances = null;
      }
      if (clearButton.Name == "ClearLists")
      {
        foreach (var control in GetChildElements(ListGroup)) if (control is ClearButton button) button.Visibility = Visibility.Hidden;
      }
      clearButton.Visibility = Visibility.Hidden;
    }
    #endregion

    /// <summary>
    /// Finds all children of parent object
    /// </summary>
    /// <param name="parent">Object of which children are needed</param>
    /// <returns>List of children</returns>
    private List<FrameworkElement> GetChildElements(DependencyObject parent)
    {
      List<FrameworkElement> elements = new();
      int count = VisualTreeHelper.GetChildrenCount(parent);
      for (int i = 0; i < count; i++)
      {
        DependencyObject child = VisualTreeHelper.GetChild(parent, i);
        if (child is FrameworkElement frameworkElement)
        {
          elements.Add(frameworkElement);
          elements.AddRange(GetChildElements(child));
        }
      }
      return elements;
    }
  }
}