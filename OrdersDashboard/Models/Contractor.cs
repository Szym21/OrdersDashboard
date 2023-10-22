using System;
using System.Collections.Generic;

namespace OrdersDashboard.Models;

public partial class Contractor
{
    public int? IdKontrahenta { get; set; }

    public string? Nazwa { get; set; }

    public string? Nip { get; set; }

    public DateTime? DataDodania { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
