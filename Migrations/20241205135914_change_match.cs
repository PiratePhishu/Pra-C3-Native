using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pra_C3_Native.Migrations
{
    /// <inheritdoc />
    public partial class change_match : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Match_id",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Team1_Score",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Team2_Score",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Winner",
                table: "Matches",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Match_id",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Team1_Score",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Team2_Score",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Winner",
                table: "Matches");
        }
    }
}
