using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatePlayerActivitySnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_player_activity_snapshots_clan_members_ClanMemberTag",
                table: "player_activity_snapshots");

            migrationBuilder.DropIndex(
                name: "IX_player_activity_snapshots_ClanMemberTag",
                table: "player_activity_snapshots");

            migrationBuilder.DropColumn(
                name: "ClanMemberTag",
                table: "player_activity_snapshots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClanMemberTag",
                table: "player_activity_snapshots",
                type: "character varying(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_activity_snapshots_ClanMemberTag",
                table: "player_activity_snapshots",
                column: "ClanMemberTag");

            migrationBuilder.AddForeignKey(
                name: "FK_player_activity_snapshots_clan_members_ClanMemberTag",
                table: "player_activity_snapshots",
                column: "ClanMemberTag",
                principalTable: "clan_members",
                principalColumn: "Tag");
        }
    }
}
