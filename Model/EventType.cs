﻿using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class EventType
{
    public int EventTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
