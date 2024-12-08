using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Engaged
{
    public int EngagedId { get; set; }

    public int EventId { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;
}
