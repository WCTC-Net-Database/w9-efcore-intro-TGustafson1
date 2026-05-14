using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreRPGEntities.Migrations
{
    /// <inheritdoc />
    public partial class ChestsAndMonsterLoot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LootId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_LootId",
                table: "Characters",
                column: "LootId",
                unique: true,
                filter: "[LootId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Container_LootId",
                table: "Characters",
                column: "LootId",
                principalTable: "Container",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Container_LootId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_LootId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "LootId",
                table: "Characters");
        }
    }
}
