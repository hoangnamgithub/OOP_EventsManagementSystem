using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OOP_EventsManagementSystem.Model;

public partial class Show
{
    public int ShowId { get; set; }

    public string ShowName { get; set; } = null!;

    public decimal Cost { get; set; }

    public int PerformerId { get; set; }

    public int GenreId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Performer Performer { get; set; } = null!;

    public virtual ICollection<ShowSchedule> ShowSchedules { get; set; } = new List<ShowSchedule>();

    [NotMapped]
    private bool _isChecked;
    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            _isChecked = value;
            OnPropertyChanged(nameof(IsChecked));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
