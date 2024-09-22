using System;
using System.Collections.Generic;

namespace Ninety.Models.Models;

public partial class User
{
    public int Id { get; set; }

    public int Role { get; set; }

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Nationality { get; set; } = null!;

    public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();

    public virtual ICollection<TeamDetail> TeamDetails { get; set; } = new List<TeamDetail>();
}
