using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.Models.Settings;
using ExpensesTracker.ViewModels;
using ExpensesTracker.Views.Controls;
using ExpensesTracker.Views.Pages.DatabaseBrowser;
using ExpensesTracker.Views.Pages.Graphs;
using ExpensesTracker.Views.Pages.Home;
using ExpensesTracker.Views.Pages.Settings;
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
    private readonly IMainSettings _mainSettings;
    MainWindowViewModel _viewModel;
    /// <summary>
    /// Stores instances of pages [value] by tab name [key]
    /// </summary>
    readonly Dictionary<string, Page> _pages = new();
    /// <summary>
    /// Stores names of all pages
    /// </summary>
    readonly List<string> _tabNames = new();

    public MainWindow()
    {
      InitializeComponent();
      _mainSettings = MainSettings.GetMainSettingsInstance();
      _viewModel = MainWindowViewModel.GetMainWindowViewModel();
      DataContext = _viewModel;
      HomeTab.CustomTabChanged += OnTabChangedHandler;
      InitTabs();
      ContentPage.Content = _pages[_tabNames[0]];
    }

    private void Menu_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      MenuShowHide();
    }

    private void Bar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed) DragMove();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      foreach (Window window in Application.Current.Windows)
      {
        if (window != this) window.Close();
      }
    }

    #region Menu Items
    //Methods handling interaction with menu items
    private void MenuHome_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SwapTab(_tabNames[0]);
      ContentPage.Navigate(_pages[_tabNames[0]]);
    }
    private void Database_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SwapTab(_tabNames[1]);
      ContentPage.Navigate(_pages[_tabNames[1]]);
    }
    private void MenuGraph_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SwapTab(_tabNames[2]);
      ContentPage.Navigate(_pages[_tabNames[2]]);
    }
    private void Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SwapTab(_tabNames[3]);
      ContentPage.Navigate(_pages[_tabNames[3]]);
    }
    #endregion

    /// <summary>
    /// Initializes _tabNames and _pages variables
    /// </summary>
    private void InitTabs()
    {
      _tabNames.Add("Home");
      _pages.Add(_tabNames.Last(), new HomePage());
      _tabNames.Add("Database");
      _pages.Add(_tabNames.Last(), new DatabaseBrowserPage(_mainSettings));
      _tabNames.Add("Graphs");
      _pages.Add(_tabNames.Last(), new GraphsPage());
      _tabNames.Add("Settings");
      _pages.Add(_tabNames.Last(), new SettingsPage());
    }

    /// <summary>
    /// Swaps active page or create new if not already opened.
    /// </summary>
    /// <param name="tabName">Name of tab to add or swap to</param>
    /// <returns>Reference to created or swapped tab</returns>
    private CustomTabControl SwapTab(string tabName)
    {
      //Gets references to all opened tabs, then checks if desired tab is already opened
      IEnumerable<CustomTabControl> customTabControls = Tabs.Children.OfType<CustomTabControl>();
      var match = customTabControls.Where(tab => tab.TabName == tabName).Select(tab => tab);
      //Grays out all opened tabs
      foreach (var customTabControl in customTabControls)
      {
        customTabControl.BackgroundTabColor = SystemColors.ScrollBarBrush;
      }
      //If tab is already opened highlights it, if not creates new tab
      if (!match.Any())
      {
        var newTab = new CustomTabControl { TabName = tabName, VerticalAlignment = VerticalAlignment.Bottom, BackgroundTabColor = SystemColors.MenuBarBrush };
        newTab.CustomTabChanged += OnTabChangedHandler;
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
    private void OnTabChangedHandler(string tabName, bool tabClose)
    {
      //If tab is marked for closing, set 'Home' tab as active tab
      if (tabClose)
      {
        Tabs.Children.Remove(SwapTab(tabName));
        SwapTab(_tabNames[0]);
        ContentPage.Navigate(_pages[_tabNames[0]]);
      }
      else
      {
        SwapTab(tabName);
        ContentPage.Navigate(_pages[tabName]);
      }
    }
  }
}
