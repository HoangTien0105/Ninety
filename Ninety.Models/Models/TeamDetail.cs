using System;
using System.Collections.Generic;

namespace Ninety.Models.Models;

public partial class TeamDetail
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TeamId { get; set; }

    public virtual Team Team { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
