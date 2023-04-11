using ExpensesTracker.ViewModels;
using ExpensesTracker.Views.Classes;
using ExpensesTracker.Views.Windows.AddEditDB;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExpensesTracker.Views.Pages.DatabaseBrowser
{
  /// <summary>
  /// Logic for DatabaseBrowserPage.xaml
  /// </summary>
  public partial class DatabaseBrowserPage : Page
  {
    public ObservableCollection<DatabaseView> Items { get; set; }
    private DatabaseBrowserPageViewModel _viewModel;
    public DatabaseBrowserPage()
    {
      InitializeComponent();
      _viewModel = new DatabaseBrowserPageViewModel(this);
      Items = _viewModel.Expenses;
      DatabaseView.ItemsSource = Items;
    }

    /// <summary>
    /// Intercepts generated column and overwrites properties based on name
    /// </summary>
    private void DatabaseView_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
      var typeText = e.Column as DataGridTextColumn;
      var typeBool = e.Column as DataGridCheckBoxColumn;
      switch (e.Column.Header)
      {
        case "Name":
          typeText.MaxWidth = 200;
          typeText.ElementStyle = new Style(typeof(TextBlock));
          typeText.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
          break;
        case "Price":
          typeText.Binding.StringFormat = ".00 zł";
          break;
        case "Quantity":
          typeText.Header = "Qty";
          break;
        case "Total":
          typeText.Binding.StringFormat = ".00 zł";
          typeText.CanUserSort = true;
          break;
        case "DateOfEntry":
          typeText.Header = "Submit date";
          break;
        case "LastUpdate":
          typeText.Header = "Last updated";
          break;
        case "Date":
          typeText.Binding.StringFormat = "d";
          typeText.CanUserSort = true;
          break;
        case "Income":
          typeBool.Header = "Income?";
          break;
        case "Recurring":
          typeBool.Header = "Recurring?";
          break;
        case "RecurringId":
          typeText.Header = "Recurring Name";
          break;
        case "Description":
          typeText.MaxWidth = 200;
          typeText.ElementStyle = new Style(typeof(TextBlock));
          typeText.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
          break;
        default:
          break;
      }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
      AddEditDBWindow window = new AddEditDBWindow();
      window.Show();
    }

    private void SearchBar_GotFocus(object sender, RoutedEventArgs e)
    {
      if (sender is TextBox textBox)
      {
        if (textBox.Text == "Search database...")
        {
          textBox.Foreground = Brushes.Black;
          textBox.FontStyle = FontStyles.Normal;
          textBox.Text = "";
        }
      }
    }
  }
}
