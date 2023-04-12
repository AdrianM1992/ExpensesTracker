using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ExpensesTracker.Views.Windows.AddEditDB
{
  /// <summary>
  /// Logika interakcji dla klasy AddEditDBWindow.xaml
  /// </summary>
  public partial class AddEditDBWindow : Window
  {
    private Dictionary<TextBox, bool> _textBoxes;

    public AddEditDBWindow()
    {
      InitializeComponent();
      foreach (var control in this.LogicalChildren.LogicalTreeHelper.GetChildren(this))
      {
        if (control is TextBox textBox)
        {
          _textBoxes.Add(textBox, false);
        }
      }
    }
    private void TitleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        DragMove();
      }
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      var textBox = sender as TextBox;
      if (!_textBoxes[textBox])
      {
        textBox.Text = string.Empty;
        textBox.FontStyle = FontStyles.Normal;
        textBox.Foreground = Brushes.Black;
        _textBoxes[textBox] = true;
      }
    }
  }
}
