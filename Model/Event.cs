using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Event
{
    public int EventId { get; set; }

    public string EventName { get; set; } = null!;

    public string? EventDescription { get; set; }

    public int ExptedAttendee { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int EventTypeId { get; set; }

    public int VenueId { get; set; }

    public virtual ICollection<Engaged> Engageds { get; set; } = new List<Engaged>();

    public virtual EventType EventType { get; set; } = null!;

    public virtual ICollection<IsSponsor> IsSponsors { get; set; } = new List<IsSponsor>();

    public virtual ICollection<Need> Needs { get; set; } = new List<Need>();

    public virtual ICollection<Required> Requireds { get; set; } = new List<Required>();

    public virtual ICollection<ShowSchedule> ShowSchedules { get; set; } = new List<ShowSchedule>();

    public virtual Venue Venue { get; set; } = null!;
}
