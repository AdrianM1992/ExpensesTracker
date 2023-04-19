using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.ViewModels;
using ExpensesTracker.Views.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static ExpensesTracker.Views.Delegates.ViewDelegates;

namespace ExpensesTracker.Views.Windows.AddEditDB
{
  /// <summary>
  /// Logic for AddEditDBWindow.xaml
  /// </summary>
  public partial class AddEditDBWindow : Window
  {
    private readonly AddEditDBWindowViewModel _viewModel;
    private readonly IMainSettings _mainSettings;
    private bool _savedFlag = false;
    private readonly bool editMode;
    private event AddEditRecordHandler? AddEditRecordEvent;

    public Dictionary<TextBox, string> _textBoxes;

    public AddEditDBWindow(IMainSettings mainSettings, DatabaseView? databaseView = null, bool editMode = true)
    {
      _mainSettings = mainSettings;
      if (databaseView == null) _viewModel = new AddEditDBWindowViewModel(_mainSettings, this);
      else _viewModel = new AddEditDBWindowViewModel(_mainSettings, this, databaseView);
      InitializeComponent();
      _textBoxes = CreateTextboxesDictionary();
      _viewModel.SetControlsValues();
      this.editMode = editMode;
    }

    public void AddListenerToAddEditRecordEvent(AddEditRecordHandler onAddEditRecordHandler)
    {
      AddEditRecordEvent += onAddEditRecordHandler;
    }

    private void TitleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed) DragMove();
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      var textBox = (TextBox)sender;
      if (_textBoxes[textBox] == textBox.Text) textBox.Text = string.Empty;
      textBox.FontStyle = FontStyles.Normal;
      textBox.Foreground = Brushes.Black;
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      var textBox = (TextBox)sender;
      if (textBox.Text == "")
      {
        textBox.Text = _textBoxes[textBox];
        textBox.FontStyle = FontStyles.Italic;
        textBox.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x5F, 0x5F, 0x5F));
      }
      else _viewModel.HandleTextboxInput(textBox);
    }

    /// <summary>
    /// Searches controls list and picks TextBoxes to create dictionary keys; values are default text
    /// </summary>
    private Dictionary<TextBox, string> CreateTextboxesDictionary()
    {
      var textBoxes = new Dictionary<TextBox, string>();
      foreach (var textBox in GetAllControls(this).OfType<TextBox>())
      {
        textBoxes.Add(textBox, textBox.Text);
      }
      return textBoxes;
    }

    /// <summary>
    /// Creates list of all controls in parent hierarchy tree
    /// </summary>
    /// <param name="parent">Reference object to list children"</param>
    /// <returns>List of all children controls</returns>
    private List<Control> GetAllControls(DependencyObject parent)
    {
      List<Control> controls = new();

      foreach (var child in LogicalTreeHelper.GetChildren(parent))
      {
        if (child is Control control) controls.Add(control);
        if (child is DependencyObject childDependencyObject) controls.AddRange(GetAllControls(childDependencyObject));
      }
      return controls;
    }

    private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      var date = (DatePicker)sender;
      if (date.SelectedDate.HasValue) _viewModel.SetDate(date.SelectedDate.Value);
      else _viewModel.SetDate(null);

    }

    /// <summary>
    /// DropDownClosed is chosen over SelectionChanged, because SelectionChanged fires when viewModel sets SelectedIndex
    /// </summary>
    #region ComboBox events
    private void Subcategory_DropDownClosed(object sender, System.EventArgs e)
    {
      var subcategory = (ComboBox)sender;
      _viewModel.SetSubcategoryValue(subcategory.SelectedValue.ToString());
    }

    private void Category_DropDownClosed(object sender, System.EventArgs e)
    {
      var category = (ComboBox)sender;
      _viewModel.SetCategoryValue(category.SelectedValue.ToString());
    }

    private void RecurringId_DropDownClosed(object sender, System.EventArgs e)
    {
      var recurringId = (ComboBox)sender;
      _viewModel.SetRecurringId(recurringId.SelectedValue.ToString());
    }
    #endregion

    private void Recurring_Checked(object sender, RoutedEventArgs e)
    {
      var checkBox = (CheckBox)sender;
      RecurringList.IsEnabled = checkBox.IsChecked == true;
      RecurringId.ItemsSource = null;
      if (checkBox.IsChecked.HasValue) _viewModel.SetRecurring(checkBox.IsChecked.Value);
    }
    private void Income_Checked(object sender, RoutedEventArgs e)
    {
      var income = sender as RadioButton;
      if (income.Name == "Income") _viewModel.SetIncome(true);
      else _viewModel.SetIncome(false);
    }

    private void AddRecord_Click(object sender, RoutedEventArgs e)
    {
      _savedFlag = true;
      _viewModel.CommitChanges(editMode);
      AddEditRecordEvent?.Invoke();
      Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void AddEditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!_savedFlag)
      {
        MessageBoxResult result = MessageBox.Show("Changes are not saved. \nExit anyway?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.No) e.Cancel = true;
      }
    }
  }
}
