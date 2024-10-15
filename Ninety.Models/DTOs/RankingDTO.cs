using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Models.DTOs
{
    public class RankingDTO
    {
        public int Id { get; set; }

        public int Point { get; set; }

        public int Rank { get; set; }

        public int TournamentId { get; set; }

        public int TeamId { get; set; }

        public TeamDTO Team { get; set; }
    }
}
