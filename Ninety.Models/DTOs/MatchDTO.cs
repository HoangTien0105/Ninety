using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Models.DTOs
{
    public class MatchDTO
    {
        public int Id { get; set; }

        public int TeamA { get; set; }

        public int TeamB { get; set; }

        public int WinningTeam { get; set; }

        public string TotalResult { get; set; } = null!;

        public string? Bracket { get; set; }

        public string? Round { get; set; }

        public DateTime? Date { get; set; }

        public int TournamentId { get; set; }
    }
}
