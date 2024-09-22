using System;
using System.Collections.Generic;

namespace Ninety.Models.Models;

public partial class Team
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int TournamentId { get; set; }

    public virtual ICollection<TeamDetail> TeamDetails { get; set; } = new List<TeamDetail>();

    public virtual Tournament Tournament { get; set; } = null!;
}
