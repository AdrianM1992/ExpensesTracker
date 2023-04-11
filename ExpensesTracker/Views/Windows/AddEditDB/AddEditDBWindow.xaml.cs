using System.Windows;
using System.Windows.Input;

namespace ExpensesTracker.Views.Windows.AddEditDB
{
  /// <summary>
  /// Logika interakcji dla klasy AddEditDBWindow.xaml
  /// </summary>
  public partial class AddEditDBWindow : Window
  {
    public AddEditDBWindow()
    {
      InitializeComponent();
    }
    private void MinMaxClose_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        DragMove();
      }
    }

    private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
    {

    }

    private void Maximize_MouseDown(object sender, MouseButtonEventArgs e)
    {

    }

    private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
    {

    }
  }
}
