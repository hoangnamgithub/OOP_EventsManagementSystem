using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Sponsor
{
    public int SponsorId { get; set; }

    public string SponsorName { get; set; } = null!;

    public string? SponsorDetails { get; set; }

    public virtual ICollection<IsSponsor> IsSponsors { get; set; } = new List<IsSponsor>();
}
