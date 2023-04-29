using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ExpensesTracker.Views.Controls
{
  /// <summary>
  /// Logic of interaction for CustomMinMaxCloseButtons.xaml
  /// </summary>
  public partial class CustomMinMaxCloseButtons : UserControl
  {
    #region Dependency properties
    public bool AllowMaximize
    {
      get { return (bool)GetValue(AllowMaximizeProperty); }
      set { SetValue(AllowMaximizeProperty, value); }
    }
    public static readonly DependencyProperty AllowMaximizeProperty =
        DependencyProperty.Register("AllowMaximize", typeof(bool), typeof(CustomMinMaxCloseButtons), new PropertyMetadata(true));


    public Brush HighlightBrush
    {
      get { return (Brush)GetValue(HighlightBrushProperty); }
      set { SetValue(HighlightBrushProperty, value); }
    }
    public static readonly DependencyProperty HighlightBrushProperty =
        DependencyProperty.Register("HighlightBrush", typeof(Brush), typeof(CustomMinMaxCloseButtons), new PropertyMetadata(Brushes.Transparent));
    #endregion

    public CustomMinMaxCloseButtons()
    {
      InitializeComponent();
    }

    #region Methods handling basic interaction events with window (min, max, close)
    private void Minimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      => Window.GetWindow(this).WindowState = WindowState.Minimized;

    private void Maximize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      //Some windows are meant to be fixed size, therefore maximizing is forbidden
      if (AllowMaximize)
      {
        Window parentWindow = Window.GetWindow(this);
        parentWindow.WindowState = parentWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
      }
    }

    private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => Window.GetWindow(this)?.Close();

    #endregion


    #region Highlighting interactions
    private void Border_MouseEnter(object sender, MouseEventArgs e)
    {
      //If maximizing is deactivated, highlight is also deactivated
      if (sender is Border borderToHighlight)
      {
        if (borderToHighlight.Name != "Max" || AllowMaximize) borderToHighlight.Background = HighlightBrush;
      }
    }
    private void Border_MouseLeave(object sender, MouseEventArgs e)
    {
      if (sender is Border borderToHighlight) borderToHighlight.Background = Brushes.Transparent;
    }
    #endregion
  }
}
