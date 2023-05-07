using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ExpensesTracker.Views.Classes
{
  public class SubstractConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null || parameter == null || string.IsNullOrWhiteSpace(value.ToString()) || string.IsNullOrWhiteSpace(parameter.ToString()))
      {
        return DependencyProperty.UnsetValue;
      }
      double input = (double)value;
      double subtract;
      if (!double.TryParse(parameter.ToString(), out subtract))
      {
        return DependencyProperty.UnsetValue;
      }

      return input - subtract;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
