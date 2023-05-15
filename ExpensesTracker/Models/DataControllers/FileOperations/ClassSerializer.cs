// Ignore Spelling: Serializer

using Microsoft.Win32;
using System;
using System.IO;
using System.Text.Json;

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

        return JsonSerializer.Deserialize(classString, classType);
      }
      else return null;
    }
  }
}
