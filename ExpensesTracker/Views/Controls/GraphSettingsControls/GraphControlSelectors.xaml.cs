using ExpensesTracker.Models.Settings;
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

    public void SetExistingGraphSettingsReference(GraphSettings graphSettings)
    {
      _graphSettings = graphSettings;
      GeneralSelector.SetExistingGraphSettingsReference(_graphSettings);
      TimeSelector.SetExistingGraphSettingsReference(_graphSettings);
      ValuesSelector.SetExistingGraphSettingsReference(_graphSettings);
    }
  }
}
