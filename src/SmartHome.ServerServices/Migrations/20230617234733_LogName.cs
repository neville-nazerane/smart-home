using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.ServerServices.Migrations
{
    /// <inheritdoc />
    public partial class LogName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DeviceLogs",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "DeviceLogs");
        }
    }
}
