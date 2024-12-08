using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class IsPartner
{
    public int IsPartnerId { get; set; }

    public int EventId { get; set; }

    public int PartnerId { get; set; }

    public int PartnerRoleId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Partner Partner { get; set; } = null!;

    public virtual PartnerRole PartnerRole { get; set; } = null!;
}
