using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
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

        private SeriesCollection _roleSalaries;
        public SeriesCollection RoleSalaries
        {
            get => _roleSalaries;
            set
            {
                _roleSalaries = value;
                OnPropertyChanged(nameof(RoleSalaries));
            }
        }

        private List<string> _roleLabels;
        public List<string> RoleLabels
        {
            get => _roleLabels;
            set
            {
                _roleLabels = value;
                OnPropertyChanged(nameof(RoleLabels));
            }
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

        private SeriesCollection _sponsorCountByTier;
        public SeriesCollection SponsorCountByTier
        {
            get => _sponsorCountByTier;
            set
            {
                _sponsorCountByTier = value;
                OnPropertyChanged(nameof(SponsorCountByTier));
            }
        }

        // Pie Chart: Percentage of sponsors by sponsor tier
        private SeriesCollection _sponsorPercentageByTier;
        public SeriesCollection SponsorPercentageByTier
        {
            get => _sponsorPercentageByTier;
            set
            {
                _sponsorPercentageByTier = value;
                OnPropertyChanged(nameof(SponsorPercentageByTier));
            }
        }

        // Stacked Bar Chart: Sponsors contributing to multiple events by sponsor tier
        private SeriesCollection _stackedSponsorChart;
        public SeriesCollection StackedSponsorChart
        {
            get => _stackedSponsorChart;
            set
            {
                _stackedSponsorChart = value;
                OnPropertyChanged(nameof(StackedSponsorChart));
            }
        }

        private List<string> _tierLabels;
        public List<string> TierLabels
        {
            get => _tierLabels;
            set
            {
                _tierLabels = value;
                OnPropertyChanged(nameof(TierLabels));
            }
        }

        // Bar Chart: Venue cost comparison
        private SeriesCollection _venueCostComparison;
        public SeriesCollection VenueCostComparison
        {
            get => _venueCostComparison;
            set
            {
                _venueCostComparison = value;
                OnPropertyChanged(nameof(VenueCostComparison));
            }
        }

        // Histogram: Venue capacity distribution
        private SeriesCollection _venueCapacityDistribution;
        public SeriesCollection VenueCapacityDistribution
        {
            get => _venueCapacityDistribution;
            set
            {
                _venueCapacityDistribution = value;
                OnPropertyChanged(nameof(VenueCapacityDistribution));
            }
        }

        // Labels for the X-axis
        public string[] VenueCapacityLabels { get; set; }

        private SeriesCollection _showsByGenre;
        public SeriesCollection ShowsByGenre
        {
            get => _showsByGenre;
            set
            {
                _showsByGenre = value;
                OnPropertyChanged(nameof(ShowsByGenre));
            }
        }

        private List<string> _genreLabels;
        public List<string> GenreLabels
        {
            get => _genreLabels;
            set
            {
                _genreLabels = value;
                OnPropertyChanged(nameof(GenreLabels));
            }
        }

        private SeriesCollection _performerCostContribution;
        public SeriesCollection PerformerCostContribution
        {
            get => _performerCostContribution;
            set
            {
                _performerCostContribution = value;
                OnPropertyChanged(nameof(PerformerCostContribution));
            }
        }

        private List<string> _performerLabels;
        public List<string> PerformerLabels
        {
            get => _performerLabels;
            set
            {
                _performerLabels = value;
                OnPropertyChanged(nameof(PerformerLabels));
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

            var sponsorCountByTierData = _context
                .SponsorTiers.Select(t => new
                {
                    t.TierName,
                    SponsorCount = t
                        .IsSponsors.Select(isSponsor => isSponsor.SponsorId)
                        .Distinct()
                        .Count(),
                })
                .ToList();

            TierLabels = sponsorCountByTierData.Select(d => d.TierName).ToList();
            SponsorCountByTier = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of Sponsors",
                    Values = new ChartValues<int>(
                        sponsorCountByTierData.Select(d => d.SponsorCount)
                    ),
                },
            };

            // Pie Chart: Percentage of sponsors by sponsor tier
            var totalSponsors = sponsorCountByTierData.Sum(d => d.SponsorCount);
            SponsorPercentageByTier = new SeriesCollection();
            foreach (var item in sponsorCountByTierData)
            {
                SponsorPercentageByTier.Add(
                    new PieSeries
                    {
                        Title = item.TierName,
                        Values = new ChartValues<double>
                        {
                            (double)item.SponsorCount / totalSponsors * 100,
                        },
                        DataLabels = true,
                    }
                );
            }

            // Stacked Bar Chart: Sponsors contributing to multiple events by sponsor tier
            var stackedSponsorChartData = _context
                .SponsorTiers.Select(t => new
                {
                    t.TierName,
                    SponsorCounts = t
                        .IsSponsors.GroupBy(isSponsor => isSponsor.SponsorId)
                        .Select(g => new { SponsorId = g.Key, EventCount = g.Count() }),
                })
                .ToList();

            StackedSponsorChart = new SeriesCollection();
            foreach (var tier in stackedSponsorChartData)
            {
                var values = tier
                    .SponsorCounts.GroupBy(sc => sc.EventCount)
                    .Select(g => new { EventCount = g.Key, Count = g.Count() })
                    .OrderBy(sc => sc.EventCount)
                    .ToList();

                var columnSeries = new StackedColumnSeries
                {
                    Title = tier.TierName,
                    Values = new ChartValues<int>(values.Select(v => v.Count)),
                };

                StackedSponsorChart.Add(columnSeries);
            }

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

            // Bar Chart: Venue cost comparison
            var venueCostComparisonData = _context
                .Venues.Select(v => new { v.VenueName, v.Cost })
                .ToList();

            VenueCostComparison = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Venue Cost",
                    Values = new ChartValues<decimal>(venueCostComparisonData.Select(d => d.Cost)),
                },
            };

            // Histogram: Venue capacity distribution
            var venueCapacityDistributionData = _context
                .Venues.GroupBy(v => v.Capacity)
                .Select(g => new { Capacity = g.Key, VenueCount = g.Count() })
                .OrderBy(d => d.Capacity)
                .ToList();

            // Assign labels for the X-axis (Capacity values)
            VenueCapacityLabels = venueCapacityDistributionData
                .Select(d => d.Capacity.ToString())
                .ToArray();

            VenueCapacityDistribution = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of Venues",
                    Values = new ChartValues<int>(
                        venueCapacityDistributionData.Select(d => d.VenueCount)
                    ),
                    DataLabels = true, // Display data labels on bars
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 123, 255)), // Custom color
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 82, 170)), // Bar border color
                    StrokeThickness = 1,
                },
            };

            var showsByGenreData = _context
                .Genres.Select(g => new { g.Genre1, ShowCount = g.Shows.Count })
                .ToList();

            GenreLabels = showsByGenreData.Select(d => d.Genre1).ToList();
            ShowsByGenre = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number of Shows",
                    Values = new ChartValues<int>(showsByGenreData.Select(d => d.ShowCount)),
                },
            };

            var performerCostContributionData = _context
                .Performers.Select(p => new { p.FullName, TotalCost = p.Shows.Sum(s => s.Cost) })
                .ToList();

            PerformerLabels = performerCostContributionData.Select(d => d.FullName).ToList();
            PerformerCostContribution = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Total Cost",
                    Values = new ChartValues<decimal>(
                        performerCostContributionData.Select(d => d.TotalCost)
                    ),
                },
            };

            var roleSalariesData = _context
                .EmployeeRoles.Select(er => new { er.RoleName, er.Salary })
                .ToList();

            RoleLabels = roleSalariesData.Select(d => d.RoleName).ToList();
            RoleSalaries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Salaries",
                    Values = new ChartValues<decimal>(roleSalariesData.Select(d => d.Salary)),
                    DataLabels = true,
                },
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
