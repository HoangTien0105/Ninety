﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Models.DTOs.Request
{
    public class CreateTournamentRequestDTO
    {
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
    }
}
