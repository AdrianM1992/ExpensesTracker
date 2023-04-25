using System.Windows;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Logika interakcji dla klasy NumericUpDown.xaml
  /// </summary>
  public partial class NumericUpDown : UserControl
  {
    private decimal _lastValue;

    public decimal NumericValue
    {
      get { return (decimal)GetValue(NumericValueProperty); }
      set { SetValue(NumericValueProperty, value); }
    }
    public static readonly DependencyProperty NumericValueProperty =
        DependencyProperty.Register("NumericValue", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(0M));


    public decimal IncrementStep
    {
      get { return (decimal)GetValue(IncrementStepProperty); }
      set { SetValue(IncrementStepProperty, value); }
    }
    public static readonly DependencyProperty IncrementStepProperty =
        DependencyProperty.Register("IncrementStep", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(1M));

    public NumericUpDown()
    {
      InitializeComponent();
    }

    private void ValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      var input = (TextBox)sender;
      if (decimal.TryParse(input.Text, out decimal inputNumber))
      {
        NumericValue = inputNumber;
        _lastValue = inputNumber;
      }
      else
      {
        NumericValue = _lastValue;
        input.Text = _lastValue.ToString();
      }
    }

    private void IncrementButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      NumericValue += IncrementStep;
      ValueTextBox.Text = NumericValue.ToString();
    }

    private void DecrementButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      NumericValue -= IncrementStep;
      ValueTextBox.Text = NumericValue.ToString();
    }
  }
}
