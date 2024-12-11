using System;
using System.Collections.Generic;

namespace OOP_EventsManagementSystem.Model;

public partial class Genre
{
    public int GenreId { get; set; }

    public string Genre1 { get; set; } = null!;

    public virtual ICollection<Show> Shows { get; set; } = new List<Show>();
}
