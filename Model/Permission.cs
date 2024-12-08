using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Permission
{
    public int PermissionId { get; set; }

    public string Permission1 { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
