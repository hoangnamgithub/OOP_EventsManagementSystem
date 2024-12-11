using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class IsSponsor
{
    public int IsSponsorId { get; set; }

    public int EventId { get; set; }

    public int SponsorId { get; set; }

    public int SponsorTierId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Sponsor Sponsor { get; set; } = null!;

    public virtual SponsorTier SponsorTier { get; set; } = null!;
}
