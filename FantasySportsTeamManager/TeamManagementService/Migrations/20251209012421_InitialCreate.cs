using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamManagementService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    teamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.teamId);
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "teamId", "createdDate", "teamName" },
                values: new object[] { -1, new DateTime(2025, 12, 8, 17, 24, 16, 872, DateTimeKind.Local).AddTicks(8584), "string" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
