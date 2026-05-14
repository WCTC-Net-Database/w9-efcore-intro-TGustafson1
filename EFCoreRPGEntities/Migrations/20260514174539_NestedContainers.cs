using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreRPGEntities.Migrations
{
    /// <inheritdoc />
    public partial class NestedContainers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentContainerId",
                table: "Container",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Container",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Container_ParentContainerId",
                table: "Container",
                column: "ParentContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Container_RoomId",
                table: "Container",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Container_Container_ParentContainerId",
                table: "Container",
                column: "ParentContainerId",
                principalTable: "Container",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Container_Rooms_RoomId",
                table: "Container",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Container_Container_ParentContainerId",
                table: "Container");

            migrationBuilder.DropForeignKey(
                name: "FK_Container_Rooms_RoomId",
                table: "Container");

            migrationBuilder.DropIndex(
                name: "IX_Container_ParentContainerId",
                table: "Container");

            migrationBuilder.DropIndex(
                name: "IX_Container_RoomId",
                table: "Container");

            migrationBuilder.DropColumn(
                name: "ParentContainerId",
                table: "Container");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Container");
        }
    }
}
