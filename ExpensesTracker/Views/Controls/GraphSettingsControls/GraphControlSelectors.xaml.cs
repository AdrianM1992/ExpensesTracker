using ExpensesTracker.Views.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Logika interakcji dla klasy GraphControlSelectors.xaml
  /// </summary>
  public partial class GraphControlSelectors : UserControl, ISettingsSetter
  {
    private readonly List<ISettingsSetter> _filters = new();

    public GraphControlSelectors()
    {
      InitializeComponent();
      _filters.Add(TypeSelector);
      TypeSelector.PropertyChanged += ClearAll_PropertyChanged;
      _filters.Add(TimeSelector);
      TimeSelector.PropertyChanged += ClearAll_PropertyChanged;
      _filters.Add(ValuesSelector);
      ValuesSelector.PropertyChanged += ClearAll_PropertyChanged;
    }

    private void ClearAll_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "ClearAll") ClearAllStackPanel.Visibility = Visibility.Visible;
    }
    private void ClearAll_MouseLeftButtonDown(object sender, MouseButtonEventArgs? e) => ClearAll();

    #region ISettingsSetter implementation
    public void SetDefaultValues()
    {
      foreach (var filter in _filters) filter.SetDefaultValues();
    }
    public void SetNewSettingsRef(object graphSettings)
    {
      foreach (var filter in _filters) filter.SetNewSettingsRef(graphSettings);
    }
    public void SetExistingSettingsRef(object graphSettings)
    {
      foreach (var filter in _filters) filter.SetExistingSettingsRef(graphSettings);
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
