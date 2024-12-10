using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pra_C3_Native.Migrations
{
    /// <inheritdoc />
    public partial class alter_bets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Won",
                table: "Bets",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Won",
                table: "Bets");
        }
    }
}
