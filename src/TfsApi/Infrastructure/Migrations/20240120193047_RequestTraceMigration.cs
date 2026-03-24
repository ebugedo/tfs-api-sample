using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RequestTraceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestTrace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IPAddress = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    NumberOfRequests = table.Column<int>(type: "int", nullable: false),
                    DateAndTimeOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAndTimeOfDeletion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateAndTimeOfLastModification = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserIdOfCreation = table.Column<int>(type: "int", nullable: false),
                    UserIdOfDeletion = table.Column<int>(type: "int", nullable: true),
                    UserIdOfLastModification = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTrace", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "UK_RequestTrace_IPAddress",
                table: "RequestTrace",
                column: "IPAddress",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestTrace");
        }
    }
}
