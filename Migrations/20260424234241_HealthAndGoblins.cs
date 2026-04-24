using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace W09.Migrations
{
    /// <inheritdoc />
    public partial class HealthAndGoblins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Health",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Health",
                table: "Characters");
        }
    }
}
