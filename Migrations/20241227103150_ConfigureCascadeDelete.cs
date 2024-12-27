using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OOP_EventsManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Accounts");

            migrationBuilder.EnsureSchema(
                name: "Employees");

            migrationBuilder.EnsureSchema(
                name: "Equipments");

            migrationBuilder.EnsureSchema(
                name: "Events");

            migrationBuilder.EnsureSchema(
                name: "Shows");

            migrationBuilder.CreateTable(
                name: "equipment_type",
                schema: "Equipments",
                columns: table => new
                {
                    equip_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__equipmen__39BE18CC99F59984", x => x.equip_type_id);
                });

            migrationBuilder.CreateTable(
                name: "event_type",
                schema: "Events",
                columns: table => new
                {
                    event_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__event_ty__BB84C6F3F8A2CB72", x => x.event_type_id);
                });

            migrationBuilder.CreateTable(
                name: "genre",
                schema: "Shows",
                columns: table => new
                {
                    genre_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__genre__18428D4250C90808", x => x.genre_id);
                });

            migrationBuilder.CreateTable(
                name: "performer",
                schema: "Shows",
                columns: table => new
                {
                    performer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    contact_detail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__performe__E95FC00DE498343A", x => x.performer_id);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                schema: "Accounts",
                columns: table => new
                {
                    permission_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    permission = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__permissi__E5331AFAB0725D1E", x => x.permission_id);
                });

            migrationBuilder.CreateTable(
                name: "sponsor_tier",
                schema: "Events",
                columns: table => new
                {
                    sponsor_tier_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tier_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__sponsor___07C90ECDB5A9290C", x => x.sponsor_tier_id);
                });

            migrationBuilder.CreateTable(
                name: "sponsors",
                schema: "Events",
                columns: table => new
                {
                    sponsor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sponsor_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sponsor_details = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__sponsors__BE37D454AB9AAEE9", x => x.sponsor_id);
                });

            migrationBuilder.CreateTable(
                name: "venue",
                schema: "Events",
                columns: table => new
                {
                    venue_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    venue_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__venue__82A8BE8D60990964", x => x.venue_id);
                });

            migrationBuilder.CreateTable(
                name: "equipment_name",
                schema: "Equipments",
                columns: table => new
                {
                    equip_name_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    equip_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    equip_cost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    equip_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__equipmen__03FCFCF4BD16274F", x => x.equip_name_id);
                    table.ForeignKey(
                        name: "FK__equipment__equip__66603565",
                        column: x => x.equip_type_id,
                        principalSchema: "Equipments",
                        principalTable: "equipment_type",
                        principalColumn: "equip_type_id");
                });

            migrationBuilder.CreateTable(
                name: "show",
                schema: "Shows",
                columns: table => new
                {
                    show_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    show_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    performer_id = table.Column<int>(type: "int", nullable: false),
                    genre_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__show__2B97D71C5A592466", x => x.show_id);
                    table.ForeignKey(
                        name: "FK__show__genre_id__5CD6CB2B",
                        column: x => x.genre_id,
                        principalSchema: "Shows",
                        principalTable: "genre",
                        principalColumn: "genre_id");
                    table.ForeignKey(
                        name: "FK__show__performer___5BE2A6F2",
                        column: x => x.performer_id,
                        principalSchema: "Shows",
                        principalTable: "performer",
                        principalColumn: "performer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event",
                schema: "Events",
                columns: table => new
                {
                    event_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    event_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    event_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    expted_attendee = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    event_type_id = table.Column<int>(type: "int", nullable: false),
                    venue_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__event__2370F7273B7271C1", x => x.event_id);
                    table.ForeignKey(
                        name: "FK__event__event_typ__49C3F6B7",
                        column: x => x.event_type_id,
                        principalSchema: "Events",
                        principalTable: "event_type",
                        principalColumn: "event_type_id");
                    table.ForeignKey(
                        name: "FK__event__venue_id__4AB81AF0",
                        column: x => x.venue_id,
                        principalSchema: "Events",
                        principalTable: "venue",
                        principalColumn: "venue_id");
                });

            migrationBuilder.CreateTable(
                name: "equipment",
                schema: "Equipments",
                columns: table => new
                {
                    equipment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    equip_name_id = table.Column<int>(type: "int", nullable: false),
                    condition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__equipmen__197068AFC32AA4C1", x => x.equipment_id);
                    table.ForeignKey(
                        name: "FK__equipment__equip__693CA210",
                        column: x => x.equip_name_id,
                        principalSchema: "Equipments",
                        principalTable: "equipment_name",
                        principalColumn: "equip_name_id");
                });

            migrationBuilder.CreateTable(
                name: "is_sponsor",
                schema: "Events",
                columns: table => new
                {
                    is_sponsor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    event_id = table.Column<int>(type: "int", nullable: false),
                    sponsor_id = table.Column<int>(type: "int", nullable: false),
                    sponsor_tier_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__is_spons__01D739A985380B28", x => x.is_sponsor_id);
                    table.ForeignKey(
                        name: "FK__is_sponso__event__534D60F1",
                        column: x => x.event_id,
                        principalSchema: "Events",
                        principalTable: "event",
                        principalColumn: "event_id");
                    table.ForeignKey(
                        name: "FK__is_sponso__spons__5441852A",
                        column: x => x.sponsor_id,
                        principalSchema: "Events",
                        principalTable: "sponsors",
                        principalColumn: "sponsor_id");
                    table.ForeignKey(
                        name: "FK__is_sponso__spons__5535A963",
                        column: x => x.sponsor_tier_id,
                        principalSchema: "Events",
                        principalTable: "sponsor_tier",
                        principalColumn: "sponsor_tier_id");
                });

            migrationBuilder.CreateTable(
                name: "required",
                schema: "Equipments",
                columns: table => new
                {
                    required_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    event_id = table.Column<int>(type: "int", nullable: false),
                    equip_name_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__required__44DEE6FC698916CF", x => x.required_id);
                    table.ForeignKey(
                        name: "FK__required__equip___6E01572D",
                        column: x => x.equip_name_id,
                        principalSchema: "Equipments",
                        principalTable: "equipment_name",
                        principalColumn: "equip_name_id");
                    table.ForeignKey(
                        name: "FK__required__event___6D0D32F4",
                        column: x => x.event_id,
                        principalSchema: "Events",
                        principalTable: "event",
                        principalColumn: "event_id");
                });

            migrationBuilder.CreateTable(
                name: "show_schedule",
                schema: "Shows",
                columns: table => new
                {
                    show_time_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    est_duration = table.Column<int>(type: "int", nullable: false),
                    show_id = table.Column<int>(type: "int", nullable: false),
                    event_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__show_sch__ADED92E382AE4F22", x => x.show_time_id);
                    table.ForeignKey(
                        name: "FK__show_sche__event__619B8048",
                        column: x => x.event_id,
                        principalSchema: "Events",
                        principalTable: "event",
                        principalColumn: "event_id");
                    table.ForeignKey(
                        name: "FK__show_sche__show___60A75C0F",
                        column: x => x.show_id,
                        principalSchema: "Shows",
                        principalTable: "show",
                        principalColumn: "show_id");
                });

            migrationBuilder.CreateTable(
                name: "account",
                schema: "Accounts",
                columns: table => new
                {
                    account_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    permission_id = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__account__46A222CD9FCE5CC6", x => x.account_id);
                    table.ForeignKey(
                        name: "FK__account__permiss__3E52440B",
                        column: x => x.permission_id,
                        principalSchema: "Accounts",
                        principalTable: "permission",
                        principalColumn: "permission_id");
                });

            migrationBuilder.CreateTable(
                name: "engaged",
                schema: "Employees",
                columns: table => new
                {
                    engaged_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    account_id = table.Column<int>(type: "int", nullable: false),
                    event_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__engaged__EAEA56F0518F081B", x => x.engaged_id);
                    table.ForeignKey(
                        name: "FK__engaged__account__71D1E811",
                        column: x => x.account_id,
                        principalSchema: "Accounts",
                        principalTable: "account",
                        principalColumn: "account_id");
                    table.ForeignKey(
                        name: "FK__engaged__event_i__72C60C4A",
                        column: x => x.event_id,
                        principalSchema: "Events",
                        principalTable: "event",
                        principalColumn: "event_id");
                });

            migrationBuilder.CreateTable(
                name: "employee",
                schema: "Employees",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    contact = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__employee__C52E0BA811CFE5B9", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "employee_role",
                schema: "Employees",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    salary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    manager_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__employee__760965CC7ACD693A", x => x.role_id);
                    table.ForeignKey(
                        name: "FK_employee_manager",
                        column: x => x.manager_id,
                        principalSchema: "Employees",
                        principalTable: "employee",
                        principalColumn: "employee_id");
                });

            migrationBuilder.CreateTable(
                name: "need",
                schema: "Employees",
                columns: table => new
                {
                    need_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    event_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__need__56F592345CFDDA4E", x => x.need_id);
                    table.ForeignKey(
                        name: "FK__need__event_id__4F7CD00D",
                        column: x => x.event_id,
                        principalSchema: "Events",
                        principalTable: "event",
                        principalColumn: "event_id");
                    table.ForeignKey(
                        name: "FK__need__role_id__4E88ABD4",
                        column: x => x.role_id,
                        principalSchema: "Employees",
                        principalTable: "employee_role",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_employee_id",
                schema: "Accounts",
                table: "account",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_account_permission_id",
                schema: "Accounts",
                table: "account",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_role_id",
                schema: "Employees",
                table: "employee",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_role_manager_id",
                schema: "Employees",
                table: "employee_role",
                column: "manager_id");

            migrationBuilder.CreateIndex(
                name: "IX_engaged_event_id",
                schema: "Employees",
                table: "engaged",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "UQ__engaged__24952DBE6CDBB78C",
                schema: "Employees",
                table: "engaged",
                columns: new[] { "account_id", "event_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_equipment_equip_name_id",
                schema: "Equipments",
                table: "equipment",
                column: "equip_name_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_name_equip_type_id",
                schema: "Equipments",
                table: "equipment_name",
                column: "equip_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_event_type_id",
                schema: "Events",
                table: "event",
                column: "event_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_venue_id",
                schema: "Events",
                table: "event",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "IX_is_sponsor_sponsor_id",
                schema: "Events",
                table: "is_sponsor",
                column: "sponsor_id");

            migrationBuilder.CreateIndex(
                name: "IX_is_sponsor_sponsor_tier_id",
                schema: "Events",
                table: "is_sponsor",
                column: "sponsor_tier_id");

            migrationBuilder.CreateIndex(
                name: "UQ__is_spons__B494436DE506E4E0",
                schema: "Events",
                table: "is_sponsor",
                columns: new[] { "event_id", "sponsor_id", "sponsor_tier_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_need_event_id",
                schema: "Employees",
                table: "need",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "UQ__need__143E6ABF7DC6B535",
                schema: "Employees",
                table: "need",
                columns: new[] { "role_id", "event_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_required_equip_name_id",
                schema: "Equipments",
                table: "required",
                column: "equip_name_id");

            migrationBuilder.CreateIndex(
                name: "UQ__required__734F38E9FD3CBBF6",
                schema: "Equipments",
                table: "required",
                columns: new[] { "event_id", "equip_name_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_show_genre_id",
                schema: "Shows",
                table: "show",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_show_performer_id",
                schema: "Shows",
                table: "show",
                column: "performer_id");

            migrationBuilder.CreateIndex(
                name: "IX_show_schedule_event_id",
                schema: "Shows",
                table: "show_schedule",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "UQ__show_sch__49A0D86FAF7F81B0",
                schema: "Shows",
                table: "show_schedule",
                columns: new[] { "show_id", "event_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__account__employe__3F466844",
                schema: "Accounts",
                table: "account",
                column: "employee_id",
                principalSchema: "Employees",
                principalTable: "employee",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employee_role",
                schema: "Employees",
                table: "employee",
                column: "role_id",
                principalSchema: "Employees",
                principalTable: "employee_role",
                principalColumn: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_manager",
                schema: "Employees",
                table: "employee_role");

            migrationBuilder.DropTable(
                name: "engaged",
                schema: "Employees");

            migrationBuilder.DropTable(
                name: "equipment",
                schema: "Equipments");

            migrationBuilder.DropTable(
                name: "is_sponsor",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "need",
                schema: "Employees");

            migrationBuilder.DropTable(
                name: "required",
                schema: "Equipments");

            migrationBuilder.DropTable(
                name: "show_schedule",
                schema: "Shows");

            migrationBuilder.DropTable(
                name: "account",
                schema: "Accounts");

            migrationBuilder.DropTable(
                name: "sponsors",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "sponsor_tier",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "equipment_name",
                schema: "Equipments");

            migrationBuilder.DropTable(
                name: "event",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "show",
                schema: "Shows");

            migrationBuilder.DropTable(
                name: "permission",
                schema: "Accounts");

            migrationBuilder.DropTable(
                name: "equipment_type",
                schema: "Equipments");

            migrationBuilder.DropTable(
                name: "event_type",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "venue",
                schema: "Events");

            migrationBuilder.DropTable(
                name: "genre",
                schema: "Shows");

            migrationBuilder.DropTable(
                name: "performer",
                schema: "Shows");

            migrationBuilder.DropTable(
                name: "employee",
                schema: "Employees");

            migrationBuilder.DropTable(
                name: "employee_role",
                schema: "Employees");
        }
    }
}
