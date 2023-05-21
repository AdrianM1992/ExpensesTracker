// Ignore Spelling: Serializer

using Microsoft.Win32;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace ExpensesTracker.Models.DataControllers.FileOperations
{
  public static class ClassSerializer
  {
    public static void SaveClass(object classToSave)
    {
      var saveFileDialog = new SaveFileDialog();
      string? location = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
      saveFileDialog.InitialDirectory = location ?? "";
      saveFileDialog.Filter = "Settings Files (*.dat)|*.dat|All Files (*.*)|*.*";
      saveFileDialog.FilterIndex = 0;
      saveFileDialog.RestoreDirectory = true;

      bool? result = saveFileDialog.ShowDialog();
      if (result == true)
      {
        string filePath = saveFileDialog.FileName;

        using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write);
        using StreamWriter sw = new(fs);
        string classString = JsonSerializer.Serialize(classToSave);
        sw.Write(classString);
      }
    }
    public static object? LoadClassFromFile(Type classType)
    {
      var loadFileDialog = new OpenFileDialog();
      string? location = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
      loadFileDialog.InitialDirectory = location ?? "";
      loadFileDialog.Filter = "Settings Files (*.dat)|*.dat|All Files (*.*)|*.*";
      loadFileDialog.FilterIndex = 0;
      loadFileDialog.RestoreDirectory = true;

      bool? result = loadFileDialog.ShowDialog();
      if (result == true)
      {
        string filePath = loadFileDialog.FileName;

        using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read);
        using StreamReader sr = new(fs);
        string classString = sr.ReadToEnd();

        object? classInstance = null;

        try
        {
          classInstance = JsonSerializer.Deserialize(classString, classType);
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Unable to load the class instance.\n\nError details:\n\n{ex.Message}\n\nTrying to create blank instance.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          try
          {
            classInstance = Activator.CreateInstance(classType);
          }
          catch (Exception ex2)
          {
            MessageBox.Show($"Unable to create blank instance.\nError details:\n{ex2.Message}\n", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          }
        }
        return classInstance;
      }
      else return null;
    }
    public static object? CopyClass(object? classToCopy, Type classType)
    {
      object? classInstance = null;
      if (classToCopy != null)
      {
        try
        {
          string classString = JsonSerializer.Serialize(classToCopy);
          classInstance = JsonSerializer.Deserialize(classString, classType);
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Unable to copy class instance.\n\nError details:\n\n{ex.Message}\n\nTrying to create blank instance.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          try
          {
            classInstance = Activator.CreateInstance(classType);
          }
          catch (Exception ex2)
          {
            MessageBox.Show($"Unable to create blank instance.\nError details:\n{ex2.Message}\n", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          }
        }
      }

      return classInstance;
    }
  }
}
