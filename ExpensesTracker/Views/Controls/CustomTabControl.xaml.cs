using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static ExpensesTracker.Views.Delegates.ViewDelegates;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Interaction logic for CustomTabControl.xaml
  /// </summary>
  public partial class CustomTabControl : UserControl
  {
    /// <summary>
    /// Custom event that informs main window to take specific actions
    /// </summary>
    public event CustomTabEventHandler? CustomTabChanged;

    #region Dependency properties
    /// <summary>
    /// Name of tab in top bar
    /// </summary>
    public string TabName
    {
      get { return (string)GetValue(TabNameProperty); }
      set { SetValue(TabNameProperty, value); }
    }
    public static readonly DependencyProperty TabNameProperty =
        DependencyProperty.Register("TabName", typeof(string), typeof(CustomTabControl), new PropertyMetadata(""));

    /// <summary>
    /// Visibility state of close button
    /// </summary>
    public Visibility CloseTabVisible
    {
      get { return (Visibility)GetValue(CloseTabVisibleProperty); }
      set { SetValue(CloseTabVisibleProperty, value); }
    }
    public static readonly DependencyProperty CloseTabVisibleProperty =
        DependencyProperty.Register("CloseTabVisible", typeof(Visibility), typeof(CustomTabControl), new PropertyMetadata(Visibility.Visible));


    /// <summary>
    /// Background color of tab
    /// </summary>
    public Brush BackgroundTabColor
    {
      get { return (Brush)GetValue(BackgroundTabColorProperty); }
      set { SetValue(BackgroundTabColorProperty, value); }
    }
    public static readonly DependencyProperty BackgroundTabColorProperty =
        DependencyProperty.Register("BackgroundTabColor", typeof(Brush), typeof(CustomTabControl), new PropertyMetadata(SystemColors.MenuBarBrush));
    #endregion

    public CustomTabControl()
    {
      InitializeComponent();
    }

    #region Custom event rising methods
    private void Button_Click(object sender, RoutedEventArgs e)
    {
      CustomTabChanged?.Invoke(TabName, true);
    }

    private void SelectTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      CustomTabChanged?.Invoke(TabName, false);
    }
    #endregion
  }
}
