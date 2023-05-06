using ExpensesTracker.DataTypes.Enums;

namespace ExpensesTracker.Views.Delegates
{
  public class ViewDelegates
  {
    public delegate void CustomTabEventHandler(string tabName, CustomTabEnums customTabEvent, string oldTabName = "");
    public delegate void AddEditRecordHandler();
  }
}
