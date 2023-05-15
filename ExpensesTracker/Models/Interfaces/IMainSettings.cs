using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Models.DataProviders;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExpensesTracker.Models.Interfaces
{
  public interface IMainSettings
  {
    public string Currency { get; }
    public void SetDefaultCurrency(Currencies currency);
    public List<string> GetAvailableCurrencies();
    public List<Expense> DatabaseRecords { get; }
    public event PropertyChangedEventHandler? PropertyChanged;
  }
}
