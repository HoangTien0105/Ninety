﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Models.PSSModels
{
    public class OrganizationParameter: QueryStringParameters
    {
        public OrganizationParameter()
        {
            OrderBy = "Name";
        }
        public string? Name { get; set; }
    }
}
