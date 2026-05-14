using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreRPGEntities.Migrations
{
    /// <inheritdoc />
    public partial class MonsterLife : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAlive",
                table: "Characters",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAlive",
                table: "Characters");
        }
    }
}
