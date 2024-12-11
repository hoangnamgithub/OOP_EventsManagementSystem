using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Performer
{
    public int PerformerId { get; set; }

    public string FullName { get; set; } = null!;

    public string? ContactDetail { get; set; }

    public virtual ICollection<Show> Shows { get; set; } = new List<Show>();
}
