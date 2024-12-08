using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class ShowSchedule
{
    public int ShowTimeId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EstDuration { get; set; }

    public DateOnly Datetime { get; set; }

    public int ShowId { get; set; }

    public virtual Show Show { get; set; } = null!;
}
