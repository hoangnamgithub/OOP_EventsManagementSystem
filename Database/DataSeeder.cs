using Bogus;
using OOP_EventsManagementSystem.Model;
using System.Data.Entity;

namespace OOP_EventsManagementSystem.Database
{
    internal class DataSeeder
    {
        public static void SeedData()
        {
            using (var context = new EventManagementDbContext())
            {
                SeedVenueData(context); //1

                SeedGenreData(context); //2 

                SeedPerformerData(context); //3

                SeedEventTypeData(context); //4

                SeedSponsorsData(context); //5

                SeedEmployeeRoles(context); //6

                SeedEmployeeData(context); //7
                AssignRandomManagers(context);//7

                SeedPermissions(context);//9

                SeedAccounts(context);//10

                SeedSponsorTierData(context);//11

                SeedEventData(context);//12

                SeedNeedData(context);//13

                SeedIsSponsorData(context);//14

                SeedShowData(context);//15

                SeedShowScheduleData(context);//16

                SeedEquipmentTypeData(context);//17

                SeedEquipmentNameData(context);//18

                SeedRequiredData(context);//19

                SeedEquipmentData(context);//20

                SeedEngagedData(context);
            }
        }

        private static void SeedVenueData(EventManagementDbContext context)
        {
            // Kiểm tra xem dữ liệu đã được seed hay chưa
            if (context.Venues.Any())
            {
                Console.WriteLine("Venue data already seeded.");
                return;
            }

            // Bogus Faker instance để tạo dữ liệu giả
            var faker = new Bogus.Faker();

            var venues = new List<Venue>();
            for (int i = 0; i < 20; i++)
            {
                var venue = new Venue
                {
                    VenueName = faker.Company.CompanyName(),
                    Cost = faker.Finance.Amount(2000, 15000),
                    Address = faker.Address.FullAddress(),
                    Capacity = faker.Random.Int(70, 1000),
                };

                venues.Add(venue);
            }

            // Thêm dữ liệu vào database
            context.Venues.AddRange(venues);
            context.SaveChanges();

            Console.WriteLine("Seeded venue data successfully.");
        }

        private static void SeedGenreData(EventManagementDbContext context)
        {
            // Kiểm tra xem dữ liệu đã được seed hay chưa
            if (context.Genres.Any())
            {
                Console.WriteLine("Genre data already seeded.");
                return;
            }

            // List of predefined stage performing arts genres
            var predefinedGenres = new List<string>
    {
        "Classical Theater",
        "Comedy",
        "Modern Drama",
        "Opera",
        "Ballet",
        "Musicals",
        "Physical Theater",
        "Classical Ballet",
        "Hip-hop Dance",
        "Contemporary Dance",
        "Flamenco",
        "Ballroom Dance",
        "Folk Dance",
        "Pop",
        "Rock",
        "Jazz",
        "Classical",
        "Blues",
        "Country",
        "Electronic",
        "Soul/R&B"
    };

            // Map genres to Genre entity objects
            var genres = predefinedGenres.Select(g => new Genre
            {
                Genre1 = g
            }).ToList();

            // Thêm dữ liệu Genre vào database
            context.Genres.AddRange(genres);
            context.SaveChanges();
            Console.WriteLine($"Seeded {genres.Count} predefined Genre data successfully.");
        }

        private static void SeedPerformerData(EventManagementDbContext context)
        {
            // Kiểm tra xem dữ liệu đã được seed hay chưa
            if (context.Performers.Any())
            {
                Console.WriteLine("Performer data already seeded.");
                return;
            }

            // Bogus Faker instance để tạo dữ liệu giả
            var faker = new Bogus.Faker();

            // Tạo 1000 Performer
            var performers = new List<Performer>();
            for (int i = 0; i < 200; i++)
            {
                var performer = new Performer
                {
                    FullName = faker.Name.FullName(),
                    ContactDetail = faker.Phone.PhoneNumber(),
                };

                performers.Add(performer);
            }

            // Thêm dữ liệu Performer vào database
            context.Performers.AddRange(performers);
            context.SaveChanges();
            Console.WriteLine("Seeded 200 Performer data successfully.");
        }

        private static void SeedEventTypeData(EventManagementDbContext context)
        {
            // Check if EventType data has already been seeded
            if (context.EventTypes.Any())
            {
                Console.WriteLine("EventType data already seeded.");
                return;
            }

            // List of predefined event types
            var predefinedEventTypes = new List<string>
    {
        "Conference",
        "Seminar",
        "Workshop",
        "Webinar",
        "Trade Show",
        "Expo",
        "Networking Event",
        "Product Launch",
        "Fundraiser",
        "Awards Ceremony",
        "Concert",
        "Festival",
        "Team Building Event",
        "Charity Event",
        "Meetup",
        "Symposium",
        "Summit",
        "Hackathon",
        "Retreat",
        "Press Conference",
        "Training Session",
        "Lectures",
        "Cultural Festival",
        "Community Event",
        "Sporting Event",
        "Gala",
        "Networking Breakfast/Lunch/Dinner",
        "Open House",
        "Pop-Up Event",
        "Focus Group",
        "Anniversary Celebration",
        "Orientation Event",
        "Demo Day",
        "Charity Walk/Run",
        "Auction",
        "Meet and Greet"
    };

            // Map event types to EventType entity objects
            var eventTypes = predefinedEventTypes.Select(et => new EventType
            {
                TypeName = et
            }).ToList();

            // Add EventType data to the database
            context.EventTypes.AddRange(eventTypes);
            context.SaveChanges();
            Console.WriteLine($"Seeded {eventTypes.Count} predefined EventType data successfully.");
        }

        private static void SeedSponsorsData(EventManagementDbContext context)
        {
            // Kiểm tra xem dữ liệu đã được seed hay chưa
            if (context.Sponsors.Any())
            {
                Console.WriteLine("Sponsors data already seeded.");
                return;
            }

            var faker = new Bogus.Faker();

            var sponsors = new List<Sponsor>();
            for (int i = 0; i < 200; i++)
            {
                var sponsor = new Sponsor
                {
                    SponsorName = faker.Company.CompanyName(),
                    SponsorDetails = faker.Lorem.Sentence(50),
                };
                sponsors.Add(sponsor);
            }

            context.Sponsors.AddRange(sponsors);
            context.SaveChanges();
            Console.WriteLine("Seeded 50 Sponsors data successfully.");
        }

        private static void SeedEmployeeRoles(EventManagementDbContext context)
        {
            if (context.EmployeeRoles.Any())
            {
                Console.WriteLine("Employee roles already seeded.");
                return;
            }

            var roles = new List<EmployeeRole>
    {
        new EmployeeRole { RoleName = "PlanningAndCoordination", Salary = 4000.00M },
        new EmployeeRole { RoleName = "Logistics", Salary = 3500.00M },
        new EmployeeRole { RoleName = "TechnicalSupport", Salary = 3500.00M },
        new EmployeeRole { RoleName = "MarketingAndPromotions", Salary = 4000.00M },
        new EmployeeRole { RoleName = "SalesAndTicketing", Salary = 3500.00M },
        new EmployeeRole { RoleName = "Catering", Salary = 4000.00M },
        new EmployeeRole { RoleName = "Security", Salary = 3000.00M },
        new EmployeeRole { RoleName = "FinanceAndBudgeting", Salary = 4500.00M },
        new EmployeeRole { RoleName = "CustomerService", Salary = 3500.00M },
        new EmployeeRole { RoleName = "DesignAndDecor", Salary = 4000.00M },
        new EmployeeRole { RoleName = "EntertainmentAndProgramming", Salary = 4500.00M },
        new EmployeeRole { RoleName = "VenueManagement", Salary = 3500.00M },
        new EmployeeRole { RoleName = "PublicRelations", Salary = 4000.00M },
        new EmployeeRole { RoleName = "Coordinator", Salary = 6000.00M }
    };

            context.EmployeeRoles.AddRange(roles);
            context.SaveChanges();
            Console.WriteLine("Seeded employee roles successfully.");
        }

        private static void SeedEmployeeData(EventManagementDbContext context)
        {
            if (context.Employees.Any())
            {
                Console.WriteLine("Employee data already seeded.");
                return;
            }

            var roles = context.EmployeeRoles.ToList();
            if (!roles.Any())
            {
                Console.WriteLine("No roles found. Please seed roles first.");
                return;
            }

            var faker = new Bogus.Faker();
            var employees = new List<Employee>();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Step 1: Create employees for each role
                    foreach (var role in roles)
                    {
                        for (int i = 0; i < 50; i++) // Create 100 employees per role as an example
                        {
                            var employee = new Employee
                            {
                                FullName = faker.Name.FullName(),
                                Contact = faker.Phone.PhoneNumber(),
                                RoleId = role.RoleId
                            };

                            context.Employees.Add(employee);
                        }
                    }

                    context.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine("Employees added successfully.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error seeding data: {ex.Message}");
                }
            }
        }

        private static void AssignRandomManagers(EventManagementDbContext context)
        {
            var employees = context.Employees.ToList();
            var random = new Random();

            // Assign random managers to each role
            foreach (var role in context.EmployeeRoles.Where(r => r.RoleName != "Manager"))
            {
                var manager = employees[random.Next(employees.Count)];
                role.ManagerId = manager.EmployeeId;
            }

            context.SaveChanges();
            Console.WriteLine("Assigned random managers to roles successfully.");
        }

        private static void SeedPermissions(EventManagementDbContext context)
        {
            if (context.Permissions.Any())
            {
                Console.WriteLine("Permissions already seeded.");
                return;
            }

            var permissions = new List<Permission>
            {
                new Permission { Permission1 = "Admin" },
                new Permission { Permission1 = "Manager" },
                new Permission { Permission1 = "Employee" }
            };

            context.Permissions.AddRange(permissions);
            context.SaveChanges();

            Console.WriteLine("Seeded permission data successfully.");
        }

        private static void SeedAccounts(EventManagementDbContext context)
        {
            if (context.Accounts.Any())
            {
                Console.WriteLine("Accounts already seeded.");
                return;
            }

            // Fetch required data
            var employees = context.Employees.Include(e => e.Role).ToList();
            var permissions = context.Permissions.ToList();
            var roles = context.EmployeeRoles.ToList();

            if (!employees.Any() || !permissions.Any() || !roles.Any())
            {
                Console.WriteLine("No employees, roles, or permissions found. Please seed these tables first.");
                return;
            }

            var accounts = new List<Account>();

            foreach (var employee in employees)
            {
                // Default permission for employees
                int permissionId = permissions.FirstOrDefault(p => p.Permission1 == "Employee")?.PermissionId ?? 0;

                // Check if the employee is a manager by checking if their EmployeeId exists in EmployeeRole.ManagerId
                bool isManager = roles.Any(r => r.ManagerId == employee.EmployeeId);
                if (isManager)
                {
                    permissionId = permissions.FirstOrDefault(p => p.Permission1 == "Manager")?.PermissionId ?? 0;
                }

                if (permissionId == 0)
                {
                    Console.WriteLine($"Permission not found for employee: {employee.FullName}");
                    continue;
                }

                // Create account for the employee
                var account = new Account
                {
                    Email = $"{employee.FullName.Replace(" ", "").ToLower()}@easys.com",
                    Password = Guid.NewGuid().ToString().Substring(0, 8), // Generate a random password
                    PermissionId = permissionId,
                    EmployeeId = employee.EmployeeId
                };

                accounts.Add(account);
            }

            if (accounts.Any())
            {
                context.Accounts.AddRange(accounts);
                context.SaveChanges();
                Console.WriteLine("Seeded account data successfully.");
            }
            else
            {
                Console.WriteLine("No accounts were created. Please check the seeding logic.");
            }
        }

        private static void SeedSponsorTierData(EventManagementDbContext context)
        {
            // Check if SponsorTier data has already been seeded
            if (context.SponsorTiers.Any())
            {
                Console.WriteLine("SponsorTier data already seeded.");
                return;
            }

            // List of predefined sponsorship tiers
            var predefinedTiers = new List<string>
    {
        "Title Sponsor",
        "Platinum Sponsor",
        "Gold Sponsor",
        "Silver Sponsor",
        "Bronze Sponsor",
        "Supporting Sponsor"
    };

            // Map tiers to SponsorTier entity objects
            var sponsorTiers = predefinedTiers.Select(t => new SponsorTier
            {
                TierName = t
            }).ToList();

            // Add SponsorTier data to the database
            context.SponsorTiers.AddRange(sponsorTiers);
            context.SaveChanges();
            Console.WriteLine($"Seeded {sponsorTiers.Count} predefined SponsorTier data successfully.");
        }

        private static void SeedEventData(EventManagementDbContext context)
        {
            if (context.Events.Any())
            {
                Console.WriteLine("Event data already seeded.");
                return;
            }

            var eventTypes = context.EventTypes.ToList();
            var venues = context.Venues.ToList();
            if (!eventTypes.Any() || !venues.Any())
            {
                Console.WriteLine("No event types or venues found. Please seed these tables first.");
                return;
            }

            var faker = new Bogus.Faker();
            var events = new List<Event>();
            var today = DateTime.Today;
            var currentYear = today.Year;

            for (int i = 0; i < 50; i++)
            {
                var eventType = eventTypes[faker.Random.Int(0, eventTypes.Count - 1)];
                var venue = venues[faker.Random.Int(0, venues.Count - 1)];

                // Randomly assign dates to be in the past, present, or near future
                DateTime startDate;
                if (i % 6 == 0) // Approximately one sixth in the past
                {
                    startDate = faker.Date.Past(1, today); // Past within the last year
                }
                else if (i % 2 != 0) // Approximately two thirds in the present
                {
                    startDate = faker.Date.Between(today.AddMonths(-1), today.AddMonths(1)); // Within the current date range
                }
                else // Approximately one sixth in the near future
                {
                    startDate = faker.Date.Between(new DateTime(2025, 1, 1), new DateTime(2025, 3, 31)); // Future within the first few months of 2025
                }

                var endDate = startDate.AddDays(faker.Random.Int(1, 5)); // Ensuring end date is after start date

                var @event = new Event
                {
                    EventName = faker.Company.CompanyName(),
                    EventDescription = string.Join(" ", faker.Lorem.Sentences(5)), // Generate 5 sentences of description
                    ExptedAttendee = faker.Random.Int(50, venue.Capacity), // Generate expected attendee count within venue capacity
                    StartDate = DateOnly.FromDateTime(startDate),
                    EndDate = DateOnly.FromDateTime(endDate),
                    EventTypeId = eventType.EventTypeId,
                    VenueId = venue.VenueId
                };

                events.Add(@event);
            }

            context.Events.AddRange(events);
            context.SaveChanges();
            Console.WriteLine("Seeded event data successfully.");
        }







        private static void SeedNeedData(EventManagementDbContext context)
        {
            if (context.Needs.Any())
            {
                Console.WriteLine("Need data already seeded.");
                return;
            }

            var roles = context.EmployeeRoles.ToList();
            var events = context.Events.ToList();
            if (!roles.Any() || !events.Any())
            {
                Console.WriteLine("No roles or events found. Please seed these tables first.");
                return;
            }

            var faker = new Bogus.Faker();
            var needs = new List<Need>();

            foreach (var @event in events)
            {
                foreach (var role in roles)
                {
                    var quantity = faker.Random.Int(0, 10); // Quantity can be between 0 and 10

                    var need = new Need
                    {
                        RoleId = role.RoleId,
                        EventId = @event.EventId,
                        Quantity = quantity
                    };

                    needs.Add(need);
                }
            }

            context.Needs.AddRange(needs);
            context.SaveChanges();
            Console.WriteLine("Seeded need data successfully.");
        }

        private static void SeedIsSponsorData(EventManagementDbContext context)
        {
            if (context.IsSponsors.Any())
            {
                Console.WriteLine("IsSponsor data already seeded.");
                return;
            }

            var events = context.Events.ToList();
            var sponsors = context.Sponsors.ToList();
            var sponsorTiers = context.SponsorTiers.ToList();

            if (!events.Any() || !sponsors.Any() || !sponsorTiers.Any())
            {
                Console.WriteLine("No events, sponsors, or sponsor tiers found. Please seed these tables first.");
                return;
            }

            var faker = new Bogus.Faker();

            // Fetch existing IsSponsor combinations from the database
            var existingIsSponsors = context.IsSponsors
                .Select(isSponsor => new { isSponsor.EventId, isSponsor.SponsorId, isSponsor.SponsorTierId })
                .ToList();

            var isSponsors = new List<IsSponsor>();

            foreach (var @event in events)
            {
                // Shuffle sponsors for more variety
                var shuffledSponsors = sponsors.OrderBy(_ => faker.Random.Int()).ToList();
                int numSponsors = faker.Random.Int(1, sponsors.Count);

                for (int i = 0; i < numSponsors; i++)
                {
                    var sponsor = shuffledSponsors[i];
                    var sponsorTier = sponsorTiers[faker.Random.Int(0, sponsorTiers.Count - 1)];

                    var isSponsor = new IsSponsor
                    {
                        EventId = @event.EventId,
                        SponsorId = sponsor.SponsorId,
                        SponsorTierId = sponsorTier.SponsorTierId
                    };

                    // Avoid duplicates in database and in-memory
                    bool alreadyExistsInDb = existingIsSponsors.Any(existing =>
                        existing.EventId == isSponsor.EventId &&
                        existing.SponsorId == isSponsor.SponsorId &&
                        existing.SponsorTierId == isSponsor.SponsorTierId);

                    bool alreadyExistsInMemory = isSponsors.Any(existing =>
                        existing.EventId == isSponsor.EventId &&
                        existing.SponsorId == isSponsor.SponsorId &&
                        existing.SponsorTierId == isSponsor.SponsorTierId);

                    if (alreadyExistsInDb || alreadyExistsInMemory)
                    {
                        continue;
                    }

                    isSponsors.Add(isSponsor);
                }
            }

            if (isSponsors.Any())
            {
                context.IsSponsors.AddRange(isSponsors);
                context.SaveChanges();
                Console.WriteLine("Seeded IsSponsor data successfully.");
            }
            else
            {
                Console.WriteLine("No IsSponsor data to seed.");
            }
        }

        private static void SeedShowData(EventManagementDbContext context)
        {
            if (context.Shows.Any())
            {
                Console.WriteLine("Show data already seeded.");
                return;
            }

            var performers = context.Performers.ToList();
            var genres = context.Genres.ToList();
            if (!performers.Any() || !genres.Any())
            {
                Console.WriteLine("No performers or genres found. Please seed these tables first.");
                return;
            }

            var faker = new Bogus.Faker();
            var shows = new List<Show>();

            foreach (var performer in performers)
            {
                // Each performer can have multiple shows
                int numShows = faker.Random.Int(1, 10);

                for (int i = 0; i < numShows; i++)
                {
                    var genre = genres[faker.Random.Int(0, genres.Count - 1)];
                    var show = new Show
                    {
                        ShowName = faker.Lorem.Sentence(3), // Generate a show name with 3 words
                        Cost = faker.Finance.Amount(1000, 5000), // Random cost between 100 and 1000
                        PerformerId = performer.PerformerId,
                        GenreId = genre.GenreId
                    };

                    shows.Add(show);
                }
            }

            context.Shows.AddRange(shows);
            context.SaveChanges();
            Console.WriteLine("Seeded show data successfully.");
        }

        private static void SeedShowScheduleData(EventManagementDbContext context)
        {
            if (context.ShowSchedules.Any())
            {
                Console.WriteLine("ShowSchedule data already seeded.");
                return;
            }

            var shows = context.Shows.ToList();
            var events = context.Events.ToList();
            if (!shows.Any() || !events.Any())
            {
                Console.WriteLine("No shows or events found. Please seed these tables first.");
                return;
            }

            var faker = new Bogus.Faker();
            var showSchedules = new List<ShowSchedule>();

            foreach (var @event in events)
            {
                // Each event needs a few different shows
                int numShows = faker.Random.Int(1, 10);

                for (int i = 0; i < numShows; i++)
                {
                    var show = shows[faker.Random.Int(0, shows.Count - 1)];
                    var startDate = faker.Date.Between(@event.StartDate.ToDateTime(TimeOnly.MinValue), @event.EndDate.ToDateTime(TimeOnly.MinValue));
                    var estDuration = faker.Random.Int(30, 120); // Estimated duration between 30 minutes and 3 hours

                    var showSchedule = new ShowSchedule
                    {
                        StartDate = DateOnly.FromDateTime(startDate),
                        EstDuration = estDuration,
                        ShowId = show.ShowId,
                        EventId = @event.EventId
                    };

                    // Avoid duplicates
                    if (!showSchedules.Any(x => x.ShowId == showSchedule.ShowId && x.EventId == showSchedule.EventId))
                    {
                        showSchedules.Add(showSchedule);
                    }
                }
            }

            context.ShowSchedules.AddRange(showSchedules);
            context.SaveChanges();
            Console.WriteLine("Seeded ShowSchedule data successfully.");
        }

        private static void SeedEquipmentTypeData(EventManagementDbContext context)
        {
            // Check if EquipmentType data has already been seeded
            if (context.EquipmentTypes.Any())
            {
                Console.WriteLine("EquipmentType data already seeded.");
                return;
            }

            // List of general equipment types
            var predefinedEquipmentTypes = new List<string>
    {
        "Audio Equipment",
        "Visual Equipment",
        "Lighting Equipment",
        "Staging Equipment",
        "Communication Equipment",
        "Power Equipment",
        "Seating and Furniture",
        "Tents and Canopies",
        "Decoration Equipment",
        "Catering Equipment",
        "Safety and Security Equipment",
        "Miscellaneous"
    };

            // Map equipment types to EquipmentType entity objects
            var equipmentTypes = predefinedEquipmentTypes.Select(et => new EquipmentType
            {
                TypeName = et
            }).ToList();

            // Add EquipmentType data to the database
            context.EquipmentTypes.AddRange(equipmentTypes);
            context.SaveChanges();
            Console.WriteLine($"Seeded {equipmentTypes.Count} predefined EquipmentType data successfully.");
        }

        private static void SeedEquipmentNameData(EventManagementDbContext context)
        {
            if (context.EquipmentNames.Any())
            {
                Console.WriteLine("EquipmentName data already seeded.");
                return;
            }

            var equipmentTypes = context.EquipmentTypes.ToList();
            if (!equipmentTypes.Any())
            {
                Console.WriteLine("No equipment types found. Please seed these tables first.");
                return;
            }

            var faker = new Bogus.Faker();
            var equipmentNames = new List<EquipmentName>();

            var equipmentDetails = new Dictionary<string, List<string>>
    {
        { "Audio Equipment", new List<string> { "Microphone", "Speaker", "Subwoofer", "Mixer", "Amplifier" } },
        { "Visual Equipment", new List<string> { "Projector", "Screen", "LED Wall", "Monitor", "Camera" } },
        { "Lighting Equipment", new List<string> { "Stage Light", "Uplight", "Spotlight", "Gobo", "DMX Controller" } },
        { "Staging Equipment", new List<string> { "Portable Stage", "Stage Riser", "Stage Skirt", "Podium", "Stage Barrier" } },
        { "Communication Equipment", new List<string> { "Two-way Radio", "Intercom System", "Paging System", "Walkie-talkie", "Headset" } },
        { "Power Equipment", new List<string> { "Generator", "Extension Cord", "Power Strip", "Power Conditioner", "UPS" } },
        { "Seating and Furniture", new List<string> { "Folding Chair", "Banquet Table", "Sofa", "Bar Stool", "Lectern" } },
        { "Tents and Canopies", new List<string> { "Pop-up Tent", "Canopy", "Marquee", "Gazebo", "Sidewall" } },
        { "Decoration Equipment", new List<string> { "Banner", "Balloon", "Flower Arrangement", "Centerpiece", "Tablecloth" } },
        { "Catering Equipment", new List<string> { "Chafing Dish", "Beverage Dispenser", "Cooler", "Serving Tray", "Tableware" } },
        { "Safety and Security Equipment", new List<string> { "Fire Extinguisher", "First Aid Kit", "Barrier", "Security Camera", "Emergency Exit Sign" } },
        { "Miscellaneous", new List<string> { "Signage", "Registration Desk", "Coat Rack", "Trash Bin", "Hand Sanitizing Station" } }
    };

            foreach (var equipmentType in equipmentTypes)
            {
                if (equipmentDetails.ContainsKey(equipmentType.TypeName))
                {
                    foreach (var equipName in equipmentDetails[equipmentType.TypeName])
                    {
                        var equipmentName = new EquipmentName
                        {
                            EquipName = equipName,
                            EquipCost = faker.Finance.Amount(100, 10000), // Random cost between 100 and 10,000
                            EquipTypeId = equipmentType.EquipTypeId
                        };

                        equipmentNames.Add(equipmentName);
                    }
                }
            }

            context.EquipmentNames.AddRange(equipmentNames);
            context.SaveChanges();
            Console.WriteLine("Seeded EquipmentName data successfully.");
        }

        private static void SeedRequiredData(EventManagementDbContext context)
        {
            if (context.Requireds.Any())
            {
                Console.WriteLine("Required data already seeded.");
                return;
            }

            var events = context.Events.ToList();
            var equipmentNames = context.EquipmentNames.ToList();
            if (!events.Any() || !equipmentNames.Any())
            {
                Console.WriteLine("No events or equipment names found. Please seed these tables first.");
                return;
            }

            var faker = new Bogus.Faker();
            var requireds = new List<Required>();

            foreach (var @event in events)
            {
                foreach (var equipName in equipmentNames)
                {
                    var quantity = faker.Random.Int(0, 10); // Quantity can be between 0 and 10

                    var required = new Required
                    {
                        Quantity = quantity,
                        EventId = @event.EventId,
                        EquipNameId = equipName.EquipNameId
                    };

                    requireds.Add(required);
                }
            }

            context.Requireds.AddRange(requireds);
            context.SaveChanges();
            Console.WriteLine("Seeded Required data successfully.");
        }

        private static void SeedEquipmentData(EventManagementDbContext context)
        {
            if (context.Equipment.Any())
            {
                Console.WriteLine("Equipment data already seeded.");
                return;
            }

            var requiredEquipments = context.Requireds.ToList();
            if (!requiredEquipments.Any())
            {
                Console.WriteLine("No required equipment found. Please seed these tables first.");
                return;
            }

            var faker = new Bogus.Faker();
            var equipments = new List<Equipment>();

            var conditions = new List<string> { "Good", "Broken", "Missing", "Needs Repair", "Excellent", "Poor" };

            foreach (var required in requiredEquipments)
            {
                for (int i = 0; i < required.Quantity; i++)
                {
                    var equipment = new Equipment
                    {
                        EquipNameId = required.EquipNameId,
                        Condition = conditions[faker.Random.Int(0, conditions.Count - 1)]
                    };

                    equipments.Add(equipment);
                }
            }

            context.Equipment.AddRange(equipments);
            context.SaveChanges();
            Console.WriteLine("Seeded Equipment data successfully.");
        }


        private static void SeedEngagedData(EventManagementDbContext context)
        {
            if (context.Engageds.Any())
            {
                Console.WriteLine("Engaged data already seeded.");
                return;
            }

            var accounts = context.Accounts.Include(a => a.Employee).ToList();
            var events = context.Events.ToList();
            var needs = context.Needs.ToList();

            if (!accounts.Any() || !events.Any() || !needs.Any())
            {
                Console.WriteLine("No accounts, events, or needs found. Please seed these tables first.");
                return;
            }

            var faker = new Bogus.Faker();
            var engageds = new List<Engaged>();

            foreach (var need in needs)
            {
                var requiredQuantity = need.Quantity;
                var assignedCount = context.Engageds.Count(e => e.EventId == need.EventId && e.Account.Employee.RoleId == need.RoleId);

                if (assignedCount >= requiredQuantity)
                {
                    continue;
                }

                var availableAccounts = accounts.Where(a => a.Employee.RoleId == need.RoleId).ToList();

                for (int i = assignedCount; i < requiredQuantity; i++)
                {
                    if (availableAccounts.Any())
                    {
                        var account = availableAccounts[faker.Random.Int(0, availableAccounts.Count - 1)];
                        var engaged = new Engaged
                        {
                            AccountId = account.AccountId,
                            EventId = need.EventId
                        };

                        engageds.Add(engaged);
                        availableAccounts.Remove(account); // Remove to prevent duplicate assignments
                    }
                }
            }

            context.Engageds.AddRange(engageds);
            context.SaveChanges();
            Console.WriteLine("Seeded Engaged data successfully.");
        }




    }
}
