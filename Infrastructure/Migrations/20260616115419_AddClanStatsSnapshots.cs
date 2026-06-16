using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClanStatsSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clan_stats_snapshots",
                columns: table => new
                {
                    ClanTag = table.Column<string>(type: "text", nullable: false),
                    CapturedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    ClanLevel = table.Column<short>(type: "smallint", nullable: false),
                    ClanPoints = table.Column<int>(type: "integer", nullable: false),
                    ClanBuilderBasePoints = table.Column<int>(type: "integer", nullable: false),
                    ClanCapitalPoints = table.Column<short>(type: "smallint", nullable: false),
                    WarLeagueName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, collation: "C"),
                    WarLeagueId = table.Column<int>(type: "integer", nullable: false),
                    CapitalLeagueName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, collation: "C"),
                    CapitalLeagueId = table.Column<int>(type: "integer", nullable: false),
                    WarWinStreak = table.Column<short>(type: "smallint", nullable: false),
                    WarWins = table.Column<short>(type: "smallint", nullable: false),
                    WarTies = table.Column<short>(type: "smallint", nullable: false),
                    WarLosses = table.Column<short>(type: "smallint", nullable: false),
                    MembersCount = table.Column<short>(type: "smallint", nullable: false),
                    AverageTownHallLevel = table.Column<float>(type: "real", nullable: false),
                    RequiredTrophies = table.Column<short>(type: "smallint", nullable: false),
                    RequiredTownHallLevel = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clan_stats_snapshots", x => new { x.CapturedAt, x.ClanTag });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clan_stats_snapshots");
        }
    }
}
