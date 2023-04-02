using System;
using System.Collections.Generic;

namespace ExpensesTracker.Models.DataProviders;

public partial class Category
{
    public short Id { get; set; }

    public bool Fixed { get; set; }

    public string Name { get; set; } = null!;
}
