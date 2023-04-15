using ExpensesTracker.Views.Classes;

namespace ExpensesTracker.ViewModels
{
  class AddEditDBWindowViewModel
  {
    public DatabaseView databaseView;

    public AddEditDBWindowViewModel()
    {
      databaseView = new DatabaseView();
    }
  }
}
