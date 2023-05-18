using ExpensesTracker.Models.Settings;

namespace ExpensesTracker.Views.Interfaces
{
  public interface IFilterSettings
  {
    public void SetDefaultValues();
    public void SetFilterSettingsRef(FilterSettings filterSettings);
    public void SetExistingFilterSettingsRef(FilterSettings filterSettings);
    public void ClearAll();
  }
}
