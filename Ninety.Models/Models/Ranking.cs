using System;
using System.Collections.Generic;

namespace Ninety.Models.Models;

public partial class Ranking
{
    public int Id { get; set; }

    public int Point { get; set; }

    public int Rank { get; set; }

    public int TournamentId { get; set; }

    public int TeamId { get; set; }

    public virtual Team Team { get; set; } = null!;

    public virtual Tournament Tournament { get; set; } = null!;
}
