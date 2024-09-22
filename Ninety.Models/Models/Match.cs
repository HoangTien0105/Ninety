using System;
using System.Collections.Generic;

namespace Ninety.Models.Models;

public partial class Match
{
    public int Id { get; set; }

    public int TeamA { get; set; }

    public int TeamB { get; set; }

    public int WinningTeam { get; set; }

    public string TotalResult { get; set; } = null!;

    public DateTime? Date { get; set; }

    public int TournamentId { get; set; }

    public virtual ICollection<BadmintonMatchDetail> BadmintonMatchDetails { get; set; } = new List<BadmintonMatchDetail>();

    public virtual Tournament Tournament { get; set; } = null!;
}
