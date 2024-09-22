using System;
using System.Collections.Generic;

namespace Ninety.Models.Models;

public partial class BadmintonMatchDetail
{
    public int Id { get; set; }

    public int ApointSet1 { get; set; }

    public int BpointSet1 { get; set; }

    public int ApointSet2 { get; set; }

    public int BpointSet2 { get; set; }

    public int? ApointSet3 { get; set; }

    public int? BpointSet3 { get; set; }

    public int MatchId { get; set; }

    public virtual Match Match { get; set; } = null!;
}
