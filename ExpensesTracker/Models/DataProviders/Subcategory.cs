using System;
using System.Collections.Generic;

namespace ExpensesTracker.Models.DataProviders;

public partial class Subcategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }
}
