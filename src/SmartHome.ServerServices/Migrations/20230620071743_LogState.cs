using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.ServerServices.Migrations
{
    /// <inheritdoc />
    public partial class LogState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OccurredOn",
                table: "DeviceLogs",
                newName: "LoggedOn");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "DeviceLogs",
                type: "TEXT",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "DeviceLogs");

            migrationBuilder.RenameColumn(
                name: "LoggedOn",
                table: "DeviceLogs",
                newName: "OccurredOn");
        }
    }
}
