using ExpensesTracker.ViewModels;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Logika interakcji dla klasy GraphControl.xaml
  /// </summary>
  public partial class GraphControl : UserControl
  {
    private readonly GraphControlViewModel _viewModel;
    public GraphControl()
    {
      InitializeComponent();
      _viewModel = new GraphControlViewModel(this);
    }

    private void Expander_Expanded(object sender, System.Windows.RoutedEventArgs e)
    {
      var expander = (Expander)sender;
      if (expander.Name == "FilterExpander") SelectorsExpander.IsExpanded = false;
      else FilterExpander.IsExpanded = false;
    }
  }
}
