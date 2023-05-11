using ExpensesTracker.Models.Interfaces;
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
    private readonly IMainSettings _mainSettings;
    public GraphControl(IMainSettings mainSettings)
    {
      InitializeComponent();
      _mainSettings = mainSettings;
      _viewModel = new GraphControlViewModel(this, _mainSettings);
    }

    private void Expander_Expanded(object sender, System.Windows.RoutedEventArgs e)
    {
      var expander = (Expander)sender;
      if (expander.Name == "FilterExpander") SelectorsExpander.IsExpanded = false;
      else FilterExpander.IsExpanded = false;
    }
  }
}
