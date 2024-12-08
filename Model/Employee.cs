using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Engaged> Engageds { get; set; } = new List<Engaged>();

    public virtual EmployeeRole Role { get; set; } = null!;
}
