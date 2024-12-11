using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class SponsorTier
{
    public int SponsorTierId { get; set; }

    public string TierName { get; set; } = null!;

    public virtual ICollection<IsSponsor> IsSponsors { get; set; } = new List<IsSponsor>();
}
