using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class PartnerRole
{
    public int PartnerRoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<IsPartner> IsPartners { get; set; } = new List<IsPartner>();
}
