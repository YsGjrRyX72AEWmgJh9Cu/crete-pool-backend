using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HellenicAmericanPoolHistory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RestrictTournamentDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Participations_PlayerId",
                table: "Participations",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_Players_PlayerId",
                table: "Participations",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_Tournaments_TournamentId",
                table: "Participations",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participations_Players_PlayerId",
                table: "Participations");

            migrationBuilder.DropForeignKey(
                name: "FK_Participations_Tournaments_TournamentId",
                table: "Participations");

            migrationBuilder.DropIndex(
                name: "IX_Participations_PlayerId",
                table: "Participations");
        }
    }
}
