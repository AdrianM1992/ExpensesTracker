using ExpensesTracker.DataTypes;
using ExpensesTracker.Models.DataControllers;
using ExpensesTracker.Models.Settings;
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
    private FilterSettings _filterSettings = new();
    private readonly FilterCluster _thisFilterCluster;

    public FilterCluster()
    {
      InitializeComponent();
      SetDefaultValues();
      _thisFilterCluster = this;
      DatabaseModel.SubtablesChanged += DatabaseModel_SubtablesChanged;
    }

    private void DatabaseModel_SubtablesChanged(object? sender, EventArgs e) => ClearAll_MouseLeftButtonDown(ClearAll, null);
    public void SetFilterSettingsRef(FilterSettings filterSettings) => _filterSettings = filterSettings;
    public void SetExistingFilterSettingsRef(FilterSettings filterSettings)
    {
      _filterSettings = filterSettings;
    }
    private void ClearAll_MouseLeftButtonDown(object sender, MouseButtonEventArgs? e)
    {
      SetDefaultValues();
      _filterSettings.ClearAllFilters();
      ClearAll.Visibility = Visibility.Hidden;

      //Exclude ClearButton inside ClearAll StackPanel, because it is hidden as whole
      ClearButton? sendersClearButton = null; ;
      foreach (var control in GetChildElements(this)) if (control is ClearButton clearButton) sendersClearButton = control as ClearButton;
      foreach (var control in GetChildElements(_thisFilterCluster))
      {
        if (control is ClearButton clearButton && control != sendersClearButton) clearButton.Visibility = Visibility.Hidden;
      }
    }
    private void SetDefaultValues()
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
          _filterSettings.Name = searchPhrase.Text;
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
        if (radioButton.Content.ToString() == "Income") _filterSettings.Income = true;
        else _filterSettings.Income = false;
      }
      else
      {
        ClearRecurrence.Visibility = Visibility.Visible;
        if (radioButton.Content.ToString() == "Recurring")
        {
          Recurrences.IsEnabled = true;
          _filterSettings.Recurring = true;
        }
        else
        {
          Recurrences.IsEnabled = false;
          _filterSettings.Recurring = false;
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
          if (_filterSettings.PriceRange == null) _filterSettings.PriceRange = new DecimalRange(numericBox.NumericValue);
          else _filterSettings.PriceRange = new DecimalRange(numericBox.NumericValue, _filterSettings.PriceRange.NumberMax);
          break;
        case "QuantityMin":
          ClearQuantity.Visibility = Visibility.Visible;
          if (_filterSettings.QuantityRange == null) _filterSettings.QuantityRange = new DecimalRange(numericBox.NumericValue);
          else _filterSettings.QuantityRange = new DecimalRange(numericBox.NumericValue, _filterSettings.QuantityRange.NumberMax);
          break;
        case "TotalMin":
          ClearTotal.Visibility = Visibility.Visible;
          if (_filterSettings.TotalRange == null) _filterSettings.TotalRange = new DecimalRange(numericBox.NumericValue);
          else _filterSettings.TotalRange = new DecimalRange(numericBox.NumericValue, _filterSettings.TotalRange.NumberMax);
          break;
        case "PriceMax":
          ClearPrice.Visibility = Visibility.Visible;
          if (_filterSettings.PriceRange == null) _filterSettings.PriceRange = new DecimalRange(max: numericBox.NumericValue);
          else _filterSettings.PriceRange = new DecimalRange(_filterSettings.PriceRange.NumberMin, numericBox.NumericValue);
          break;
        case "QuantityMax":
          ClearQuantity.Visibility = Visibility.Visible;
          if (_filterSettings.QuantityRange == null) _filterSettings.QuantityRange = new DecimalRange(max: numericBox.NumericValue);
          else _filterSettings.QuantityRange = new DecimalRange(_filterSettings.QuantityRange.NumberMin, numericBox.NumericValue);
          break;
        case "TotalMax":
          ClearTotal.Visibility = Visibility.Visible;
          if (_filterSettings.TotalRange == null) _filterSettings.TotalRange = new DecimalRange(max: numericBox.NumericValue);
          else _filterSettings.TotalRange = new DecimalRange(_filterSettings.TotalRange.NumberMin, numericBox.NumericValue);
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
          if (_filterSettings.SubmitDateRange == null) _filterSettings.SubmitDateRange = new DateRange(datePicker.SelectedDate);
          else _filterSettings.SubmitDateRange = new DateRange(datePicker.SelectedDate, _filterSettings.SubmitDateRange.EndDate);
          break;
        case "UpdateDateMin":
          ClearDateUpdated.Visibility = Visibility.Visible;
          if (_filterSettings.UpdateDateRange == null) _filterSettings.UpdateDateRange = new DateRange(datePicker.SelectedDate);
          else _filterSettings.UpdateDateRange = new DateRange(datePicker.SelectedDate, _filterSettings.UpdateDateRange.EndDate);
          break;
        case "UserDateMin":
          ClearDateOccurred.Visibility = Visibility.Visible;
          if (_filterSettings.UserDateRange == null) _filterSettings.UserDateRange = new DateRange(datePicker.SelectedDate);
          else _filterSettings.UserDateRange = new DateRange(datePicker.SelectedDate, _filterSettings.UserDateRange.EndDate);
          break;
        case "SubmitDateMax":
          ClearDateAdded.Visibility = Visibility.Visible;
          if (_filterSettings.SubmitDateRange == null) _filterSettings.SubmitDateRange = new DateRange(endDate: datePicker.SelectedDate);
          else _filterSettings.SubmitDateRange = new DateRange(_filterSettings.SubmitDateRange.StartDate, datePicker.SelectedDate);
          break;
        case "UpdateDateMax":
          ClearDateUpdated.Visibility = Visibility.Visible;
          if (_filterSettings.UpdateDateRange == null) _filterSettings.UpdateDateRange = new DateRange(endDate: datePicker.SelectedDate);
          else _filterSettings.UpdateDateRange = new DateRange(_filterSettings.UpdateDateRange.StartDate, datePicker.SelectedDate);
          break;
        case "UserDateMax":
          ClearDateOccurred.Visibility = Visibility.Visible;
          if (_filterSettings.UserDateRange == null) _filterSettings.UserDateRange = new DateRange(endDate: datePicker.SelectedDate);
          else _filterSettings.UserDateRange = new DateRange(_filterSettings.UserDateRange.StartDate, datePicker.SelectedDate);
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
      List<string> subCatList = new();

      ClearAll.Visibility = Visibility.Visible;
      ClearLists.Visibility = Visibility.Visible;
      ClearCategory.Visibility = Visibility.Visible;
      if (CategoriesList.SelectedItems.Count != 0)
      {
        foreach (var category in CategoriesList.SelectedItems)
        {
          catList.Add((string)category);
          subCatList.AddRange(DatabaseModel.GetSubcategoriesNames((string)category));
        }
        _filterSettings.Categories = catList;
      }
      else _filterSettings.Categories = null;

      if (subCatList.Count == 0)
      {
        _filterSettings.Subcategories = null;
        SubcategoriesList.ItemsSource = null;
        Subcategories.IsExpanded = false;
        Subcategories.IsEnabled = false;
      }
      else
      {
        SubcategoriesList.ItemsSource = subCatList;
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
        List<string> subCatList = new();
        foreach (var subcategory in SubcategoriesList.SelectedItems) subCatList.Add((string)subcategory);

        _filterSettings.Subcategories = subCatList;
      }
      else _filterSettings.Subcategories = null;
    }
    private void RecurrencesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ClearAll.Visibility = Visibility.Visible;
      ClearLists.Visibility = Visibility.Visible;
      ClearRecurrenceList.Visibility = Visibility.Visible;
      if (RecurrencesList.SelectedItems.Count != 0)
      {
        List<string> recList = new();
        foreach (var recurrence in RecurrencesList.SelectedItems) recList.Add((string)recurrence);

        _filterSettings.Recurrences = recList;
      }
      else _filterSettings.Recurrences = null;
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
        _filterSettings.Income = null;
      }
      if (clearButton.Name == "ClearBasic" || clearButton.Name == "ClearRecurrence")
      {
        RecurringT.IsChecked = false;
        RecurringF.IsChecked = false;
        _filterSettings.Recurring = null;
      }
      if (clearButton.Name == "ClearBasic")
      {
        SearchBox.Text = "Search database...";
        _filterSettings.Name = null;
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
        _filterSettings.PriceRange = null;
      }
      if (clearButton.Name == "ClearPrices" || clearButton.Name == "ClearQuantity")
      {
        QuantityMin.Clear();
        QuantityMax.Clear();
        _filterSettings.QuantityRange = null;
      }
      if (clearButton.Name == "ClearPrices" || clearButton.Name == "ClearTotal")
      {
        TotalMin.Clear();
        TotalMax.Clear();
        _filterSettings.TotalRange = null;
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
        _filterSettings.SubmitDateRange = null;
      }
      if (clearButton.Name == "ClearDateUpdated" || clearButton.Name == "ClearDates")
      {
        UpdateDateMin.SelectedDate = null;
        UpdateDateMax.SelectedDate = null;
        _filterSettings.UpdateDateRange = null;
      }
      if (clearButton.Name == "ClearDateOccurred" || clearButton.Name == "ClearDates")
      {
        UserDateMin.SelectedDate = null;
        UserDateMax.SelectedDate = null;
        _filterSettings.UserDateRange = null;
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
        _filterSettings.Categories = null;
      }
      if (clearButton.Name == "ClearSubcategory" || clearButton.Name == "ClearLists")
      {
        SubcategoriesList.UnselectAll();
        _filterSettings.Subcategories = null;
      }
      if (clearButton.Name == "ClearRecurrenceList" || clearButton.Name == "ClearLists")
      {
        RecurrencesList.UnselectAll();
        _filterSettings.Recurrences = null;
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