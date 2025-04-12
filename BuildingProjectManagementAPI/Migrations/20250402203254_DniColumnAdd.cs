using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingProjectManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class DniColumnAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dni",
                table: "Usuarios",
                type: "varchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dni",
                table: "Usuarios");
        }
    }
}
