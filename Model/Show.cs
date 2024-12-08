using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Show
{
    public int ShowId { get; set; }

    public string ShowName { get; set; } = null!;

    public int PerformerId { get; set; }

    public int GenreId { get; set; }

    public int EventId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;

    public virtual Performer Performer { get; set; } = null!;

    public virtual ICollection<ShowSchedule> ShowSchedules { get; set; } = new List<ShowSchedule>();
}
