using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Required
{
    public int RequiredId { get; set; }

    public int Quantity { get; set; }

    public int EventId { get; set; }

    public int EquipNameId { get; set; }

    public virtual EquipmentName EquipName { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;
}
