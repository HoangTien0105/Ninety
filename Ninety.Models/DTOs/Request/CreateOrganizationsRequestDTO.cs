using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Models.DTOs.Request
{
    public class CreateOrganizationsRequestDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
