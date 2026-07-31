using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HellenicAmericanPoolHistory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTournament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Discipline",
                table: "Tournaments");

            migrationBuilder.AddColumn<string>(
                name: "BracketType",
                table: "Tournaments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GameSet",
                table: "Tournaments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TournamentStatus",
                table: "Tournaments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TournamentType",
                table: "Tournaments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "VenueId",
                table: "Tournaments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_VenueId",
                table: "Tournaments",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Venues_VenueId",
                table: "Tournaments",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Venues_VenueId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_VenueId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "BracketType",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "GameSet",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "TournamentStatus",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "TournamentType",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "Tournaments");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Tournaments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Tournaments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discipline",
                table: "Tournaments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
