using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Views.Windows.NewElementWindowInput;
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
    /// Specifies ability to change header
    /// </summary>
    public bool IsHeaderEditable
    {
      get { return (bool)GetValue(IsHeaderEditableProperty); }
      set { SetValue(IsHeaderEditableProperty, value); }
    }
    public static readonly DependencyProperty IsHeaderEditableProperty =
        DependencyProperty.Register("IsHeaderEditable", typeof(bool), typeof(CustomTabControl), new PropertyMetadata(false));


    /// <summary>
    /// Custom event that informs main window to take specific actions
    /// </summary>
    public event CustomTabEventHandler? CustomTabEvent;

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

    #region Click events
    private void Button_Click(object sender, RoutedEventArgs e)
    {
      CustomTabEvent?.Invoke(TabName, CustomTabEnums.Closed);
    }

    private void Description_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      CustomTabEvent?.Invoke(TabName, CustomTabEnums.Clicked);
    }

    private void Description_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (IsHeaderEditable)
      {
        var dialog = new NewElementWindow { Header = "Name the tab:" };
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
          var oldName = TabName;
          TabName = dialog.NewElementName;
          CustomTabEvent?.Invoke(TabName, CustomTabEnums.NameChanged, oldName);
        }
        dialog.Close();
      }
    }
    #endregion
  }
}
