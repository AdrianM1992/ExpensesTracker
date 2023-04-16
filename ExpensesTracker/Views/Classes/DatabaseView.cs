using System;

namespace ExpensesTracker.Views.Classes
{
  /// <summary>
  /// Class with properties meant to be displayed in DataGrid
  /// </summary>
  public class DatabaseView
  {
    public string Name { get; set; }

    public decimal Price { get; set; }

    public decimal Quantity { get; set; }

    public decimal? Total { get; set; }

    public DateTime DateOfEntry { get; set; }

    public DateTime LastUpdate { get; set; }

    public DateTime? Date { get; set; }

    public string Category { get; set; }

    public string? Subcategory { get; set; }

    public bool Income { get; set; }

    public bool Recurring { get; set; }

    public string? RecurringId { get; set; }

    public string? Description { get; set; }
  }
}
