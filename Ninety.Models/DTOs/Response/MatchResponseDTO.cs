using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Models.DTOs.Response
{
    public class MatchResponseDTO
    {
        public int Id { get; set; }

        public int TeamA { get; set; }
        public string TeamAName { get; set; }   

        public int TeamB { get; set; }
        public string TeamBName { get; set; }

        public int WinningTeam { get; set; }

        public string TotalResult { get; set; } = null!;

        public string? Bracket { get; set; }

        public string? Round { get; set; }

        public DateTime? Date { get; set; }

        public int TournamentId { get; set; }
    }
}
