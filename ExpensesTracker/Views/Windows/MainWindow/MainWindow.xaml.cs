using ExpensesTracker.ViewModels;
using ExpensesTracker.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExpensesTracker
{
  /// <summary>
  /// Logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    MainWindowViewModel _viewModel;
    /// <summary>
    /// Stores locations [value] to pages [key]
    /// </summary>
    readonly Dictionary<string, Uri> _pages = new Dictionary<string, Uri>();
    /// <summary>
    /// Stores names of all pages
    /// </summary>
    readonly List<string> _tabNames = new List<string>();

    public MainWindow()
    {
      InitializeComponent();

      _viewModel = MainWindowViewModel.GetMainWindowViewModel(this);
      DataContext = _viewModel;
      HomeTab.CustomTabChanged += TabChanged;
      InitTabs();
    }

    private void Menu_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      MenuShowHide();
    }

    private void Bar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        DragMove();
      }
    }

    #region Window Buttons
    //Methods handling basic interaction events with window (min, max, close)
    private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
    {
      Close();
    }
    private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
    {
      WindowState = WindowState.Minimized;
    }
    private void Maximize_MouseDown(object sender, MouseButtonEventArgs e)
    {
      WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }
    #endregion

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
    }

    #region Menu Items
    //Methods handling interaction with menu items
    private void MenuHome_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SwapTab(_tabNames[0]);
      ContentPage.Source = _pages[_tabNames[0]];
    }
    private void Database_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SwapTab(_tabNames[1]);
      ContentPage.Source = _pages[_tabNames[1]];
    }
    private void MenuGraph_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SwapTab(_tabNames[2]);
      ContentPage.Source = _pages[_tabNames[2]];
    }
    private void Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SwapTab(_tabNames[3]);
      ContentPage.Source = _pages[_tabNames[3]];
    }
    #endregion

    /// <summary>
    /// Initializes _tabNames and _pages variables
    /// </summary>
    private void InitTabs()
    {
      _tabNames.Add("Home");
      _pages.Add(_tabNames[0], new Uri("/Views/Pages/Home/HomePage.xaml", UriKind.Relative));
      _tabNames.Add("Database");
      _pages.Add(_tabNames[1], new Uri("/Views/Pages/DatabaseBrowser/DatabaseBrowserPage.xaml", UriKind.Relative));
      _tabNames.Add("Graphs");
      _pages.Add(_tabNames[2], new Uri("/Views/Pages/Graphs/DataGraphsPage.xaml", UriKind.Relative));
      _tabNames.Add("Settings");
      _pages.Add(_tabNames[3], new Uri("/Views/Pages/Settings/Settings.xaml", UriKind.Relative));
    }

    /// <summary>
    /// Swaps active page or create new if not already opened.
    /// </summary>
    /// <param name="tabName">Name of tab to add or swap to</param>
    /// <returns>Reference to created or swapped tab</returns>
    private CustomTabControl SwapTab(string tabName)
    {
      //Gets refferences to all opened tabs, then checks if desired tab is already opened
      IEnumerable<CustomTabControl> customTabControls = Tabs.Children.OfType<CustomTabControl>();
      var match = customTabControls.Where(tab => tab.TabName == tabName).Select(tab => tab);
      //Grays out all opened tabs
      foreach (var customTabControl in customTabControls)
      {
        customTabControl.BackgroundTabColor = SystemColors.ScrollBarBrush;
      }
      //If tab is already opened highlites it, if not creates new tab
      if (!match.Any())
      {
        var newTab = new CustomTabControl { TabName = tabName, VerticalAlignment = VerticalAlignment.Bottom, BackgroundTabColor = SystemColors.MenuBarBrush };
        newTab.CustomTabChanged += TabChanged;
        Tabs.Children.Add(newTab);
        return newTab;
      }
      else
      {
        match.First().BackgroundTabColor = SystemColors.MenuBarBrush;
        return match.First();
      }
    }

    /// <summary>
    /// Handles behavior of wrapped menu
    /// </summary>
    /// <param name="menuIcon">Specifies if caller was menu icon </param>
    private void MenuShowHide()
    {
      var menuColumn = Layout.ColumnDefinitions.First();

      Visibility textState = Visibility.Collapsed;
      //If menu is unwrapped, wraps it. If not, if caller is menu icon unwraps menu
      if (menuColumn.ActualWidth == 120)
      {
        Task.Run(() =>
        {
          for (int i = 120; i >= 42; i--)
          {
            this.Dispatcher.Invoke((Action)delegate { menuColumn.Width = new GridLength(i); });
            Task.Delay(3).Wait();
          }
        });
      }
      else
      {
        textState = Visibility.Visible;
        Task.Run(() =>
        {
          for (int i = 42; i <= 120; i++)
          {
            this.Dispatcher.Invoke((Action)delegate { menuColumn.Width = new GridLength(i); });
            Task.Delay(3).Wait();
          }
        });
      }
      //Specifies visibility of descriptions of menu items
      foreach (StackPanel stackPanel in MenuSP.Children)
      {
        foreach (var control in stackPanel.Children)
        {
          if (control is TextBlock textBlock)
          {
            textBlock.Visibility = textState;
          }
        }
      }
    }

    /// <summary>
    /// Event handler for CustomTabControl
    /// </summary>
    /// <param name="tabName">Name of calling tab</param>
    /// <param name="tabClose">Specifies if tab is marked for closing</param>
    private void TabChanged(string tabName, bool tabClose)
    {
      //If tab is marked for closing, set 'Home' tab as active tab
      if (tabClose)
      {
        Tabs.Children.Remove(SwapTab(tabName));
        SwapTab(_tabNames[0]);
        ContentPage.Source = _pages[_tabNames[0]];
      }
      else
      {
        SwapTab(tabName);
        ContentPage.Source = _pages[tabName];
      }
    }
  }
}
