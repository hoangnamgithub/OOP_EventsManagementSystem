using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public int EquipNameId { get; set; }

    public string? Condition { get; set; }

    public virtual EquipmentName EquipName { get; set; } = null!;
}
