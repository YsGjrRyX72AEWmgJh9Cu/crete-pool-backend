using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HellenicAmericanPoolHistory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchBracketPositionUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Matches_TournamentId",
                table: "Matches");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentId_Round_BracketPosition",
                table: "Matches",
                columns: new[] { "TournamentId", "Round", "BracketPosition" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Matches_TournamentId_Round_BracketPosition",
                table: "Matches");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentId",
                table: "Matches",
                column: "TournamentId");
        }
    }
}
