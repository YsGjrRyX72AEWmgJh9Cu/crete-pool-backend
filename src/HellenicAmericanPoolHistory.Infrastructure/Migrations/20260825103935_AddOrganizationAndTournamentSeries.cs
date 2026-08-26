using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HellenicAmericanPoolHistory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationAndTournamentSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TournamentSeriesId",
                table: "Tournaments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TournamentSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentSeries_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_TournamentSeriesId",
                table: "Tournaments",
                column: "TournamentSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentSeries_OrganizationId",
                table: "TournamentSeries",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_TournamentSeries_TournamentSeriesId",
                table: "Tournaments",
                column: "TournamentSeriesId",
                principalTable: "TournamentSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_TournamentSeries_TournamentSeriesId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "TournamentSeries");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_TournamentSeriesId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "TournamentSeriesId",
                table: "Tournaments");
        }
    }
}
