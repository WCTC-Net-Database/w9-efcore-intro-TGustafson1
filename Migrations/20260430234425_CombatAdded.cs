using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace W09.Migrations
{
    /// <inheritdoc />
    public partial class CombatAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Kick",
                table: "Abilities",
                newName: "Uses");

            migrationBuilder.RenameColumn(
                name: "Heckle",
                table: "Abilities",
                newName: "PlayerAbility_AbilityLevel");

            migrationBuilder.AddColumn<int>(
                name: "Defense",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Strength",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AbilityLevel",
                table: "Abilities",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Defense",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "AbilityLevel",
                table: "Abilities");

            migrationBuilder.RenameColumn(
                name: "Uses",
                table: "Abilities",
                newName: "Kick");

            migrationBuilder.RenameColumn(
                name: "PlayerAbility_AbilityLevel",
                table: "Abilities",
                newName: "Heckle");
        }
    }
}
