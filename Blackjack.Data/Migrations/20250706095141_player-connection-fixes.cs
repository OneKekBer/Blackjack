using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blackjack.Data.Migrations
{
    /// <inheritdoc />
    public partial class playerconnectionfixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerConnections_Players_PlayerId",
                table: "PlayerConnections");

            migrationBuilder.DropIndex(
                name: "IX_PlayerConnections_PlayerId",
                table: "PlayerConnections");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Players");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerConnections_PlayerId",
                table: "PlayerConnections",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerConnections_Players_PlayerId",
                table: "PlayerConnections",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
