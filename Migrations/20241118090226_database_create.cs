using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pra_C3_Native.Migrations
{
    /// <inheritdoc />
    public partial class database_create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
