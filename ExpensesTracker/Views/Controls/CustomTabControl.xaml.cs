using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Logika interakcji dla klasy CustomTabControl.xaml
  /// </summary>
  public partial class CustomTabControl : UserControl
  {
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

    public CustomTabControl()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      //zrobić obsługę tego przycisku
      throw new NotImplementedException();
    }
  }
}
