using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace W09.Migrations
{
    /// <inheritdoc />
    public partial class RefactoredMonsters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerAbility_AbilityLevel",
                table: "Abilities");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Abilities",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AlterColumn<int>(
                name: "AbilityLevel",
                table: "Abilities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Abilities",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.AlterColumn<int>(
                name: "AbilityLevel",
                table: "Abilities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PlayerAbility_AbilityLevel",
                table: "Abilities",
                type: "int",
                nullable: true);
        }
    }
}
