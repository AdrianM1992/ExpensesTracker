﻿using ExpensesTracker.Models.Interfaces;
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
    private readonly DatabaseBrowserPageViewModel _viewModel;
    private AddEditDBWindow? addEditDBWindow;
    private readonly IMainSettings _mainSettings;

    public DatabaseBrowserPage(IMainSettings mainSettings)
    {
      _mainSettings = mainSettings;
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

    /// <summary>
    /// Opens new AddEditDBWindow if reference is null or window is not loaded.
    /// </summary>
    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
      if (addEditDBWindow == null || !addEditDBWindow.IsLoaded) addEditDBWindow = new AddEditDBWindow(_mainSettings); //Items[2]
      else
      {
        var action = MessageBox.Show("Another window is already opened.\n\nDo you wish to open new one?", "Warning", button: MessageBoxButton.YesNo);
        if (action == MessageBoxResult.OK) addEditDBWindow = new AddEditDBWindow(_mainSettings);
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
  }
}
