using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edusync.Migrations
{
    /// <inheritdoc />
    public partial class CreateResultsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    // Add other columns specific to your table as needed
                    CreatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    UpdatedAt = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");
        }
    }

}
