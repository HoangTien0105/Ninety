using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ninety.Models.DTOs
{
    public class TeamDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        [JsonIgnore]
        public int TournamentId { get; set; }

        public TournamentDTO Tournament { get; set; }
    }
}
