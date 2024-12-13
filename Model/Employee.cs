using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Contact { get; set; }

    public int RoleId { get; set; }

    public int? ManagerId { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

    public virtual Employee? Manager { get; set; }

    public virtual EmployeeRole Role { get; set; } = null!;
}
