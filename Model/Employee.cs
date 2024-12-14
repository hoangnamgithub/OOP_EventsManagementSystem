using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Contact { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();

    public virtual EmployeeRole Role { get; set; } = null!;
}
