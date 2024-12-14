﻿using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Need
{
    public int NeedId { get; set; }

    public int RoleId { get; set; }

    public int EventId { get; set; }

    public int Quantity { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual EmployeeRole Role { get; set; } = null!;
}