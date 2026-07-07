using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estapar.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        // ── GUIDs fixos para testes ──────────────────────────────────────────
        private static readonly Guid Park1Id = new("10000001-0000-0000-0000-000000000001");
        private static readonly Guid Park2Id = new("10000001-0000-0000-0000-000000000002");

        private static readonly Guid Park1LaneEntryId = new("20000001-0000-0000-0001-000000000001");
        private static readonly Guid Park1LaneExitId  = new("20000001-0000-0000-0001-000000000002");
        private static readonly Guid Park2LaneEntryId = new("20000001-0000-0000-0002-000000000001");
        private static readonly Guid Park2LaneExitId  = new("20000001-0000-0000-0002-000000000002");

        private static readonly Guid Park1PriceTableId = new("30000001-0000-0000-0001-000000000001");
        private static readonly Guid Park2PriceTableId = new("30000001-0000-0000-0002-000000000001");

        // Garagens: 40000001-0000-0000-{parkIndex}-{garageIndex:12}
        private static readonly Guid[] Park1GarageIds =
        [
            new("40000001-0000-0000-0001-000000000001"),
            new("40000001-0000-0000-0001-000000000002"),
            new("40000001-0000-0000-0001-000000000003"),
            new("40000001-0000-0000-0001-000000000004"),
            new("40000001-0000-0000-0001-000000000005"),
            new("40000001-0000-0000-0001-000000000006"),
            new("40000001-0000-0000-0001-000000000007"),
            new("40000001-0000-0000-0001-000000000008"),
            new("40000001-0000-0000-0001-000000000009"),
            new("40000001-0000-0000-0001-000000000010"),
        ];

        private static readonly Guid[] Park2GarageIds =
        [
            new("40000001-0000-0000-0002-000000000001"),
            new("40000001-0000-0000-0002-000000000002"),
            new("40000001-0000-0000-0002-000000000003"),
            new("40000001-0000-0000-0002-000000000004"),
            new("40000001-0000-0000-0002-000000000005"),
            new("40000001-0000-0000-0002-000000000006"),
            new("40000001-0000-0000-0002-000000000007"),
            new("40000001-0000-0000-0002-000000000008"),
            new("40000001-0000-0000-0002-000000000009"),
            new("40000001-0000-0000-0002-000000000010"),
        ];

        private static readonly DateTime SeedDate = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── 1. Parks ─────────────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "Parks",
                columns: ["Id", "Name", "Description", "Created"],
                values: new object[,]
                {
                    { Park1Id, "Estapar Centro",    "Estacionamento central, próximo às principais avenidas do centro.", SeedDate },
                    { Park2Id, "Estapar Zona Sul",  "Estacionamento na zona sul, com fácil acesso ao metrô.",           SeedDate }
                });

            // ── 2. Lanes (1 entrada + 1 saída por estacionamento) ────────────
            // LaneType: Entry = 1 | Exit = 2
            // LaneStatus: Active = 1
            migrationBuilder.InsertData(
                table: "Lanes",
                columns: ["Id", "ParkId", "Name", "LaneType", "Status", "Created"],
                values: new object[,]
                {
                    { Park1LaneEntryId, Park1Id, "Entrada Principal — Centro",    1, 1, SeedDate },
                    { Park1LaneExitId,  Park1Id, "Saída Principal — Centro",      2, 1, SeedDate },
                    { Park2LaneEntryId, Park2Id, "Entrada Principal — Zona Sul",  1, 1, SeedDate },
                    { Park2LaneExitId,  Park2Id, "Saída Principal — Zona Sul",    2, 1, SeedDate }
                });

            // ── 3. PriceTables ───────────────────────────────────────────────
            // HourlyRate: R$ 15,00 (Centro) | R$ 12,00 (Zona Sul)
            // GracePeriodMinutes: 30 min (ambos)
            migrationBuilder.InsertData(
                table: "PriceTables",
                columns: ["Id", "ParkId", "HourlyRate", "GracePeriodMinutes", "Created"],
                values: new object[,]
                {
                    { Park1PriceTableId, Park1Id, 15.00m, 30, SeedDate },
                    { Park2PriceTableId, Park2Id, 12.00m, 30, SeedDate }
                });

            // ── 4. Garages ───────────────────────────────────────────────────
            // Park 1 — 10 garagens (A a J)
            var garageLetters = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            var park1GarageRows = new object[10, 4];
            var park2GarageRows = new object[10, 4];

            for (int i = 0; i < 10; i++)
            {
                park1GarageRows[i, 0] = Park1GarageIds[i];
                park1GarageRows[i, 1] = Park1Id;
                park1GarageRows[i, 2] = $"Garagem {garageLetters[i]} — Centro";
                park1GarageRows[i, 3] = SeedDate;

                park2GarageRows[i, 0] = Park2GarageIds[i];
                park2GarageRows[i, 1] = Park2Id;
                park2GarageRows[i, 2] = $"Garagem {garageLetters[i]} — Zona Sul";
                park2GarageRows[i, 3] = SeedDate;
            }

            migrationBuilder.InsertData(
                table: "Garages",
                columns: ["Id", "ParkId", "Name", "Created"],
                values: park1GarageRows);

            migrationBuilder.InsertData(
                table: "Garages",
                columns: ["Id", "ParkId", "Name", "Created"],
                values: park2GarageRows);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Ordem inversa às FKs

            migrationBuilder.DeleteData(table: "PriceTables", keyColumn: "Id", keyValue: Park1PriceTableId);
            migrationBuilder.DeleteData(table: "PriceTables", keyColumn: "Id", keyValue: Park2PriceTableId);

            migrationBuilder.DeleteData(table: "Lanes", keyColumn: "Id", keyValue: Park1LaneEntryId);
            migrationBuilder.DeleteData(table: "Lanes", keyColumn: "Id", keyValue: Park1LaneExitId);
            migrationBuilder.DeleteData(table: "Lanes", keyColumn: "Id", keyValue: Park2LaneEntryId);
            migrationBuilder.DeleteData(table: "Lanes", keyColumn: "Id", keyValue: Park2LaneExitId);

            foreach (var id in Park1GarageIds)
                migrationBuilder.DeleteData(table: "Garages", keyColumn: "Id", keyValue: id);

            foreach (var id in Park2GarageIds)
                migrationBuilder.DeleteData(table: "Garages", keyColumn: "Id", keyValue: id);

            migrationBuilder.DeleteData(table: "Parks", keyColumn: "Id", keyValue: Park1Id);
            migrationBuilder.DeleteData(table: "Parks", keyColumn: "Id", keyValue: Park2Id);
        }
    }
}

