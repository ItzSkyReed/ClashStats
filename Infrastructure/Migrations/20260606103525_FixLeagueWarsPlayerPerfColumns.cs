using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLeagueWarsPlayerPerfColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "OurDestructionPercentage",
                table: "clan_war_league_wars",
                type: "real",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<float>(
                name: "OpponentDestructionPercentage",
                table: "clan_war_league_wars",
                type: "real",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "OurDestructionPercentage",
                table: "clan_war_league_wars",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<short>(
                name: "OpponentDestructionPercentage",
                table: "clan_war_league_wars",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
