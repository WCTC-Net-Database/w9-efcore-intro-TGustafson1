using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreRPGEntities.Migrations
{
    /// <inheritdoc />
    public partial class DoorsAndLockable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Container",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequiredKeyItemId",
                table: "Container",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Doors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomAId = table.Column<int>(type: "int", nullable: false),
                    RoomBId = table.Column<int>(type: "int", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    RequiredKeyItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doors_Rooms_RoomAId",
                        column: x => x.RoomAId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Doors_Rooms_RoomBId",
                        column: x => x.RoomBId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doors_RoomAId",
                table: "Doors",
                column: "RoomAId");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_RoomBId",
                table: "Doors",
                column: "RoomBId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doors");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Container");

            migrationBuilder.DropColumn(
                name: "RequiredKeyItemId",
                table: "Container");
        }
    }
}
