using System;
using System.Collections.Generic;

namespace Ninety.Models.Models;

public partial class Organization
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();

    public virtual User User { get; set; } = null!;
}
