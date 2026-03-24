using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueKeysMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectsTechnologies_ProjectId",
                table: "ProjectsTechnologies");

            migrationBuilder.CreateIndex(
                name: "UK_Technologies_Name",
                table: "Technologies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Sectors_Name",
                table: "Sectors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_ProjectsTechnologies_ProjectId_TechnologyId",
                table: "ProjectsTechnologies",
                columns: new[] { "ProjectId", "TechnologyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Projects_Name",
                table: "Projects",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_Clients_Name",
                table: "Clients",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UK_Technologies_Name",
                table: "Technologies");

            migrationBuilder.DropIndex(
                name: "UK_Sectors_Name",
                table: "Sectors");

            migrationBuilder.DropIndex(
                name: "UK_ProjectsTechnologies_ProjectId_TechnologyId",
                table: "ProjectsTechnologies");

            migrationBuilder.DropIndex(
                name: "UK_Projects_Name",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "UK_Clients_Name",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsTechnologies_ProjectId",
                table: "ProjectsTechnologies",
                column: "ProjectId");
        }
    }
}
