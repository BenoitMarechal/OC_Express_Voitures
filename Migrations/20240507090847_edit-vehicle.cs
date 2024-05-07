using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OC_Express_Voitures.Migrations
{
    /// <inheritdoc />
    public partial class editvehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAvailable",
                table: "Operation",
                newName: "IsAvailable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Operation",
                newName: "isAvailable");
        }
    }
}
