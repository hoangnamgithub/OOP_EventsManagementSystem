using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class EmployeeRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public decimal Salary { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Need> Needs { get; set; } = new List<Need>();
}
