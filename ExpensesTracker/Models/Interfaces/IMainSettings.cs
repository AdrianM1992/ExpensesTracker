using ExpensesTracker.DataTypes.Enums;
using System.Collections.Generic;

namespace ExpensesTracker.Models.Interfaces
{
  public interface IMainSettings
  {
    public string Currency { get; }
    public void SetDefaultCurrency(Currencies currency);
    public List<string> GetAvaliableCurrencies();
  }
}
