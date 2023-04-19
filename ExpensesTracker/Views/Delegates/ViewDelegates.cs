namespace ExpensesTracker.Views.Delegates
{
  public class ViewDelegates
  {
    public delegate void CustomTabEventHandler(string tabName, bool closeTab);
    public delegate void AddEditRecordHandler();
  }
}
