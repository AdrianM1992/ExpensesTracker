using ExpensesTracker.DataTypes;
using ExpensesTracker.Models.Settings;
using ExpensesTracker.Views.Interfaces;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls.FilterSettingsControls
{
  /// <summary>
  /// Prices database filter control
  /// </summary>
  public partial class FilterPricesSelector : UserControl, INotifyPropertyChanged, IFilterSettings
  {
    private FilterSettings _filterSettings = new();
    private bool _initFlag = false;

    #region Notifying properties
    private bool? _clearAll;

    public bool? ClearAllVisible
    {
      get { return _clearAll; }
      set { _clearAll = value; OnPropertyChanged(nameof(ClearAll)); }
    }
    #endregion

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
      if (!_initFlag) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public FilterPricesSelector() => InitializeComponent();

    #region IFilterSettings implementation
    public void ClearAll() => ClearButtonPrices_MouseLeftButtonDown(ClearPrices, null);
    public void SetDefaultValues()
    {
      PriceMin.Clear();
      PriceMax.Clear();
      QuantityMin.Clear();
      QuantityMax.Clear();
      TotalMin.Clear();
      TotalMax.Clear();
    }
    public void SetExistingFilterSettingsRef(FilterSettings filterSettings)
    {
      _initFlag = true;

      _filterSettings = filterSettings;
      if (_filterSettings.PriceRange != null)
      {
        PriceMin.NumericValue = _filterSettings.PriceRange.NumberMin;
        PriceMax.NumericValue = _filterSettings.PriceRange.NumberMax;
      }
      if (_filterSettings.QuantityRange != null)
      {
        QuantityMin.NumericValue = _filterSettings.QuantityRange.NumberMin;
        QuantityMax.NumericValue = _filterSettings.QuantityRange.NumberMax;
      }
      if (_filterSettings.TotalRange != null)
      {
        TotalMin.NumericValue = _filterSettings.TotalRange.NumberMin;
        TotalMax.NumericValue = _filterSettings.TotalRange.NumberMax;
      }

      _initFlag = false;
    }
    public void SetFilterSettingsRef(FilterSettings filterSettings) => _filterSettings = filterSettings;
    #endregion

    #region Front panel events
    private void NumericUpDown_NumericValueChanged(object sender, EventArgs e)
    {
      ClearAllVisible = true;
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
    private void ClearButtonPrices_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs? e)
    {
      var clearButton = sender as ClearButton;

      if (clearButton.Name == "ClearPrices" || clearButton.Name == "ClearPrice")
      {
        PriceMin.Clear();
        PriceMax.Clear();
        ClearPrice.Visibility = Visibility.Hidden;
        _filterSettings.PriceRange = null;
      }
      if (clearButton.Name == "ClearPrices" || clearButton.Name == "ClearQuantity")
      {
        QuantityMin.Clear();
        QuantityMax.Clear();
        ClearQuantity.Visibility = Visibility.Hidden;
        _filterSettings.QuantityRange = null;
      }
      if (clearButton.Name == "ClearPrices" || clearButton.Name == "ClearTotal")
      {
        TotalMin.Clear();
        TotalMax.Clear();
        ClearTotal.Visibility = Visibility.Hidden;
        _filterSettings.TotalRange = null;
      }

      clearButton.Visibility = Visibility.Hidden;
    }
    #endregion
  }
}
