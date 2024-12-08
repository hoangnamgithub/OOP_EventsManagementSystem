using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Account
{
    public int AccountId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int PermissionId { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual Permission Permission { get; set; } = null!;
}
