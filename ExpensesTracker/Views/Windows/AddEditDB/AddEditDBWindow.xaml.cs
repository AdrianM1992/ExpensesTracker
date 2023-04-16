using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.ViewModels;
using ExpensesTracker.Views.Classes;
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
    public Dictionary<TextBox, string> _textBoxes;
    readonly private AddEditDBWindowViewModel _viewModel;
    private readonly IMainSettings _mainSettings;

    public AddEditDBWindow(IMainSettings mainSettings)
    {
      _viewModel = new AddEditDBWindowViewModel(_mainSettings, this);
      InitializeComponent();
      CreateTextboxesDictionary();
      _mainSettings = mainSettings;

    }
    public AddEditDBWindow(IMainSettings mainSettings, DatabaseView databaseView)
    {
      _mainSettings = mainSettings;
      _viewModel = new AddEditDBWindowViewModel(_mainSettings, databaseView, this);
      InitializeComponent();
      CreateTextboxesDictionary();
      _viewModel.SetControlsValues();

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

    private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var category = sender as ComboBox;
      _viewModel.SetCategoryValue(category.SelectedValue.ToString());
    }

    private void Subcategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var subcategory = sender as ComboBox;
      _viewModel.SetSubcategoryValue(subcategory.SelectedValue.ToString());
    }

    /// <summary>
    /// Searches controls list and picks TextBoxes to create dictionary keys; values are default text
    /// </summary>
    private void CreateTextboxesDictionary()
    {
      _textBoxes = new Dictionary<TextBox, string>();
      foreach (var textBox in GetAllControls(this).OfType<TextBox>())
      {
        _textBoxes.Add(textBox, textBox.Text);
      }
    }

    /// <summary>
    /// Creates list of all controls in parent hierarchy tree
    /// </summary>
    /// <param name="parent">Reference object to list children"</param>
    /// <returns>List of all children controls</returns>
    private List<Control> GetAllControls(DependencyObject parent)
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
          controls.AddRange(GetAllControls(childDependencyObject));
        }
      }
      return controls;
    }

    private void Income_Checked(object sender, RoutedEventArgs e)
    {
      var income = sender as RadioButton;

      if (income.Name == "Income") _viewModel.SetIncome(true);
      else if (income.Name == "Expense") _viewModel.SetIncome(false);
    }

    private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      var date = sender as DatePicker;
      _viewModel.SetDate(date.SelectedDate.Value);
    }
  }
}
