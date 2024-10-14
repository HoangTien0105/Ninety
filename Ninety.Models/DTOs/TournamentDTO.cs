using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Models.DTOs
{
    public class TournamentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public string Format { get; set; } = null!;
        public int NumOfParticipants { get; set; }
<<<<<<< HEAD
=======

        public int? SlotLeft { get; set; }

>>>>>>> ad0d12b23f47b422ddc1fdfcdb854cc5bd3a380c
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Fee { get; set; }
<<<<<<< HEAD
        public string Place { get; set; } = null!;
        public UserDTO User { get; set; }
        public SportDTO Sport { get; set; }
=======

        public bool? IsRegister { get; set; }

        public bool? CreateMatch { get; set; }

        public string Place { get; set; } = null!;

        public int SportId { get; set; }

        public int UserId { get; set; }
>>>>>>> ad0d12b23f47b422ddc1fdfcdb854cc5bd3a380c
    }
}
