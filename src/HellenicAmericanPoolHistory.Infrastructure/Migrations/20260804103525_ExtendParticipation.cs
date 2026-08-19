using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HellenicAmericanPoolHistory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtendParticipation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Seed",
                table: "Participations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Participations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Participations_TournamentId_PlayerId",
                table: "Participations",
                columns: new[] { "TournamentId", "PlayerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Participations_TournamentId_PlayerId",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "Seed",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Participations");
        }
    }
}
