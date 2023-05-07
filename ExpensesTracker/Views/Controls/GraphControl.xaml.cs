using System.Windows.Controls;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Logika interakcji dla klasy GraphControl.xaml
  /// </summary>
  public partial class GraphControl : UserControl
  {
    private readonly GraphControl _instance;
    public GraphControl()
    {
      InitializeComponent();
      _instance = this;
    }

  }
}
