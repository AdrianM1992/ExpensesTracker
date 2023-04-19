using ExpensesTracker.Models.DataProviders;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExpensesTracker.Models.DataControllers
{
  public static class DatabaseModel
  {
    public static void AddEditDBRecord(Expense expense, bool editMode)
    {
      using var db = new ExpensesContext();
      if (editMode) db.Entry(expense).State = EntityState.Modified;
      else db.Add(expense);
      db.SaveChanges();
    }
    public static void DeleteDBRecord(Expense expense)
    {
      using var db = new ExpensesContext();
      if (expense != null)
      {
        var records = from e in db.Expenses.ToList()
                      where e.Id == expense.Id
                      select e;

        Expense record = records.First();
        db.Remove(record);
      }
      db.SaveChanges();
    }
  }
}
