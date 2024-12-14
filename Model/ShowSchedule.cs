using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class ShowSchedule
{
    public int ShowTimeId { get; set; }

    public DateOnly StartDate { get; set; }

    public int EstDuration { get; set; }

    public int ShowId { get; set; }

    public int EventId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Show Show { get; set; } = null!;
}
