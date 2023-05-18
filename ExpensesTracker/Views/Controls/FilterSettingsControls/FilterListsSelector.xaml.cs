using ExpensesTracker.Models.DataControllers;
using ExpensesTracker.Models.Settings;
using ExpensesTracker.Views.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExpensesTracker.Views.Controls.FilterSettingsControls
{
  /// <summary>
  /// Lists database filter control
  /// </summary>
  public partial class FilterListsSelector : UserControl, INotifyPropertyChanged, IFilterSettings
  {
    private FilterSettings _filterSettings = new();
    private bool _initFlag = false;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
      if (!_initFlag) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void FilterSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "Recurring") Recurrences.IsEnabled = _filterSettings.Recurring == true;
    }

    #region Notifying properties
    private bool? _clearAll;

    public bool? ClearAllVisible
    {
      get { return _clearAll; }
      set { _clearAll = value; OnPropertyChanged(nameof(ClearAll)); }
    }
    #endregion

    public FilterListsSelector() => InitializeComponent();

    #region IFilterSettings implementation
    public void ClearAll() => ClearButtonLists_MouseLeftButtonDown(ClearLists, null);
    public void SetDefaultValues()
    {
      _initFlag = true;

      foreach (var control in GetChildElements(ListGroup)) if (control is ListBox listBox) listBox.UnselectAll();
      foreach (var control in GetChildElements(ListGroup)) if (control is Expander expander) expander.IsExpanded = false;
      CategoriesList.ItemsSource = DatabaseModel.GetCategoriesNames();
      RecurrencesList.ItemsSource = DatabaseModel.GetRecurringNames();
      Subcategories.IsEnabled = false;
      Recurrences.IsEnabled = false;

      _initFlag = false;
    }
    public void SetExistingFilterSettingsRef(FilterSettings filterSettings)
    {
      _filterSettings = filterSettings;
      _filterSettings.PropertyChanged += FilterSettings_PropertyChanged;
    }

    public void SetFilterSettingsRef(FilterSettings filterSettings)
    {
      SetDefaultValues();

      _initFlag = true;

      _filterSettings = filterSettings;
      _filterSettings.PropertyChanged += FilterSettings_PropertyChanged;
      if (_filterSettings.Categories != null)
      {
        foreach (var category in _filterSettings.Categories)
        {
          if (CategoriesList.Items.Contains(category)) CategoriesList.SelectedItems.Add(category);
        }
      }
      if (_filterSettings.Subcategories != null)
      {
        foreach (var subCategory in _filterSettings.Subcategories)
        {
          if (SubcategoriesList.Items.Contains(subCategory)) SubcategoriesList.SelectedItems.Add(subCategory);
        }
      }
      else Subcategories.IsEnabled = false;
      if (_filterSettings.Recurrences != null && _filterSettings.Recurring == true)
      {
        foreach (var recurrence in _filterSettings.Recurrences)
        {
          if (RecurrencesList.Items.Contains(recurrence)) RecurrencesList.SelectedItems.Add(recurrence);
        }
      }
      else Recurrences.IsEnabled = false;

      _initFlag = false;
    }
    #endregion

    #region Front panel events
    private void CategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      List<string> catList = new();
      List<string> subCatList = new();

      ClearAllVisible = true;
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
      ClearAllVisible = true;
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
      ClearAllVisible = true;
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
    private void ClearButtonLists_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs? e)
    {
      var clearButton = sender as ClearButton;

      if (clearButton.Name == "ClearCategory" || clearButton.Name == "ClearLists")
      {
        CategoriesList.UnselectAll();
        ClearCategory.Visibility = Visibility.Hidden;
        _filterSettings.Categories = null;
      }
      if (clearButton.Name == "ClearSubcategory" || clearButton.Name == "ClearLists")
      {
        SubcategoriesList.UnselectAll();
        ClearSubcategory.Visibility = Visibility.Hidden;
        _filterSettings.Subcategories = null;
      }
      if (clearButton.Name == "ClearRecurrenceList" || clearButton.Name == "ClearLists")
      {
        RecurrencesList.UnselectAll();
        ClearRecurrenceList.Visibility = Visibility.Hidden;
        _filterSettings.Recurrences = null;
      }

      clearButton.Visibility = Visibility.Hidden;
    }
    private void Expander_Expanded(object sender, RoutedEventArgs e)
    {
      //To save space in control, only one Expander can be expanded
      foreach (var control in GetChildElements(ListGroup))
      {
        if (control is Expander expander && control != sender) expander.IsExpanded = false;
      }
    }
    #endregion

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
