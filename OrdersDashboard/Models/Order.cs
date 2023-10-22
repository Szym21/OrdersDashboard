using System;

namespace OrdersDashboard.Models;

public partial class Order
{
    public int? IdZamowienia { get; set; }

    public int? IdKontrahenta { get; set; }

    public string? Numer { get; set; }

    public DateTime? DataDodania { get; set; }

    public virtual Contractor? IdKontrahentaNavigation { get; set; }
}
