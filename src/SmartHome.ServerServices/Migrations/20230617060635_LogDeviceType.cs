using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.ServerServices.Migrations
{
    /// <inheritdoc />
    public partial class LogDeviceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceType",
                table: "DeviceLogs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "DeviceLogs");
        }
    }
}
