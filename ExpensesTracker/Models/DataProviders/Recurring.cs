using System;
using System.Collections.Generic;

namespace ExpensesTracker.Models.DataProviders;

public partial class Recurring
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;
}
