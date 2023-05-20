using ExpensesTracker.Views.Interfaces;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExpensesTracker.Views.Controls.FilterSettingsControls
{
  /// <summary>
  /// Logika interakcji dla klasy FilterControlSelectors.xaml
  /// </summary>
  public partial class FilterControlSelectors : UserControl, ISettingsSetter
  {
    private readonly List<ISettingsSetter> _filters = new();

    public FilterControlSelectors()
    {
      InitializeComponent();
      _filters.Add(BasicSelector);
      BasicSelector.PropertyChanged += ClearAll_PropertyChanged;
      _filters.Add(PricesSelector);
      PricesSelector.PropertyChanged += ClearAll_PropertyChanged;
      _filters.Add(DatesSelector);
      DatesSelector.PropertyChanged += ClearAll_PropertyChanged;
      _filters.Add(ListsSelector);
      ListsSelector.PropertyChanged += ClearAll_PropertyChanged;
    }

    private void ClearAll_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "ClearAll") ClearAllStackPanel.Visibility = Visibility.Visible;
    }
    private void ClearAll_MouseLeftButtonDown(object sender, MouseButtonEventArgs? e) => ClearAll();

    #region ISettingsSetter implementation
    public void SetDefaultValues()
    {
      foreach (var filter in _filters) filter.SetDefaultValues();
    }

    public void SetNewSettingsRef(object filterSettings)
    {
      foreach (var filter in _filters) filter.SetNewSettingsRef(filterSettings);
    }

    public void SetExistingSettingsRef(object filterSettings)
    {
      foreach (var filter in _filters) filter.SetExistingSettingsRef(filterSettings);
    }

    public void ClearAll()
    {
      SetDefaultValues();
      foreach (var filter in _filters) filter.ClearAll();
      ClearAllStackPanel.Visibility = Visibility.Hidden;
    }
    #endregion
  }
}
