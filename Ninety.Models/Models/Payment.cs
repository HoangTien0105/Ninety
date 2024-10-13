using System;
using System.Collections.Generic;

namespace Ninety.Models.Models;

public partial class Payment
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public int PaymentStatus { get; set; }

    public DateTime? DateTime { get; set; }

    public string? Description { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
