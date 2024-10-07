using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Models.DTOs.Request
{
    public class CreateMatchDTO
    {
        public int TeamA { get; set; }

        public int TeamB { get; set; }
        public DateTime? Date { get; set; }

        public int TournamentId { get; set; }
    }
}
