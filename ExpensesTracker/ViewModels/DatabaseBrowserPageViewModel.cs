using ExpensesTracker.Models.DataProviders;
using System.Collections.Generic;
using System.Linq;

namespace ExpensesTracker.ViewModels
{
  class DatabaseBrowserPageViewModel
  {
    public List<string> Name
    {
      get
      {
        using (var db = new ExpensesContext())
        {
          var names = db.Expenses.ToList().Select(x => x.Name).ToList();
          Name = names;
        }
        return Name;
      }
      set
      {
        using (var db = new ExpensesContext())
        {
          var names = db.Expenses.ToList().Select(x => x.Name).ToList();
          Name = names;
        }
      }
    }
  }
}
