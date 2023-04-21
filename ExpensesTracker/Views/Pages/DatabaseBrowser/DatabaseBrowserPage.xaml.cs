using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.ViewModels;
using ExpensesTracker.Views.Classes;
using ExpensesTracker.Views.Windows.AddEditDB;
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
    private readonly DatabaseBrowserPageViewModel _viewModel;
    private AddEditDBWindow? addEditDBWindow;
    private readonly IMainSettings _mainSettings;
    private bool _deleteInfoNotShown = true;

    public DatabaseBrowserPage(IMainSettings mainSettings)
    {
      _mainSettings = mainSettings;
      InitializeComponent();
      _viewModel = new DatabaseBrowserPageViewModel(this);
      DatabaseView.ItemsSource = _viewModel.ExpensesItems;
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
          typeText.Binding.StringFormat = "dd/MM//yyyy HH:mm";
          break;
        case "LastUpdate":
          typeText.Header = "Last updated";
          typeText.Binding.StringFormat = "dd/MM//yyyy HH:mm";
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

    /// <summary>
    /// Opens new AddEditDBWindow if reference is null or window is not loaded.
    /// </summary>
    private void AddEditButton_Click(object sender, RoutedEventArgs e)
    {
      var button = (Button)sender;
      OpenAddEditWindow(button.Name == "EditButton");
    }
    private void OpenAddEditWindow(bool editMode)
    {
      if (addEditDBWindow == null || !addEditDBWindow.IsLoaded)
      {
        if (!editMode) addEditDBWindow = new AddEditDBWindow(_mainSettings, editMode: editMode);
        else addEditDBWindow = new AddEditDBWindow(_mainSettings, (DatabaseView)DatabaseView.SelectedItem);
        addEditDBWindow.AddListenerToAddEditRecordEvent(OnAddEditHandler);
      }
      else
      {
        var action = MessageBox.Show("Another window is already opened.\n\nDo you wish to open new one?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (action == MessageBoxResult.Yes)
        {
          addEditDBWindow.Close();
          if (!editMode) addEditDBWindow = new AddEditDBWindow(_mainSettings, editMode: editMode);
          else addEditDBWindow = new AddEditDBWindow(_mainSettings, (DatabaseView)DatabaseView.SelectedItem);
          addEditDBWindow.AddListenerToAddEditRecordEvent(OnAddEditHandler);
        }
        else addEditDBWindow.Focus();
      }
      addEditDBWindow.Show();
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

    private void SearchBar_LostFocus(object sender, RoutedEventArgs e)
    {
      if (sender is TextBox textBox)
      {
        if (textBox.Text == "")
        {
          textBox.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x5F, 0x5F, 0x5F));
          textBox.FontStyle = FontStyles.Italic;
          textBox.Text = "Search database...";
        }
      }
    }

    private void DatabaseView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      OpenAddEditWindow(true);
    }

    private void OnAddEditHandler()
    {
      _viewModel.RefreshView();
    }
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      if (DatabaseView.Items.Count <= 0) MessageBox.Show("Nothing to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
      else
      {
        if (DatabaseView.SelectedIndex == -1 && _deleteInfoNotShown)
        {
          _deleteInfoNotShown = false;
          MessageBoxResult result = MessageBox.Show("One time message.\n If no row is selected, first element will be deleted.\nContinue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Information);

          if (result == MessageBoxResult.No) return;
          DatabaseView.SelectedIndex = 0;
        }
        else if (DatabaseView.SelectedIndex == -1) DatabaseView.SelectedIndex = 0;

        _viewModel.RemoveRecord((DatabaseView)DatabaseView.SelectedItem);
        _viewModel.RefreshView();
      }
    }

    private void ShowMoreButton_Click(object sender, RoutedEventArgs e)
    {
      _viewModel.LoadMoreRecords();
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      var searchPhrase = (TextBox)sender;
      //_viewModel.SearchRecord((searchPhrase.Text));
    }
  }
}
