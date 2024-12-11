using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class EquipmentType
{
    public int EquipTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<EquipmentName> EquipmentNames { get; set; } = new List<EquipmentName>();
}
