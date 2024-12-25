﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;
using OOP_EventsManagementSystem.Model;

namespace OOP_EventsManagementSystem.ViewModel
{
    class HomeVM : INotifyPropertyChanged
    {
        private readonly EventManagementDbContext _context;

        public HomeVM()
        {
            _context = new EventManagementDbContext();
            LoadChartData();
        }

        private SeriesCollection _equipmentQuantityByEvent;
        public SeriesCollection EquipmentQuantityByEvent
        {
            get => _equipmentQuantityByEvent;
            set
            {
                _equipmentQuantityByEvent = value;
                OnPropertyChanged(nameof(EquipmentQuantityByEvent));
            }
        }

        private SeriesCollection _eventsByType;
        public SeriesCollection EventsByType
        {
            get => _eventsByType;
            set
            {
                _eventsByType = value;
                OnPropertyChanged(nameof(EventsByType));
            }
        }

        private List<string> _eventTypeLabels;
        public List<string> EventTypeLabels
        {
            get => _eventTypeLabels;
            set
            {
                _eventTypeLabels = value;
                OnPropertyChanged(nameof(EventTypeLabels));
            }
        }

        private SeriesCollection _attendeesDistribution;
        public SeriesCollection AttendeesDistribution
        {
            get => _attendeesDistribution;
            set
            {
                _attendeesDistribution = value;
                OnPropertyChanged(nameof(AttendeesDistribution));
            }
        }

        private List<int> _attendeeBins;
        public List<int> AttendeeBins
        {
            get => _attendeeBins;
            set
            {
                _attendeeBins = value;
                OnPropertyChanged(nameof(AttendeeBins));
            }
        }

        private SeriesCollection _eventsByVenue;
        public SeriesCollection EventsByVenue
        {
            get => _eventsByVenue;
            set
            {
                _eventsByVenue = value;
                OnPropertyChanged(nameof(EventsByVenue));
            }
        }

        private List<string> _venueLabels;
        public List<string> VenueLabels
        {
            get => _venueLabels;
            set
            {
                _venueLabels = value;
                OnPropertyChanged(nameof(VenueLabels));
            }
        }
        private SeriesCollection _heatMapData;
        public SeriesCollection HeatMapData
        {
            get => _heatMapData;
            set
            {
                _heatMapData = value;
                OnPropertyChanged(nameof(HeatMapData));
            }
        }

        private List<string> _eventLabels;
        public List<string> EventLabels
        {
            get => _eventLabels;
            set
            {
                _eventLabels = value;
                OnPropertyChanged(nameof(EventLabels));
            }
        }

        private List<string> _equipmentLabels;
        public List<string> EquipmentLabels
        {
            get => _equipmentLabels;
            set
            {
                _equipmentLabels = value;
                OnPropertyChanged(nameof(EquipmentLabels));
            }
        }

        private SeriesCollection _equipmentByTypeAcrossEvents;
        public SeriesCollection EquipmentByTypeAcrossEvents
        {
            get => _equipmentByTypeAcrossEvents;
            set
            {
                _equipmentByTypeAcrossEvents = value;
                OnPropertyChanged(nameof(EquipmentByTypeAcrossEvents));
            }
        }

        private SeriesCollection _equipmentCostByType;
        public SeriesCollection EquipmentCostByType
        {
            get => _equipmentCostByType;
            set
            {
                _equipmentCostByType = value;
                OnPropertyChanged(nameof(EquipmentCostByType));
            }
        }

        private List<string> _equipmentTypeLabels;
        public List<string> EquipmentTypeLabels
        {
            get => _equipmentTypeLabels;
            set
            {
                _equipmentTypeLabels = value;
                OnPropertyChanged(nameof(EquipmentTypeLabels));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        private void LoadChartData()
        {
            var eventsByTypeData = _context
                .EventTypes.Select(et => new
                {
                    TypeName = et.TypeName,
                    EventCount = et.Events.Count,
                })
                .ToList();

            EventTypeLabels = eventsByTypeData.Select(d => d.TypeName).ToList();
            EventsByType = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of Events",
                    Values = new ChartValues<int>(eventsByTypeData.Select(d => d.EventCount)),
                },
            };

            var attendeesDistributionData = _context
                .Events.GroupBy(e => e.ExptedAttendee)
                .Select(g => new { AttendeeCount = g.Key, EventCount = g.Count() })
                .OrderBy(d => d.AttendeeCount)
                .ToList();

            AttendeeBins = attendeesDistributionData.Select(d => d.AttendeeCount).ToList();
            AttendeesDistribution = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of Events",
                    Values = new ChartValues<int>(
                        attendeesDistributionData.Select(d => d.EventCount)
                    ),
                },
            };
            var eventsByVenueData = _context
                .Venues.Select(v => new { v.VenueName, EventCount = v.Events.Count })
                .ToList();

            VenueLabels = eventsByVenueData.Select(d => d.VenueName).ToList();
            EventsByVenue = new SeriesCollection();
            foreach (var item in eventsByVenueData)
            {
                EventsByVenue.Add(
                    new PieSeries
                    {
                        Title = item.VenueName,
                        Values = new ChartValues<int> { item.EventCount },
                        DataLabels = true,
                    }
                );
            }

            var heatMapData = _context
                .Events.Select(e => new
                {
                    e.EventName,
                    EquipmentQuantities = e
                        .Requireds.Select(r => new { r.EquipName.EquipName, r.Quantity })
                        .ToList(),
                })
                .ToList();

            EventLabels = heatMapData.Select(e => e.EventName).ToList();
            EquipmentLabels = _context.EquipmentNames.Select(en => en.EquipName).ToList();

            HeatMapData = new SeriesCollection();

            foreach (var eventItem in heatMapData)
            {
                var row = new RowSeries
                {
                    Title = eventItem.EventName,
                    Values = new ChartValues<int>(
                        EquipmentLabels
                            .Select(en =>
                                eventItem
                                    .EquipmentQuantities.Where(eq => eq.EquipName == en)
                                    .Sum(eq => eq.Quantity)
                            )
                            .ToList()
                    ),
                };
                HeatMapData.Add(row);
            }
            var equipmentQuantityByEventData = _context
                .Events.Select(e => new
                {
                    e.EventName,
                    TotalQuantity = e.Requireds.Sum(r => r.Quantity),
                })
                .ToList();

            EventLabels = equipmentQuantityByEventData.Select(d => d.EventName).ToList();
            EquipmentQuantityByEvent = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Total Quantity",
                    Values = new ChartValues<int>(
                        equipmentQuantityByEventData.Select(d => d.TotalQuantity)
                    ),
                },
            };

            var events = _context.Events.Select(e => new { e.EventId, e.EventName }).ToList();

            EventLabels = events.Select(e => e.EventName).ToList();

            var equipmentTypes = _context
                .EquipmentTypes.Select(et => new
                {
                    et.TypeName,
                    EquipmentQuantities = et.EquipmentNames.Select(en => new
                    {
                        en.EquipNameId,
                        en.EquipName,
                        Quantities = en
                            .Requireds.GroupBy(r => r.Event.EventName)
                            .Select(g => new
                            {
                                EventName = g.Key,
                                TotalQuantity = g.Sum(r => r.Quantity),
                            }),
                    }),
                })
                .ToList();

            EquipmentByTypeAcrossEvents = new SeriesCollection();

            foreach (var equipmentType in equipmentTypes)
            {
                var stackedColumnSeries = new StackedColumnSeries
                {
                    Title = equipmentType.TypeName,
                    Values = new ChartValues<int>(),
                };

                foreach (var eventLabel in EventLabels)
                {
                    var totalQuantity = equipmentType
                        .EquipmentQuantities.SelectMany(eq => eq.Quantities)
                        .Where(q => q.EventName == eventLabel)
                        .Sum(q => q.TotalQuantity);

                    stackedColumnSeries.Values.Add(totalQuantity);
                }

                EquipmentByTypeAcrossEvents.Add(stackedColumnSeries);
            }

            var equipmentCostByTypeData = _context
                .EquipmentTypes.Select(et => new
                {
                    et.TypeName,
                    TotalCost = et.EquipmentNames.Sum(en => en.EquipCost),
                })
                .ToList();

            EquipmentTypeLabels = equipmentCostByTypeData.Select(d => d.TypeName).ToList();
            EquipmentCostByType = new SeriesCollection();
            foreach (var item in equipmentCostByTypeData)
            {
                EquipmentCostByType.Add(
                    new PieSeries
                    {
                        Title = item.TypeName,
                        Values = new ChartValues<decimal> { item.TotalCost },
                        DataLabels = true,
                    }
                );
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
