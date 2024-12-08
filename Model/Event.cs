using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Event
{
    public int EventId { get; set; }

    public string EventName { get; set; } = null!;

    public string? EventDescription { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int AccountId { get; set; }

    public int VenueId { get; set; }

    public int EventTypeId { get; set; }

    public int ExpectedAttendee { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Engaged> Engageds { get; set; } = new List<Engaged>();

    public virtual EventType EventType { get; set; } = null!;

    public virtual ICollection<IsPartner> IsPartners { get; set; } = new List<IsPartner>();

    public virtual ICollection<Required> Requireds { get; set; } = new List<Required>();

    public virtual ICollection<Show> Shows { get; set; } = new List<Show>();

    public virtual Venue Venue { get; set; } = null!;
}
