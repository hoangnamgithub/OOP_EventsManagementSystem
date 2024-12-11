using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Venue
{
    public int VenueId { get; set; }

    public string VenueName { get; set; } = null!;

    public decimal Cost { get; set; }

    public string? Address { get; set; }

    public int Capacity { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
