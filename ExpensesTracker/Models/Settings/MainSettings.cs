using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.DataControllers;
using ExpensesTracker.Models.DataProviders;
using ExpensesTracker.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExpensesTracker.Models.Settings
{
  public class MainSettings : IMainSettings, INotifyPropertyChanged
  {
    private static MainSettings? _mainSettingsInstance;
    private List<Expense> _databaseRecords = new List<Expense>();
    private Currencies _currency;
    private readonly Dictionary<Currencies, string> _currencies = new()
    {
          {Currencies.PLN, "pl-pl" },
          {Currencies.USD, "en-US" },
          {Currencies.EUR, "fr-FR" },
        };

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Currency { get => _currencies[_currency]; }

    public List<Expense> DatabaseRecords { get => _databaseRecords; }

    private MainSettings()
    {
      _currency = Currencies.USD;
      DatabaseModel.SubtablesChanged += DatabaseModel_DatabaseChanged;
      DatabaseModel.DataChanged += DatabaseModel_DatabaseChanged;
      _databaseRecords = DatabaseModel.FilterByRange(null, true, -1);
    }

    private void DatabaseModel_DatabaseChanged(object? sender, System.EventArgs e)
    {
      _databaseRecords = DatabaseModel.FilterByRange(null, true, -1);
      OnPropertyChanged(nameof(DatabaseRecords));
    }
    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public void SetDefaultCurrency(Currencies currency)
    {
      _currency = currency;
    }
    public List<string> GetAvaliableCurrencies()
    {
      var currencies = new List<string>();
      foreach (var currency in _currencies)
      {
        currencies.Add(currency.Key.ToString());
      }
      return currencies;
    }

    static public IMainSettings GetMainSettingsInstance()
    {
      if (_mainSettingsInstance == null)
      {
        _mainSettingsInstance = new MainSettings();
        return _mainSettingsInstance;
      }
      else
      {
        return _mainSettingsInstance;
      }
    }
  }
}
