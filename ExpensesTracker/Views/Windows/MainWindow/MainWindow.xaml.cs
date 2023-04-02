using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExpensesTracker
{
  /// <summary>
  /// Logika interakcji dla klasy MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    public MainWindow()
    {
      InitializeComponent();
    }

    private void Menu_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var menuColumn = Layout.ColumnDefinitions.First();
      Visibility textState;
      if (menuColumn.ActualWidth == 120)
      {
        textState = Visibility.Collapsed;
        menuColumn.Width = new GridLength(42);
      }
      else
      {
        textState = Visibility.Visible;
        menuColumn.Width = new GridLength(120);
      }

      foreach (StackPanel stackPanel in MenuSP.Children)
      {
        foreach (var control in stackPanel.Children)
        {
          if (control is TextBlock textBlock)
          {
            textBlock.Visibility = textState;
          }
        }
      }
    }

    private void Bar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        DragMove();
      }
    }

    private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
    {
      Close();
    }
    private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
    {
      WindowState = WindowState.Minimized;
    }
    private void Maximize_MouseDown(object sender, MouseButtonEventArgs e)
    {
      WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {

    }
  }
}
