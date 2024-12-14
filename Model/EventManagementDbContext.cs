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

    public virtual DbSet<IsSponsor> IsSponsors { get; set; }

    public virtual DbSet<Need> Needs { get; set; }

    public virtual DbSet<Performer> Performers { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Required> Requireds { get; set; }

    public virtual DbSet<Show> Shows { get; set; }

    public virtual DbSet<ShowSchedule> ShowSchedules { get; set; }

    public virtual DbSet<Sponsor> Sponsors { get; set; }

    public virtual DbSet<SponsorTier> SponsorTiers { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=EventManagementDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__account__46A222CD9FCE5CC6");

            entity.ToTable("account", "Accounts");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__account__employe__3F466844");

            entity.HasOne(d => d.Permission).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__account__permiss__3E52440B");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__employee__C52E0BA811CFE5B9");

            entity.ToTable("employee", "Employees");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Contact)
                .HasMaxLength(50)
                .HasColumnName("contact");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_employee_role");
        });

        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__employee__760965CC7ACD693A");

            entity.ToTable("employee_role", "Employees");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");
            entity.Property(e => e.Salary)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("salary");

            entity.HasOne(d => d.Manager).WithMany(p => p.EmployeeRoles)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_employee_manager");
        });

        modelBuilder.Entity<Engaged>(entity =>
        {
            entity.HasKey(e => e.EngagedId).HasName("PK__engaged__EAEA56F0518F081B");

            entity.ToTable("engaged", "Employees");

            entity.HasIndex(e => new { e.AccountId, e.EventId }, "UQ__engaged__24952DBE6CDBB78C").IsUnique();

            entity.Property(e => e.EngagedId).HasColumnName("engaged_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");

            entity.HasOne(d => d.Account).WithMany(p => p.Engageds)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__engaged__account__71D1E811");

            entity.HasOne(d => d.Event).WithMany(p => p.Engageds)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__engaged__event_i__72C60C4A");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("PK__equipmen__197068AFC32AA4C1");

            entity.ToTable("equipment", "Equipments");

            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.Condition)
                .HasMaxLength(100)
                .HasColumnName("condition");
            entity.Property(e => e.EquipNameId).HasColumnName("equip_name_id");

            entity.HasOne(d => d.EquipName).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.EquipNameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__equipment__equip__693CA210");
        });

        modelBuilder.Entity<EquipmentName>(entity =>
        {
            entity.HasKey(e => e.EquipNameId).HasName("PK__equipmen__03FCFCF4BD16274F");

            entity.ToTable("equipment_name", "Equipments");

            entity.Property(e => e.EquipNameId).HasColumnName("equip_name_id");
            entity.Property(e => e.EquipCost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("equip_cost");
            entity.Property(e => e.EquipName)
                .HasMaxLength(100)
                .HasColumnName("equip_name");
            entity.Property(e => e.EquipTypeId).HasColumnName("equip_type_id");

            entity.HasOne(d => d.EquipType).WithMany(p => p.EquipmentNames)
                .HasForeignKey(d => d.EquipTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__equipment__equip__66603565");
        });

        modelBuilder.Entity<EquipmentType>(entity =>
        {
            entity.HasKey(e => e.EquipTypeId).HasName("PK__equipmen__39BE18CC99F59984");

            entity.ToTable("equipment_type", "Equipments");

            entity.Property(e => e.EquipTypeId).HasColumnName("equip_type_id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__event__2370F7273B7271C1");

            entity.ToTable("event", "Events");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.EventDescription).HasColumnName("event_description");
            entity.Property(e => e.EventName)
                .HasMaxLength(100)
                .HasColumnName("event_name");
            entity.Property(e => e.EventTypeId).HasColumnName("event_type_id");
            entity.Property(e => e.ExptedAttendee).HasColumnName("expted_attendee");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.VenueId).HasColumnName("venue_id");

            entity.HasOne(d => d.EventType).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event__event_typ__49C3F6B7");

            entity.HasOne(d => d.Venue).WithMany(p => p.Events)
                .HasForeignKey(d => d.VenueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__event__venue_id__4AB81AF0");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.EventTypeId).HasName("PK__event_ty__BB84C6F3F8A2CB72");

            entity.ToTable("event_type", "Events");

            entity.Property(e => e.EventTypeId).HasColumnName("event_type_id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__genre__18428D4250C90808");

            entity.ToTable("genre", "Shows");

            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.Genre1)
                .HasMaxLength(100)
                .HasColumnName("genre");
        });

        modelBuilder.Entity<IsSponsor>(entity =>
        {
            entity.HasKey(e => e.IsSponsorId).HasName("PK__is_spons__01D739A985380B28");

            entity.ToTable("is_sponsor", "Events");

            entity.HasIndex(e => new { e.EventId, e.SponsorId, e.SponsorTierId }, "UQ__is_spons__B494436DE506E4E0").IsUnique();

            entity.Property(e => e.IsSponsorId).HasColumnName("is_sponsor_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.SponsorId).HasColumnName("sponsor_id");
            entity.Property(e => e.SponsorTierId).HasColumnName("sponsor_tier_id");

            entity.HasOne(d => d.Event).WithMany(p => p.IsSponsors)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__is_sponso__event__534D60F1");

            entity.HasOne(d => d.Sponsor).WithMany(p => p.IsSponsors)
                .HasForeignKey(d => d.SponsorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__is_sponso__spons__5441852A");

            entity.HasOne(d => d.SponsorTier).WithMany(p => p.IsSponsors)
                .HasForeignKey(d => d.SponsorTierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__is_sponso__spons__5535A963");
        });

        modelBuilder.Entity<Need>(entity =>
        {
            entity.HasKey(e => e.NeedId).HasName("PK__need__56F592345CFDDA4E");

            entity.ToTable("need", "Employees");

            entity.HasIndex(e => new { e.RoleId, e.EventId }, "UQ__need__143E6ABF7DC6B535").IsUnique();

            entity.Property(e => e.NeedId).HasColumnName("need_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Event).WithMany(p => p.Needs)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__need__event_id__4F7CD00D");

            entity.HasOne(d => d.Role).WithMany(p => p.Needs)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__need__role_id__4E88ABD4");
        });

        modelBuilder.Entity<Performer>(entity =>
        {
            entity.HasKey(e => e.PerformerId).HasName("PK__performe__E95FC00DE498343A");

            entity.ToTable("performer", "Shows");

            entity.Property(e => e.PerformerId).HasColumnName("performer_id");
            entity.Property(e => e.ContactDetail)
                .HasMaxLength(50)
                .HasColumnName("contact_detail");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__permissi__E5331AFAB0725D1E");

            entity.ToTable("permission", "Accounts");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.Permission1)
                .HasMaxLength(100)
                .HasColumnName("permission");
        });

        modelBuilder.Entity<Required>(entity =>
        {
            entity.HasKey(e => e.RequiredId).HasName("PK__required__44DEE6FC698916CF");

            entity.ToTable("required", "Equipments");

            entity.HasIndex(e => new { e.EventId, e.EquipNameId }, "UQ__required__734F38E9FD3CBBF6").IsUnique();

            entity.Property(e => e.RequiredId).HasColumnName("required_id");
            entity.Property(e => e.EquipNameId).HasColumnName("equip_name_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.EquipName).WithMany(p => p.Requireds)
                .HasForeignKey(d => d.EquipNameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__required__equip___6E01572D");

            entity.HasOne(d => d.Event).WithMany(p => p.Requireds)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__required__event___6D0D32F4");
        });

        modelBuilder.Entity<Show>(entity =>
        {
            entity.HasKey(e => e.ShowId).HasName("PK__show__2B97D71C5A592466");

            entity.ToTable("show", "Shows");

            entity.Property(e => e.ShowId).HasColumnName("show_id");
            entity.Property(e => e.Cost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cost");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.PerformerId).HasColumnName("performer_id");
            entity.Property(e => e.ShowName)
                .HasMaxLength(100)
                .HasColumnName("show_name");

            entity.HasOne(d => d.Genre).WithMany(p => p.Shows)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__show__genre_id__5CD6CB2B");

            entity.HasOne(d => d.Performer).WithMany(p => p.Shows)
                .HasForeignKey(d => d.PerformerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__show__performer___5BE2A6F2");
        });

        modelBuilder.Entity<ShowSchedule>(entity =>
        {
            entity.HasKey(e => e.ShowTimeId).HasName("PK__show_sch__ADED92E382AE4F22");

            entity.ToTable("show_schedule", "Shows");

            entity.HasIndex(e => new { e.ShowId, e.EventId }, "UQ__show_sch__49A0D86FAF7F81B0").IsUnique();

            entity.Property(e => e.ShowTimeId).HasColumnName("show_time_id");
            entity.Property(e => e.EstDuration).HasColumnName("est_duration");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.ShowId).HasColumnName("show_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Event).WithMany(p => p.ShowSchedules)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__show_sche__event__619B8048");

            entity.HasOne(d => d.Show).WithMany(p => p.ShowSchedules)
                .HasForeignKey(d => d.ShowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__show_sche__show___60A75C0F");
        });

        modelBuilder.Entity<Sponsor>(entity =>
        {
            entity.HasKey(e => e.SponsorId).HasName("PK__sponsors__BE37D454AB9AAEE9");

            entity.ToTable("sponsors", "Events");

            entity.Property(e => e.SponsorId).HasColumnName("sponsor_id");
            entity.Property(e => e.SponsorDetails).HasColumnName("sponsor_details");
            entity.Property(e => e.SponsorName)
                .HasMaxLength(100)
                .HasColumnName("sponsor_name");
        });

        modelBuilder.Entity<SponsorTier>(entity =>
        {
            entity.HasKey(e => e.SponsorTierId).HasName("PK__sponsor___07C90ECDB5A9290C");

            entity.ToTable("sponsor_tier", "Events");

            entity.Property(e => e.SponsorTierId).HasColumnName("sponsor_tier_id");
            entity.Property(e => e.TierName)
                .HasMaxLength(100)
                .HasColumnName("tier_name");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.VenueId).HasName("PK__venue__82A8BE8D60990964");

            entity.ToTable("venue", "Events");

            entity.Property(e => e.VenueId).HasColumnName("venue_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Cost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cost");
            entity.Property(e => e.VenueName)
                .HasMaxLength(100)
                .HasColumnName("venue_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
