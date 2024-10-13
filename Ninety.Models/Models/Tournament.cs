using System;
using System.Collections.Generic;

namespace Ninety.Models.Models;

public partial class Tournament
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Rules { get; set; }

    public string Format { get; set; } = null!;

    public int NumOfParticipants { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public double Fee { get; set; }

    public string Place { get; set; } = null!;

    public int SportId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<Ranking> Rankings { get; set; } = new List<Ranking>();

    public virtual Sport Sport { get; set; } = null!;

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

    public virtual User User { get; set; } = null!;
}
