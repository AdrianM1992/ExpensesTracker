using ExpensesTracker.Models.DataControllers;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Logika interakcji dla klasy GraphControlSelectors.xaml
  /// </summary>
  public partial class GraphControlSelectors : UserControl
  {
    private GraphSettings _graphSettings = new();

    public GraphControlSelectors()
    {
      InitializeComponent();
    }

    public void SetGraphSettingsReference(GraphSettings graphSettings)
    {
      _graphSettings = graphSettings;
      GeneralSelector.SetGraphSettingsReference(_graphSettings);
      TimeSelector.SetGraphSettingsReference(_graphSettings);
      ValuesSelector.SetGraphSettingsReference(_graphSettings);
    }
  }
}
