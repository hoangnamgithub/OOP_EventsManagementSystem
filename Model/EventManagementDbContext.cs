using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OOP_EventsManagementSystem.Model;

public partial class EventManagementDbContext : DbContext
{
    public EventManagementDbContext()
    {
    }

    public EventManagementDbContext(DbContextOptions<EventManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }

    public virtual DbSet<Engaged> Engageds { get; set; }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<EquipmentName> EquipmentNames { get; set; }

    public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<IsPartner> IsPartners { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<PartnerRole> PartnerRoles { get; set; }

    public virtual DbSet<Performer> Performers { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Required> Requireds { get; set; }

    public virtual DbSet<Show> Shows { get; set; }

    public virtual DbSet<ShowSchedule> ShowSchedules { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=EventManagementDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__account__46A222CDF801DE8A");

            entity.ToTable("account", "Account");

            entity.HasIndex(e => e.Email, "UQ__account__AB6E616490A5C2C6").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__account__employe__2E1BDC42");

            entity.HasOne(d => d.Permission).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK__account__permiss__2D27B809");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__employee__C52E0BA8A9AEB6BB");

            entity.ToTable("employee", "Employees");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Contact).HasColumnName("contact");
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .HasColumnName("full_name");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__employee__role_i__276EDEB3");
        });

        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__employee__760965CC86341C85");

            entity.ToTable("employee_role", "Employees");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");
            entity.Property(e => e.Salary)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("salary");
        });

        modelBuilder.Entity<Engaged>(entity =>
        {
            entity.HasKey(e => e.EngagedId).HasName("PK__engaged__EAEA56F08CF96AD8");

            entity.ToTable("engaged", "Employees");

            entity.Property(e => e.EngagedId).HasColumnName("engaged_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.Engageds)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__engaged__employe__5EBF139D");

            entity.HasOne(d => d.Event).WithMany(p => p.Engageds)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__engaged__event_i__5DCAEF64");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("PK__equipmen__197068AFB158D9CC");

            entity.ToTable("equipment", "Equipment");

            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.Available).HasColumnName("available");
            entity.Property(e => e.EquipNameId).HasColumnName("equip_name_id");

            entity.HasOne(d => d.EquipName).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.EquipNameId)
                .HasConstraintName("FK__equipment__equip__5629CD9C");
        });

        modelBuilder.Entity<EquipmentName>(entity =>
        {
            entity.HasKey(e => e.EquipNameId).HasName("PK__equipmen__03FCFCF4015D1698");

            entity.ToTable("equipment_name", "Equipment");

            entity.Property(e => e.EquipNameId).HasColumnName("equip_name_id");
            entity.Property(e => e.EquipCost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("equip_cost");
            entity.Property(e => e.EquipName)
                .HasMaxLength(200)
                .HasColumnName("equip_name");
            entity.Property(e => e.EquipTypeId).HasColumnName("equip_type_id");

            entity.HasOne(d => d.EquipType).WithMany(p => p.EquipmentNames)
                .HasForeignKey(d => d.EquipTypeId)
                .HasConstraintName("FK__equipment__equip__534D60F1");
        });

        modelBuilder.Entity<EquipmentType>(entity =>
        {
            entity.HasKey(e => e.EquipTypeId).HasName("PK__equipmen__39BE18CC7598579E");

            entity.ToTable("equipment_type", "Equipment");

            entity.Property(e => e.EquipTypeId).HasColumnName("equip_type_id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__event__2370F727D5867CF4");

            entity.ToTable("event", "Events");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.EventDescription).HasColumnName("event_description");
            entity.Property(e => e.EventName)
                .HasMaxLength(200)
                .HasColumnName("event_name");
            entity.Property(e => e.EventTypeId).HasColumnName("event_type_id");
            entity.Property(e => e.ExpectedAttendee).HasColumnName("expected_attendee");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.VenueId).HasColumnName("venue_id");

            entity.HasOne(d => d.Account).WithMany(p => p.Events)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__event__account_i__3A81B327");

            entity.HasOne(d => d.EventType).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventTypeId)
                .HasConstraintName("FK__event__event_typ__3C69FB99");

            entity.HasOne(d => d.Venue).WithMany(p => p.Events)
                .HasForeignKey(d => d.VenueId)
                .HasConstraintName("FK__event__venue_id__3B75D760");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.EventTypeId).HasName("PK__event_ty__BB84C6F3229FC8A6");

            entity.ToTable("event_type", "Events");

            entity.Property(e => e.EventTypeId).HasColumnName("event_type_id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__genre__18428D4296928597");

            entity.ToTable("genre", "Shows");

            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.Genre1)
                .HasMaxLength(100)
                .HasColumnName("genre");
        });

        modelBuilder.Entity<IsPartner>(entity =>
        {
            entity.HasKey(e => e.IsPartnerId).HasName("PK__is_partn__858C5CE5884F0197");

            entity.ToTable("is_partner", "Events");

            entity.Property(e => e.IsPartnerId).HasColumnName("is_partner_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.PartnerId).HasColumnName("partner_id");
            entity.Property(e => e.PartnerRoleId).HasColumnName("partner_role_id");

            entity.HasOne(d => d.Event).WithMany(p => p.IsPartners)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__is_partne__event__3F466844");

            entity.HasOne(d => d.Partner).WithMany(p => p.IsPartners)
                .HasForeignKey(d => d.PartnerId)
                .HasConstraintName("FK__is_partne__partn__403A8C7D");

            entity.HasOne(d => d.PartnerRole).WithMany(p => p.IsPartners)
                .HasForeignKey(d => d.PartnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__is_partne__partn__412EB0B6");
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.PartnerId).HasName("PK__partner__576F1B27A78B2DBA");

            entity.ToTable("partner", "Events");

            entity.Property(e => e.PartnerId).HasColumnName("partner_id");
            entity.Property(e => e.PartnerDetails).HasColumnName("partner_details");
            entity.Property(e => e.PartnerName)
                .HasMaxLength(200)
                .HasColumnName("partner_name");
        });

        modelBuilder.Entity<PartnerRole>(entity =>
        {
            entity.HasKey(e => e.PartnerRoleId).HasName("PK__partner___95A5960C72363A61");

            entity.ToTable("partner_role", "Events");

            entity.Property(e => e.PartnerRoleId).HasColumnName("partner_role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Performer>(entity =>
        {
            entity.HasKey(e => e.PerformerId).HasName("PK__performe__E95FC00D775F8FAB");

            entity.ToTable("performer", "Shows");

            entity.Property(e => e.PerformerId).HasColumnName("performer_id");
            entity.Property(e => e.ContactDetail).HasColumnName("contact_detail");
            entity.Property(e => e.Cost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cost");
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .HasColumnName("full_name");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__permissi__E5331AFAA925101C");

            entity.ToTable("permission", "Account");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.Permission1)
                .HasMaxLength(100)
                .HasColumnName("permission");
        });

        modelBuilder.Entity<Required>(entity =>
        {
            entity.HasKey(e => e.RequiredId).HasName("PK__required__44DEE6FC34D3E94F");

            entity.ToTable("required", "Equipment");

            entity.Property(e => e.RequiredId).HasColumnName("required_id");
            entity.Property(e => e.EquipNameId).HasColumnName("equip_name_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.EquipName).WithMany(p => p.Requireds)
                .HasForeignKey(d => d.EquipNameId)
                .HasConstraintName("FK__required__equip___5AEE82B9");

            entity.HasOne(d => d.Event).WithMany(p => p.Requireds)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__required__event___59FA5E80");
        });

        modelBuilder.Entity<Show>(entity =>
        {
            entity.HasKey(e => e.ShowId).HasName("PK__show__2B97D71C5FD5701A");

            entity.ToTable("show", "Shows");

            entity.Property(e => e.ShowId).HasColumnName("show_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.PerformerId).HasColumnName("performer_id");
            entity.Property(e => e.ShowName)
                .HasMaxLength(200)
                .HasColumnName("show_name");

            entity.HasOne(d => d.Event).WithMany(p => p.Shows)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__show__event_id__4AB81AF0");

            entity.HasOne(d => d.Genre).WithMany(p => p.Shows)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__show__genre_id__49C3F6B7");

            entity.HasOne(d => d.Performer).WithMany(p => p.Shows)
                .HasForeignKey(d => d.PerformerId)
                .HasConstraintName("FK__show__performer___48CFD27E");
        });

        modelBuilder.Entity<ShowSchedule>(entity =>
        {
            entity.HasKey(e => e.ShowTimeId).HasName("PK__show_sch__ADED92E341EAF5CB");

            entity.ToTable("show_schedule", "Shows");

            entity.Property(e => e.ShowTimeId).HasColumnName("show_time_id");
            entity.Property(e => e.Datetime).HasColumnName("datetime");
            entity.Property(e => e.EstDuration).HasColumnName("est_duration");
            entity.Property(e => e.ShowId).HasColumnName("show_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");

            entity.HasOne(d => d.Show).WithMany(p => p.ShowSchedules)
                .HasForeignKey(d => d.ShowId)
                .HasConstraintName("FK__show_sche__show___4D94879B");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.VenueId).HasName("PK__venue__82A8BE8DB9307547");

            entity.ToTable("venue", "Events");

            entity.Property(e => e.VenueId).HasColumnName("venue_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Cost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cost");
            entity.Property(e => e.VenueName)
                .HasMaxLength(200)
                .HasColumnName("venue_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
