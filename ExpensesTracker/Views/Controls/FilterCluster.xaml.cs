using ExpensesTracker.DataTypes;
using ExpensesTracker.Models.DataControllers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Database filtering control
  /// </summary>
  public partial class FilterCluster : UserControl
  {
    private FilterSortController _filterController = new();
    public FilterCluster()
    {
      InitializeComponent();
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
        if (radioButton.Content.ToString() == "Recurring") _filterController.Recurring = true;
        else _filterController.Income = false;
      }
    }

    private void NumericUpDown_NumericValueChanged(object sender, System.EventArgs e)
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
  }
}
