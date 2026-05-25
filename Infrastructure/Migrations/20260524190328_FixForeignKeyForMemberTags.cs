using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeyForMemberTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_clan_war_player_performances_PlayerTag",
                table: "clan_war_player_performances",
                column: "PlayerTag");

            migrationBuilder.AddForeignKey(
                name: "FK_clan_war_player_performances_clan_members_PlayerTag",
                table: "clan_war_player_performances",
                column: "PlayerTag",
                principalTable: "clan_members",
                principalColumn: "Tag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clan_war_player_performances_clan_members_PlayerTag",
                table: "clan_war_player_performances");

            migrationBuilder.DropIndex(
                name: "IX_clan_war_player_performances_PlayerTag",
                table: "clan_war_player_performances");
        }
    }
}
