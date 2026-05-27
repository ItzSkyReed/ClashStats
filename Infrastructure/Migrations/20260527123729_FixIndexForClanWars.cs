using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixIndexForClanWars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_clan_wars_OpponentClanTag",
                table: "clan_wars");

            migrationBuilder.DropIndex(
                name: "IX_clan_wars_StartTime_EndTime",
                table: "clan_wars");

            migrationBuilder.CreateIndex(
                name: "IX_clan_wars_OpponentClanTag_OpponentClanName_StartTime",
                table: "clan_wars",
                columns: new[] { "OpponentClanTag", "OpponentClanName", "StartTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_clan_wars_OpponentClanTag_OpponentClanName_StartTime",
                table: "clan_wars");

            migrationBuilder.CreateIndex(
                name: "IX_clan_wars_OpponentClanTag",
                table: "clan_wars",
                column: "OpponentClanTag");

            migrationBuilder.CreateIndex(
                name: "IX_clan_wars_StartTime_EndTime",
                table: "clan_wars",
                columns: new[] { "StartTime", "EndTime" });
        }
    }
}
