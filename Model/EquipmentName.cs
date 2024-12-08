using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class EquipmentName
{
    public int EquipNameId { get; set; }

    public string EquipName { get; set; } = null!;

    public decimal EquipCost { get; set; }

    public int EquipTypeId { get; set; }

    public virtual EquipmentType EquipType { get; set; } = null!;

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    public virtual ICollection<Required> Requireds { get; set; } = new List<Required>();
}
