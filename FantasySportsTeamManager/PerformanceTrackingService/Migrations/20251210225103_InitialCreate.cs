using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerformanceTrackingService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PerformanceStats",
                columns: table => new
                {
                    statId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    playerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points = table.Column<int>(type: "int", nullable: false),
                    assists = table.Column<int>(type: "int", nullable: false),
                    rebounds = table.Column<int>(type: "int", nullable: false),
                    gameDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    competitionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceStats", x => x.statId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceStats_playerId_gameDate",
                table: "PerformanceStats",
                columns: new[] { "playerId", "gameDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerformanceStats");
        }
    }
}
