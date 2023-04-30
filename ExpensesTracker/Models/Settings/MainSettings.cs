using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.Interfaces;
using System.Collections.Generic;

namespace ExpensesTracker.Models.Settings
{
  public class MainSettings : IMainSettings
  {
#pragma warning disable CS8618 // Pole niedopuszczające wartości null musi zawierać wartość inną niż null podczas kończenia działania konstruktora. Rozważ zadeklarowanie pola jako dopuszczającego wartość null.
    private static MainSettings _mainSettingsInstance;
#pragma warning restore CS8618 // Pole niedopuszczające wartości null musi zawierać wartość inną niż null podczas kończenia działania konstruktora. Rozważ zadeklarowanie pola jako dopuszczającego wartość null.
    Currencies _currency;
    readonly Dictionary<Currencies, string> _currencies = new()
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
