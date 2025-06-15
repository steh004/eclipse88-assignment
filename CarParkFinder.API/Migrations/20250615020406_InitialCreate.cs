using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CarParkFinder.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarParks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CarParkNo = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarParks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarParkAvailabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CarParkNo = table.Column<string>(type: "text", nullable: false),
                    TotalLots = table.Column<int>(type: "integer", nullable: false),
                    AvailableLots = table.Column<int>(type: "integer", nullable: false),
                    RetrievedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CarParkId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarParkAvailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarParkAvailabilities_CarParks_CarParkId",
                        column: x => x.CarParkId,
                        principalTable: "CarParks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarParkAvailabilities_CarParkId",
                table: "CarParkAvailabilities",
                column: "CarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_CarParkAvailabilities_CarParkNo_RetrievedAt",
                table: "CarParkAvailabilities",
                columns: new[] { "CarParkNo", "RetrievedAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarParks_CarParkNo",
                table: "CarParks",
                column: "CarParkNo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarParkAvailabilities");

            migrationBuilder.DropTable(
                name: "CarParks");
        }
    }
}
