using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Logic of interaction for CustomMinMaxCloseButtons.xaml
  /// </summary>
  public partial class CustomMinMaxCloseButtons : UserControl
  {
    public CustomMinMaxCloseButtons()
    {
      InitializeComponent();
    }

    //Methods handling basic interaction events with window (min, max, close)
    private void Minimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Window.GetWindow(this).WindowState = WindowState.Minimized;
    }

    private void Maximize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Window parentWindow = Window.GetWindow(this);
      parentWindow.WindowState = parentWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Window.GetWindow(this)?.Close();
    }
  }
}
