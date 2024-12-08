using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Partner
{
    public int PartnerId { get; set; }

    public string PartnerName { get; set; } = null!;

    public string? PartnerDetails { get; set; }

    public virtual ICollection<IsPartner> IsPartners { get; set; } = new List<IsPartner>();
}
