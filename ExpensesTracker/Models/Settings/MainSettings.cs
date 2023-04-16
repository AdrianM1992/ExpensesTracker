using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.Interfaces;
using System.Collections.Generic;

namespace ExpensesTracker.Models.Settings
{
  public class MainSettings : IMainSettings
  {
    private static MainSettings _mainSettingsInstance;
    Currencies _currency;
    Dictionary<Currencies, string> _currencies = new Dictionary<Currencies, string>
        {
          {Currencies.PLN, "pl-pl" },
          {Currencies.USD, "en-US" },
          {Currencies.EUR, "fr-FR" },
        };
    public string Currency { get => _currencies[_currency]; }

    private MainSettings()
    {
      _currency = Currencies.USD;
    }

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
