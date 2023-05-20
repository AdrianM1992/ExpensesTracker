namespace ExpensesTracker.Views.Interfaces
{
  public interface ISettingsSetter
  {
    public void SetDefaultValues();
    public void SetNewSettingsRef(object settingsRef);
    public void SetExistingSettingsRef(object settingsRef);
    public void ClearAll();
  }
}
