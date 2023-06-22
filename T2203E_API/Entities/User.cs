using System;
using System.Collections.Generic;

namespace T2203E_API.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? RoleTitle { get; set; }

    public string? JobTitle { get; set; }

    public DateTime? Birthday { get; set; }
}
