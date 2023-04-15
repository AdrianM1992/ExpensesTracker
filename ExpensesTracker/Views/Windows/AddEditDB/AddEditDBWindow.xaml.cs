using ExpensesTracker.ViewModels;
using System.Collections.Generic;
using System.Linq;
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
    readonly private Dictionary<TextBox, string> _textBoxes;
    readonly private AddEditDBWindowViewModel _viewModel;

    public AddEditDBWindow()
    {
      InitializeComponent();
      _viewModel = new AddEditDBWindowViewModel();
      _textBoxes = new Dictionary<TextBox, string>();
      foreach (var textBox in GetAllTextboxes(this).OfType<TextBox>())
      {
        _textBoxes.Add(textBox, textBox.Text);
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
      if (_textBoxes[textBox] == textBox.Text)
      {
        textBox.Text = string.Empty;
        textBox.FontStyle = FontStyles.Normal;
        textBox.Foreground = Brushes.Black;
      }
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      var textBox = sender as TextBox;
      if (textBox.Text == "")
      {
        textBox.Text = _textBoxes[textBox];
        textBox.FontStyle = FontStyles.Italic;
        textBox.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x5F, 0x5F, 0x5F));
      }
    }
    private List<Control> GetAllTextboxes(DependencyObject parent)
    {
      List<Control> controls = new List<Control>();

      foreach (var child in LogicalTreeHelper.GetChildren(parent))
      {
        if (child is Control control)
        {
          controls.Add(control);
        }

        if (child is DependencyObject childDependencyObject)
        {
          controls.AddRange(GetAllTextboxes(childDependencyObject));
        }
      }
      return controls;
    }

  }
}
