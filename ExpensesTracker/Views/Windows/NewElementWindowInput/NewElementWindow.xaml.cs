using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ExpensesTracker.Views.Windows.NewElementWindowInput
{
  /// <summary>
  /// Logic for NewElementWindow.xaml
  /// </summary>
  public partial class NewElementWindow : Window
  {
    /// <summary>
    /// Inputed value to be accessed before closing window
    /// </summary>
    public string NewElementName { get; private set; }

    /// <summary>
    /// Customizable request for input
    /// </summary>
    public string Header
    {
      get { return (string)GetValue(HeaderProperty); }
      set { SetValue(HeaderProperty, value); }
    }
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register("Header", typeof(string), typeof(NewElementWindow), new PropertyMetadata(""));

    public NewElementWindow()
    {
      InitializeComponent();
      NewElementName = string.Empty;
    }

    /// <summary>
    /// Sets accessible property
    /// </summary>
    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
      NewElementName = ElementNameValue.Text;
      //If TextBox is left blank, blink it to notify user
      if (NewElementName != string.Empty) this.DialogResult = true;
      else
      {
        BlinkTextBoxOnce(ElementNameValue);
      }

    }

    /// <summary>
    /// Do nothing, just exit
    /// </summary>
    private void CancelButton_Click(object sender, RoutedEventArgs e) => this.DialogResult = false;

    /// <summary>
    /// Allows user to move window around
    /// </summary>
    private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();

    /// <summary>
    /// Blink control to get attention
    /// </summary>
    /// <param name="textBox">TextBox to blink</param>
    private void BlinkTextBoxOnce(TextBox textBox)
    {
      DoubleAnimation animation = new DoubleAnimation
      {
        From = 1,
        To = 0,
        Duration = new Duration(TimeSpan.FromSeconds(0.25)),
        AutoReverse = true
      };

      textBox.BeginAnimation(UIElement.OpacityProperty, animation);
      animation.FillBehavior = FillBehavior.Stop;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) => ElementNameValue.Focus();
  }
}
