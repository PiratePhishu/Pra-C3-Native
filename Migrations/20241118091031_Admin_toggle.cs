using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pra_C3_Native.Migrations
{
    /// <inheritdoc />
    public partial class Admin_toggle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Admin",
                table: "users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "users");
        }
    }
}
