using ExpensesTracker.DataTypes.Enums;
using ExpensesTracker.Views.Controls;

namespace ExpensesTracker.Views.Delegates
{
  public class ViewDelegates
  {
    public delegate void CustomTabEventHandler(string tabName, CustomTabEnums customTabEvent, string oldTabName = "");
    public delegate void AddEditRecordHandler();
    public delegate void ModifyContainers(CustomTabControl? grpahTab, GraphTabActions? tabAction, GraphControl? graph, string? newCurrentTabName);
  }
}
