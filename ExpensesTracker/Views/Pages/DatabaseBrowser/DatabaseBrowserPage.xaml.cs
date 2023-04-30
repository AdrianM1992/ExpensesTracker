﻿using ExpensesTracker.Models.Interfaces;
using ExpensesTracker.ViewModels;
using ExpensesTracker.Views.Classes;
using ExpensesTracker.Views.Windows.AddEditDB;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ExpensesTracker.Views.Pages.DatabaseBrowser
{
  /// <summary>
  /// Logic for DatabaseBrowserPage.xaml
  /// </summary>
  public partial class DatabaseBrowserPage : Page, IObserver<double>
  {
    private readonly DatabaseBrowserPageViewModel _viewModel;
    private AddEditDBWindow? addEditDBWindow;
    private readonly IMainSettings _mainSettings;
    private bool _deleteInfoNotShown = true;
    private IDisposable? _unsubscription;

    public DatabaseBrowserPage(IMainSettings mainSettings)
    {
      _mainSettings = mainSettings;
      InitializeComponent();
      _viewModel = DatabaseBrowserPageViewModel.GetDatabaseBrowserPageViewModelRef(this);
      DatabaseView.DataContext = _viewModel;
      DatabaseView.ItemsSource = _viewModel.DatabaseViewItems;
    }

    private void OpenAddEditWindow(bool editMode)
    {
      if (addEditDBWindow == null || !addEditDBWindow.IsLoaded)
      {
        if (!editMode) addEditDBWindow = new AddEditDBWindow(_mainSettings, editMode: editMode);
        else addEditDBWindow = new AddEditDBWindow(_mainSettings, (DatabaseView)DatabaseView.SelectedItem);
        addEditDBWindow.AddListenerToAddEditRecordEvent(OnAddEditHandler);
      }
      else
      {
        var action = MessageBox.Show("Another window is already opened.\n\nDo you wish to open new one?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (action == MessageBoxResult.Yes)
        {
          addEditDBWindow.Close();
          if (!editMode) addEditDBWindow = new AddEditDBWindow(_mainSettings, editMode: editMode);
          else addEditDBWindow = new AddEditDBWindow(_mainSettings, (DatabaseView)DatabaseView.SelectedItem);
          addEditDBWindow.AddListenerToAddEditRecordEvent(OnAddEditHandler);
        }
        else addEditDBWindow.Focus();
      }
      addEditDBWindow.Show();
    }

    private void OnAddEditHandler() => _viewModel.AddedRecord();

    /// <summary>
    /// Opens new AddEditDBWindow if reference is null or window is not loaded.
    /// </summary>
    private void AddEditButton_Click(object sender, RoutedEventArgs e)
    {
      var button = (Button)sender;
      OpenAddEditWindow(button.Name == "EditButton");
    }
    private void DatabaseView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) => OpenAddEditWindow(true);
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      if (DatabaseView.Items.Count <= 0) MessageBox.Show("Nothing to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
      else
      {
        if (DatabaseView.SelectedIndex == -1 && _deleteInfoNotShown)
        {
          _deleteInfoNotShown = false;
          MessageBoxResult result = MessageBox.Show("One time message.\n If no row is selected, first element will be deleted.\nContinue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Information);

          if (result == MessageBoxResult.No) return;
          DatabaseView.SelectedIndex = 0;
        }
        else if (DatabaseView.SelectedIndex == -1) DatabaseView.SelectedIndex = 0;

        _viewModel.RemoveRecord((DatabaseView)DatabaseView.SelectedItem);
      }
    }
    private void ShowMoreButton_Click(object sender, RoutedEventArgs e) => _viewModel.LoadMoreRecords();

    public void Subscribe(MainWindow mainWindow) => _unsubscription = mainWindow.Subscribe(this);
    public void Unsubscribe() => _unsubscription.Dispose();

    public void OnCompleted()
    {
      //No implementation
    }
    public void OnError(Exception error)
    {
      //No implementation
    }
    public void OnNext(double value)
    {
      double maxWidth = value - 210;
      foreach (var column in DatabaseView.Columns)
      {
        if (column != DescriptionColumn) maxWidth -= column.ActualWidth;
      }
      if (maxWidth < 300) DescriptionColumn.MaxWidth = 300;
      else DescriptionColumn.MaxWidth = maxWidth;
    }
  }
}