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

                AssignRandomManagers(context);

                SeedPermissions(context);

                SeedAccounts(context);

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
        new EmployeeRole { RoleName = "Coordinator", Salary = 6000.00M } // General Manager role
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
                        for (int i = 0; i < 100; i++) // Create 100 employees per role as an example
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
                // Determine permission based on employee_id in manager_id
                int permissionId = permissions.FirstOrDefault(p => p.Permission1 == "Employee")?.PermissionId ?? 0;

                var managerRole = roles.FirstOrDefault(r => r.ManagerId == employee.EmployeeId);
                if (managerRole != null)
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





    }
}
