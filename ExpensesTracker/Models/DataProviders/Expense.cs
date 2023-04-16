using System;
using System.Collections.Generic;

namespace ExpensesTracker.Models.DataProviders;

public partial class Expense
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Quantity { get; set; }

    public decimal? Total { get; set; }

    public DateTime DateOfEntry { get; set; }

    public DateTime LastUpdate { get; set; }

    public DateTime? Date { get; set; }

    public short CategoryId { get; set; }

    public int? SubcategoryId { get; set; }

    public bool Income { get; set; }

    public bool Recurring { get; set; }

    public short? RecurringId { get; set; }

    public string? Description { get; set; }

    public string? Files { get; set; }
}
