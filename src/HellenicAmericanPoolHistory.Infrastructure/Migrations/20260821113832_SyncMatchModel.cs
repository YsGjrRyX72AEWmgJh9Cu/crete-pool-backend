using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HellenicAmericanPoolHistory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncMatchModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BracketPosition",
                table: "Matches",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BracketPosition",
                table: "Matches");
        }
    }
}
