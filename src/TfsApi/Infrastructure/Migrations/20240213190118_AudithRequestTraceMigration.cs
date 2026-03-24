using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AudithRequestTraceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAndTimeOfDeletion",
                table: "RequestTrace");

            migrationBuilder.DropColumn(
                name: "UserIdOfCreation",
                table: "RequestTrace");

            migrationBuilder.DropColumn(
                name: "UserIdOfDeletion",
                table: "RequestTrace");

            migrationBuilder.DropColumn(
                name: "UserIdOfLastModification",
                table: "RequestTrace");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAndTimeOfDeletion",
                table: "RequestTrace",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserIdOfCreation",
                table: "RequestTrace",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserIdOfDeletion",
                table: "RequestTrace",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserIdOfLastModification",
                table: "RequestTrace",
                type: "int",
                nullable: true);
        }
    }
}
