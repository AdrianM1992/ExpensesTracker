using ExpensesTracker.ViewModels;
using ExpensesTracker.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExpensesTracker
{
  /// <summary>
  /// Logika interakcji dla klasy MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    MainWindowViewModel _viewModel;

    public MainWindow()
    {
      InitializeComponent();
      _viewModel = MainWindowViewModel.GetMainWindowViewModel(this);
      DataContext = _viewModel;
    }

    private void Menu_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      _viewModel.Set();
      MenuShowHide();
    }

    private void Bar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        DragMove();
      }
    }

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

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {

    }

    private void MenuHome_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SwapTab("Home");
      ContentPage.Source = new Uri("/Views/Pages/Home/HomePage.xaml", UriKind.Relative);
      MenuShowHide();
    }
    private void MenuGraph_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      SwapTab("Graphs");
      ContentPage.Source = new Uri("/Views/Pages/Graphs/DataGraphsPage.xaml", UriKind.Relative);
      MenuShowHide();
    }

    private void SwapTab(string tabName)
    {
      //Okementować to
      IEnumerable<CustomTabControl> customTabControls = Tabs.Children.OfType<CustomTabControl>();
      var match = customTabControls.Where(tab => tab.TabName == tabName).Select(tab => tab);
      foreach (var customTabControl in customTabControls)
      {
        customTabControl.BackgroundTabColor = SystemColors.ScrollBarBrush;
      }
      if (!match.Any())
      {
        var newTab = new CustomTabControl { TabName = tabName, VerticalAlignment = VerticalAlignment.Bottom, BackgroundTabColor = SystemColors.MenuBarBrush };
        Tabs.Children.Add(newTab);
      }
      match.First().BackgroundTabColor = SystemColors.MenuBarBrush;
    }
    private void MenuShowHide()
    {
      //okomentowac
      var menuColumn = Layout.ColumnDefinitions.First();
      Visibility textState;
      if (menuColumn.ActualWidth == 120)
      {
        textState = Visibility.Collapsed;
        menuColumn.Width = new GridLength(42);
      }
      else
      {
        textState = Visibility.Visible;
        menuColumn.Width = new GridLength(120);
      }

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
  }
}
