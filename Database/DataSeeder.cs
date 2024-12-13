using Bogus;
using OOP_EventsManagementSystem.Model;

namespace OOP_EventsManagementSystem.Database
{
    internal class DataSeeder
    {
        public static void SeedData()
        {
            using (var context = new EventManagementDbContext())
            {
                // Seed Venue Data
                SeedVenueData(context);

                // Seed Genre Data
                SeedGenreData(context);

                // Seed Performer Data
                SeedPerformerData(context);

                // Seed EventType Data
                SeedEventTypeData(context);

                // Seed Sponsors Data
                SeedSponsorsData(context);

                // Seed Roles Data
                SeedEmployeeRoles(context);

                SeedEmployeeData(context);

                SeedPermissions(context);

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
                    Cost = faker.Finance.Amount(1000, 10000),
                    Address = faker.Address.FullAddress(),
                    Capacity = faker.Random.Int(50, 500),
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
            for (int i = 0; i < 1000; i++)
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
            Console.WriteLine("Seeded 1000 Performer data successfully.");
        }

        private static void SeedEventTypeData(EventManagementDbContext context)
        {
            // Kiểm tra xem dữ liệu đã được seed hay chưa
            if (context.EventTypes.Any())
            {
                Console.WriteLine("EventType data already seeded.");
                return;
            }

            var faker = new Bogus.Faker();

            var eventTypes = new List<EventType>();
            for (int i = 0; i < 20; i++)
            {
                var eventType = new EventType
                {
                    TypeName = faker.Lorem.Word(),
                };
                eventTypes.Add(eventType);
            }

            context.EventTypes.AddRange(eventTypes);
            context.SaveChanges();
            Console.WriteLine("Seeded 20 EventType data successfully.");
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
            for (int i = 0; i < 100; i++)
            {
                var sponsor = new Sponsor
                {
                    SponsorName = faker.Company.CompanyName(),
                    SponsorDetails = faker.Lorem.Sentence(),
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
                new EmployeeRole { RoleName = "PlanningAndCoordinationManager", Salary = 6100.00M },
                new EmployeeRole { RoleName = "Logistics", Salary = 3500.00M },
                new EmployeeRole { RoleName = "LogisticsManager", Salary = 5500.00M },
                new EmployeeRole { RoleName = "TechnicalSupport", Salary = 3500.00M },
                new EmployeeRole { RoleName = "TechnicalSupportManager", Salary = 6000.00M },
                new EmployeeRole { RoleName = "MarketingAndPromotions", Salary = 4000.00M },
                new EmployeeRole { RoleName = "MarketingAndPromotionsManager", Salary = 6000.00M },
                new EmployeeRole { RoleName = "SalesAndTicketing", Salary = 3500.00M },
                new EmployeeRole { RoleName = "SalesAndTicketingManager", Salary = 5500.00M },
                new EmployeeRole { RoleName = "Catering", Salary = 4000.00M },
                new EmployeeRole { RoleName = "CateringManager", Salary = 6000.00M },
                new EmployeeRole { RoleName = "Security", Salary = 3000.00M },
                new EmployeeRole { RoleName = "SecurityManager", Salary = 5000.00M },
                new EmployeeRole { RoleName = "FinanceAndBudgeting", Salary = 4500.00M },
                new EmployeeRole { RoleName = "FinanceAndBudgetingManager", Salary = 6500.00M },
                new EmployeeRole { RoleName = "CustomerService", Salary = 3500.00M },
                new EmployeeRole { RoleName = "CustomerServiceManager", Salary = 5500.00M },
                new EmployeeRole { RoleName = "DesignAndDecor", Salary = 4000.00M },
                new EmployeeRole { RoleName = "DesignAndDecorManager", Salary = 6000.00M },
                new EmployeeRole { RoleName = "EntertainmentAndProgramming", Salary = 4500.00M },
                new EmployeeRole { RoleName = "EntertainmentAndProgrammingManager", Salary = 6500.00M },
                new EmployeeRole { RoleName = "VenueManagement", Salary = 3500.00M },
                new EmployeeRole { RoleName = "VenueManagementManager", Salary = 5500.00M },
                new EmployeeRole { RoleName = "PublicRelations", Salary = 4000.00M },
                new EmployeeRole { RoleName = "PublicRelationsManager", Salary = 6000.00M }
            };

            context.EmployeeRoles.AddRange(roles);
            context.SaveChanges();
            Console.WriteLine("Seeded employee roles successfully.");
        }

        // fix seed employee
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
                    // Step 1: Create managers first
                    foreach (var role in roles.Where(r => r.RoleName.EndsWith("Manager")))
                    {
                        var manager = new Employee
                        {
                            FullName = faker.Name.FullName(),
                            Contact = faker.Phone.PhoneNumber(),
                            RoleId = role.RoleId,
                            ManagerId = null // Managers don't have managers
                        };

                        context.Employees.Add(manager);
                    }

                    context.SaveChanges();

                    // Step 2: Assign employees to their respective roles
                    foreach (var role in roles.Where(r => !r.RoleName.EndsWith("Manager")))
                    {
                        for (int i = 0; i < 100; i++) // Create 100 employees per role as an example
                        {
                            var employee = new Employee
                            {
                                FullName = faker.Name.FullName(),
                                Contact = faker.Phone.PhoneNumber(),
                                RoleId = role.RoleId,
                                ManagerId = null // Leave ManagerId as null
                            };

                            context.Employees.Add(employee);
                        }
                    }

                    context.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine("Employees added without setting ManagerId.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error seeding data: {ex.Message}");
                }
            }
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



    }
}
