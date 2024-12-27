﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OOP_EventsManagementSystem.Model;

#nullable disable

namespace OOP_EventsManagementSystem.Migrations
{
    [DbContext(typeof(EventManagementDbContext))]
    [Migration("20241227103150_ConfigureCascadeDelete")]
    partial class ConfigureCascadeDelete
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("account_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("email");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("password");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int")
                        .HasColumnName("permission_id");

                    b.HasKey("AccountId")
                        .HasName("PK__account__46A222CD9FCE5CC6");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PermissionId");

                    b.ToTable("account", "Accounts");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("employee_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<string>("Contact")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("contact");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("full_name");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.HasKey("EmployeeId")
                        .HasName("PK__employee__C52E0BA811CFE5B9");

                    b.HasIndex("RoleId");

                    b.ToTable("employee", "Employees");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.EmployeeRole", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int")
                        .HasColumnName("manager_id");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("role_name");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("salary");

                    b.HasKey("RoleId")
                        .HasName("PK__employee__760965CC7ACD693A");

                    b.HasIndex("ManagerId");

                    b.ToTable("employee_role", "Employees");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Engaged", b =>
                {
                    b.Property<int>("EngagedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("engaged_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EngagedId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int")
                        .HasColumnName("account_id");

                    b.Property<int>("EventId")
                        .HasColumnType("int")
                        .HasColumnName("event_id");

                    b.HasKey("EngagedId")
                        .HasName("PK__engaged__EAEA56F0518F081B");

                    b.HasIndex("EventId");

                    b.HasIndex(new[] { "AccountId", "EventId" }, "UQ__engaged__24952DBE6CDBB78C")
                        .IsUnique();

                    b.ToTable("engaged", "Employees");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Equipment", b =>
                {
                    b.Property<int>("EquipmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("equipment_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EquipmentId"));

                    b.Property<string>("Condition")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("condition");

                    b.Property<int>("EquipNameId")
                        .HasColumnType("int")
                        .HasColumnName("equip_name_id");

                    b.HasKey("EquipmentId")
                        .HasName("PK__equipmen__197068AFC32AA4C1");

                    b.HasIndex("EquipNameId");

                    b.ToTable("equipment", "Equipments");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.EquipmentName", b =>
                {
                    b.Property<int>("EquipNameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("equip_name_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EquipNameId"));

                    b.Property<decimal>("EquipCost")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("equip_cost");

                    b.Property<string>("EquipName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("equip_name");

                    b.Property<int>("EquipTypeId")
                        .HasColumnType("int")
                        .HasColumnName("equip_type_id");

                    b.HasKey("EquipNameId")
                        .HasName("PK__equipmen__03FCFCF4BD16274F");

                    b.HasIndex("EquipTypeId");

                    b.ToTable("equipment_name", "Equipments");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.EquipmentType", b =>
                {
                    b.Property<int>("EquipTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("equip_type_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EquipTypeId"));

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("type_name");

                    b.HasKey("EquipTypeId")
                        .HasName("PK__equipmen__39BE18CC99F59984");

                    b.ToTable("equipment_type", "Equipments");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("event_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"));

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("end_date");

                    b.Property<string>("EventDescription")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("event_description");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("event_name");

                    b.Property<int>("EventTypeId")
                        .HasColumnType("int")
                        .HasColumnName("event_type_id");

                    b.Property<int>("ExptedAttendee")
                        .HasColumnType("int")
                        .HasColumnName("expted_attendee");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("start_date");

                    b.Property<int>("VenueId")
                        .HasColumnType("int")
                        .HasColumnName("venue_id");

                    b.HasKey("EventId")
                        .HasName("PK__event__2370F7273B7271C1");

                    b.HasIndex("EventTypeId");

                    b.HasIndex("VenueId");

                    b.ToTable("event", "Events");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.EventType", b =>
                {
                    b.Property<int>("EventTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("event_type_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventTypeId"));

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("type_name");

                    b.HasKey("EventTypeId")
                        .HasName("PK__event_ty__BB84C6F3F8A2CB72");

                    b.ToTable("event_type", "Events");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Genre", b =>
                {
                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("genre_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GenreId"));

                    b.Property<string>("Genre1")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("genre");

                    b.HasKey("GenreId")
                        .HasName("PK__genre__18428D4250C90808");

                    b.ToTable("genre", "Shows");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.IsSponsor", b =>
                {
                    b.Property<int>("IsSponsorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("is_sponsor_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IsSponsorId"));

                    b.Property<int>("EventId")
                        .HasColumnType("int")
                        .HasColumnName("event_id");

                    b.Property<int>("SponsorId")
                        .HasColumnType("int")
                        .HasColumnName("sponsor_id");

                    b.Property<int>("SponsorTierId")
                        .HasColumnType("int")
                        .HasColumnName("sponsor_tier_id");

                    b.HasKey("IsSponsorId")
                        .HasName("PK__is_spons__01D739A985380B28");

                    b.HasIndex("SponsorId");

                    b.HasIndex("SponsorTierId");

                    b.HasIndex(new[] { "EventId", "SponsorId", "SponsorTierId" }, "UQ__is_spons__B494436DE506E4E0")
                        .IsUnique();

                    b.ToTable("is_sponsor", "Events");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Need", b =>
                {
                    b.Property<int>("NeedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("need_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NeedId"));

                    b.Property<int>("EventId")
                        .HasColumnType("int")
                        .HasColumnName("event_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.HasKey("NeedId")
                        .HasName("PK__need__56F592345CFDDA4E");

                    b.HasIndex("EventId");

                    b.HasIndex(new[] { "RoleId", "EventId" }, "UQ__need__143E6ABF7DC6B535")
                        .IsUnique();

                    b.ToTable("need", "Employees");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Performer", b =>
                {
                    b.Property<int>("PerformerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("performer_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PerformerId"));

                    b.Property<string>("ContactDetail")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("contact_detail");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("full_name");

                    b.HasKey("PerformerId")
                        .HasName("PK__performe__E95FC00DE498343A");

                    b.ToTable("performer", "Shows");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("permission_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PermissionId"));

                    b.Property<string>("Permission1")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("permission");

                    b.HasKey("PermissionId")
                        .HasName("PK__permissi__E5331AFAB0725D1E");

                    b.ToTable("permission", "Accounts");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Required", b =>
                {
                    b.Property<int>("RequiredId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("required_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequiredId"));

                    b.Property<int>("EquipNameId")
                        .HasColumnType("int")
                        .HasColumnName("equip_name_id");

                    b.Property<int>("EventId")
                        .HasColumnType("int")
                        .HasColumnName("event_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.HasKey("RequiredId")
                        .HasName("PK__required__44DEE6FC698916CF");

                    b.HasIndex("EquipNameId");

                    b.HasIndex(new[] { "EventId", "EquipNameId" }, "UQ__required__734F38E9FD3CBBF6")
                        .IsUnique();

                    b.ToTable("required", "Equipments");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Show", b =>
                {
                    b.Property<int>("ShowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("show_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShowId"));

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("cost");

                    b.Property<int>("GenreId")
                        .HasColumnType("int")
                        .HasColumnName("genre_id");

                    b.Property<int>("PerformerId")
                        .HasColumnType("int")
                        .HasColumnName("performer_id");

                    b.Property<string>("ShowName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("show_name");

                    b.HasKey("ShowId")
                        .HasName("PK__show__2B97D71C5A592466");

                    b.HasIndex("GenreId");

                    b.HasIndex("PerformerId");

                    b.ToTable("show", "Shows");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.ShowSchedule", b =>
                {
                    b.Property<int>("ShowTimeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("show_time_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShowTimeId"));

                    b.Property<int>("EstDuration")
                        .HasColumnType("int")
                        .HasColumnName("est_duration");

                    b.Property<int>("EventId")
                        .HasColumnType("int")
                        .HasColumnName("event_id");

                    b.Property<int>("ShowId")
                        .HasColumnType("int")
                        .HasColumnName("show_id");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("start_date");

                    b.HasKey("ShowTimeId")
                        .HasName("PK__show_sch__ADED92E382AE4F22");

                    b.HasIndex("EventId");

                    b.HasIndex(new[] { "ShowId", "EventId" }, "UQ__show_sch__49A0D86FAF7F81B0")
                        .IsUnique();

                    b.ToTable("show_schedule", "Shows");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Sponsor", b =>
                {
                    b.Property<int>("SponsorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("sponsor_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SponsorId"));

                    b.Property<string>("SponsorDetails")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("sponsor_details");

                    b.Property<string>("SponsorName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("sponsor_name");

                    b.HasKey("SponsorId")
                        .HasName("PK__sponsors__BE37D454AB9AAEE9");

                    b.ToTable("sponsors", "Events");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.SponsorTier", b =>
                {
                    b.Property<int>("SponsorTierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("sponsor_tier_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SponsorTierId"));

                    b.Property<string>("TierName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("tier_name");

                    b.HasKey("SponsorTierId")
                        .HasName("PK__sponsor___07C90ECDB5A9290C");

                    b.ToTable("sponsor_tier", "Events");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Venue", b =>
                {
                    b.Property<int>("VenueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("venue_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VenueId"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("address");

                    b.Property<int>("Capacity")
                        .HasColumnType("int")
                        .HasColumnName("capacity");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("cost");

                    b.Property<string>("VenueName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("venue_name");

                    b.HasKey("VenueId")
                        .HasName("PK__venue__82A8BE8D60990964");

                    b.ToTable("venue", "Events");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Account", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.Employee", "Employee")
                        .WithMany("Accounts")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__account__employe__3F466844");

                    b.HasOne("OOP_EventsManagementSystem.Model.Permission", "Permission")
                        .WithMany("Accounts")
                        .HasForeignKey("PermissionId")
                        .IsRequired()
                        .HasConstraintName("FK__account__permiss__3E52440B");

                    b.Navigation("Employee");

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Employee", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.EmployeeRole", "Role")
                        .WithMany("Employees")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK_employee_role");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.EmployeeRole", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.Employee", "Manager")
                        .WithMany("EmployeeRoles")
                        .HasForeignKey("ManagerId")
                        .HasConstraintName("FK_employee_manager");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Engaged", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.Account", "Account")
                        .WithMany("Engageds")
                        .HasForeignKey("AccountId")
                        .IsRequired()
                        .HasConstraintName("FK__engaged__account__71D1E811");

                    b.HasOne("OOP_EventsManagementSystem.Model.Event", "Event")
                        .WithMany("Engageds")
                        .HasForeignKey("EventId")
                        .IsRequired()
                        .HasConstraintName("FK__engaged__event_i__72C60C4A");

                    b.Navigation("Account");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Equipment", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.EquipmentName", "EquipName")
                        .WithMany("Equipment")
                        .HasForeignKey("EquipNameId")
                        .IsRequired()
                        .HasConstraintName("FK__equipment__equip__693CA210");

                    b.Navigation("EquipName");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.EquipmentName", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.EquipmentType", "EquipType")
                        .WithMany("EquipmentNames")
                        .HasForeignKey("EquipTypeId")
                        .IsRequired()
                        .HasConstraintName("FK__equipment__equip__66603565");

                    b.Navigation("EquipType");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Event", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.EventType", "EventType")
                        .WithMany("Events")
                        .HasForeignKey("EventTypeId")
                        .IsRequired()
                        .HasConstraintName("FK__event__event_typ__49C3F6B7");

                    b.HasOne("OOP_EventsManagementSystem.Model.Venue", "Venue")
                        .WithMany("Events")
                        .HasForeignKey("VenueId")
                        .IsRequired()
                        .HasConstraintName("FK__event__venue_id__4AB81AF0");

                    b.Navigation("EventType");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.IsSponsor", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.Event", "Event")
                        .WithMany("IsSponsors")
                        .HasForeignKey("EventId")
                        .IsRequired()
                        .HasConstraintName("FK__is_sponso__event__534D60F1");

                    b.HasOne("OOP_EventsManagementSystem.Model.Sponsor", "Sponsor")
                        .WithMany("IsSponsors")
                        .HasForeignKey("SponsorId")
                        .IsRequired()
                        .HasConstraintName("FK__is_sponso__spons__5441852A");

                    b.HasOne("OOP_EventsManagementSystem.Model.SponsorTier", "SponsorTier")
                        .WithMany("IsSponsors")
                        .HasForeignKey("SponsorTierId")
                        .IsRequired()
                        .HasConstraintName("FK__is_sponso__spons__5535A963");

                    b.Navigation("Event");

                    b.Navigation("Sponsor");

                    b.Navigation("SponsorTier");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Need", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.Event", "Event")
                        .WithMany("Needs")
                        .HasForeignKey("EventId")
                        .IsRequired()
                        .HasConstraintName("FK__need__event_id__4F7CD00D");

                    b.HasOne("OOP_EventsManagementSystem.Model.EmployeeRole", "Role")
                        .WithMany("Needs")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK__need__role_id__4E88ABD4");

                    b.Navigation("Event");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Required", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.EquipmentName", "EquipName")
                        .WithMany("Requireds")
                        .HasForeignKey("EquipNameId")
                        .IsRequired()
                        .HasConstraintName("FK__required__equip___6E01572D");

                    b.HasOne("OOP_EventsManagementSystem.Model.Event", "Event")
                        .WithMany("Requireds")
                        .HasForeignKey("EventId")
                        .IsRequired()
                        .HasConstraintName("FK__required__event___6D0D32F4");

                    b.Navigation("EquipName");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Show", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.Genre", "Genre")
                        .WithMany("Shows")
                        .HasForeignKey("GenreId")
                        .IsRequired()
                        .HasConstraintName("FK__show__genre_id__5CD6CB2B");

                    b.HasOne("OOP_EventsManagementSystem.Model.Performer", "Performer")
                        .WithMany("Shows")
                        .HasForeignKey("PerformerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__show__performer___5BE2A6F2");

                    b.Navigation("Genre");

                    b.Navigation("Performer");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.ShowSchedule", b =>
                {
                    b.HasOne("OOP_EventsManagementSystem.Model.Event", "Event")
                        .WithMany("ShowSchedules")
                        .HasForeignKey("EventId")
                        .IsRequired()
                        .HasConstraintName("FK__show_sche__event__619B8048");

                    b.HasOne("OOP_EventsManagementSystem.Model.Show", "Show")
                        .WithMany("ShowSchedules")
                        .HasForeignKey("ShowId")
                        .IsRequired()
                        .HasConstraintName("FK__show_sche__show___60A75C0F");

                    b.Navigation("Event");

                    b.Navigation("Show");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Account", b =>
                {
                    b.Navigation("Engageds");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Employee", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("EmployeeRoles");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.EmployeeRole", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Needs");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.EquipmentName", b =>
                {
                    b.Navigation("Equipment");

                    b.Navigation("Requireds");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.EquipmentType", b =>
                {
                    b.Navigation("EquipmentNames");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Event", b =>
                {
                    b.Navigation("Engageds");

                    b.Navigation("IsSponsors");

                    b.Navigation("Needs");

                    b.Navigation("Requireds");

                    b.Navigation("ShowSchedules");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.EventType", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Genre", b =>
                {
                    b.Navigation("Shows");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Performer", b =>
                {
                    b.Navigation("Shows");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Permission", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Show", b =>
                {
                    b.Navigation("ShowSchedules");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Sponsor", b =>
                {
                    b.Navigation("IsSponsors");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.SponsorTier", b =>
                {
                    b.Navigation("IsSponsors");
                });

            modelBuilder.Entity("OOP_EventsManagementSystem.Model.Venue", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
