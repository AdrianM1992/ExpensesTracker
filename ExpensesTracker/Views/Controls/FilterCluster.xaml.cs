using ExpensesTracker.Models.DataControllers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Database filtering control
  /// </summary>
  public partial class FilterCluster : UserControl
  {
    private FilterSortController _filterController = new();
    public FilterCluster()
    {
      InitializeComponent();
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

    //this will be replaced
    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (sender is TextBox searchPhrase)
      {
        //if (searchPhrase.Text != "Search database...") _viewModel.SearchRecord((searchPhrase.Text));
      }
    }
  }
}
