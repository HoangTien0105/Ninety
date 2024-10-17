using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Models.DTOs
{
    public class BadmintonMatchDetailDTO
    {
        public int Id { get; set; }

        public int ApointSet1 { get; set; }

        public int BpointSet1 { get; set; }

        public int ApointSet2 { get; set; }

        public int BpointSet2 { get; set; }

        public int? ApointSet3 { get; set; }

        public int? BpointSet3 { get; set; }

        //public int MatchId { get; set; }
        public MatchDTO Match { get; set; }
    }
}
