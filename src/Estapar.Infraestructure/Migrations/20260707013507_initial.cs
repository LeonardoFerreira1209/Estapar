using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estapar.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Garages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Garages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Garages_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lanes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LaneType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lanes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lanes_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceTables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    GracePeriodMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 30),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceTables_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Traffics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LicensePlate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LaneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Error = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traffics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Traffics_Lanes_LaneId",
                        column: x => x.LaneId,
                        principalTable: "Lanes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParkedVehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LicensePlate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EntryTrafficId = table.Column<Guid>(type: "uuid", nullable: false),
                    GarageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkedVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkedVehicles_Garages_GarageId",
                        column: x => x.GarageId,
                        principalTable: "Garages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParkedVehicles_Traffics_EntryTrafficId",
                        column: x => x.EntryTrafficId,
                        principalTable: "Traffics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryTrafficId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExitTrafficId = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StayDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Traffics_EntryTrafficId",
                        column: x => x.EntryTrafficId,
                        principalTable: "Traffics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Traffics_ExitTrafficId",
                        column: x => x.ExitTrafficId,
                        principalTable: "Traffics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Garages_Created",
                table: "Garages",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Garages_ParkId",
                table: "Garages",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_Lanes_Created",
                table: "Lanes",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Lanes_Name",
                table: "Lanes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Lanes_ParkId",
                table: "Lanes",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_Lanes_Status",
                table: "Lanes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_EntryTrafficId",
                table: "ParkedVehicles",
                column: "EntryTrafficId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_GarageId",
                table: "ParkedVehicles",
                column: "GarageId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_LicensePlate",
                table: "ParkedVehicles",
                column: "LicensePlate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parks_Created",
                table: "Parks",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Parks_Name",
                table: "Parks",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTables_Created",
                table: "PriceTables",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTables_ParkId",
                table: "PriceTables",
                column: "ParkId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Traffics_Created",
                table: "Traffics",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Traffics_Date",
                table: "Traffics",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Traffics_LaneId",
                table: "Traffics",
                column: "LaneId");

            migrationBuilder.CreateIndex(
                name: "IX_Traffics_LicensePlate",
                table: "Traffics",
                column: "LicensePlate");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Created",
                table: "Transactions",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_EntryTrafficId",
                table: "Transactions",
                column: "EntryTrafficId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ExitTrafficId",
                table: "Transactions",
                column: "ExitTrafficId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkedVehicles");

            migrationBuilder.DropTable(
                name: "PriceTables");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Garages");

            migrationBuilder.DropTable(
                name: "Traffics");

            migrationBuilder.DropTable(
                name: "Lanes");

            migrationBuilder.DropTable(
                name: "Parks");
        }
    }
}
