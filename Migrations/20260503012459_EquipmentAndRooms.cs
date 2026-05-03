using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace W09.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentAndRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Rooms_RoomId",
                table: "Characters");

            migrationBuilder.AddColumn<int>(
                name: "EastRoomId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NorthRoomId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SouthRoomId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WestRoomId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Characters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EquipmentId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attack = table.Column<int>(type: "int", nullable: false),
                    Defense = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeaponId = table.Column<int>(type: "int", nullable: true),
                    ArmorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_Items_ArmorId",
                        column: x => x.ArmorId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_Items_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_EastRoomId",
                table: "Rooms",
                column: "EastRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_NorthRoomId",
                table: "Rooms",
                column: "NorthRoomId",
                unique: true,
                filter: "[NorthRoomId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SouthRoomId",
                table: "Rooms",
                column: "SouthRoomId",
                unique: true,
                filter: "[SouthRoomId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_WestRoomId",
                table: "Rooms",
                column: "WestRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_EquipmentId",
                table: "Characters",
                column: "EquipmentId",
                unique: true,
                filter: "[EquipmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_ArmorId",
                table: "Equipment",
                column: "ArmorId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_WeaponId",
                table: "Equipment",
                column: "WeaponId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Equipment_EquipmentId",
                table: "Characters",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Rooms_RoomId",
                table: "Characters",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Rooms_EastRoomId",
                table: "Rooms",
                column: "EastRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Rooms_NorthRoomId",
                table: "Rooms",
                column: "NorthRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Rooms_SouthRoomId",
                table: "Rooms",
                column: "SouthRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Rooms_WestRoomId",
                table: "Rooms",
                column: "WestRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Equipment_EquipmentId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Rooms_RoomId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Rooms_EastRoomId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Rooms_NorthRoomId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Rooms_SouthRoomId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Rooms_WestRoomId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_EastRoomId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_NorthRoomId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_SouthRoomId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_WestRoomId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Characters_EquipmentId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "EastRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NorthRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "SouthRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "WestRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "Characters");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Rooms_RoomId",
                table: "Characters",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
