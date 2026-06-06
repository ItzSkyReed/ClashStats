using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedClanWarLeaguesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clan_war_league_groups",
                columns: table => new
                {
                    Season = table.Column<string>(type: "text", nullable: false, collation: "C"),
                    State = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, collation: "C"),
                    TeamSize = table.Column<short>(type: "smallint", nullable: false),
                    Place = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clan_war_league_groups", x => x.Season);
                });

            migrationBuilder.CreateTable(
                name: "clan_war_league_wars",
                columns: table => new
                {
                    WarTag = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, collation: "C"),
                    StartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    WarStartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Season = table.Column<string>(type: "text", nullable: false, collation: "C"),
                    Round = table.Column<short>(type: "smallint", nullable: false),
                    OpponentClanTag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, collation: "C"),
                    OpponentClanName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    OpponentClanLevel = table.Column<short>(type: "smallint", nullable: false),
                    OpponentAttacks = table.Column<short>(type: "smallint", nullable: false),
                    OpponentStars = table.Column<short>(type: "smallint", nullable: false),
                    OpponentDestructionPercentage = table.Column<short>(type: "smallint", nullable: false),
                    OurAttacks = table.Column<short>(type: "smallint", nullable: false),
                    OurStars = table.Column<short>(type: "smallint", nullable: false),
                    OurDestructionPercentage = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clan_war_league_wars", x => x.WarTag);
                    table.ForeignKey(
                        name: "FK_clan_war_league_wars_clan_war_league_groups_Season",
                        column: x => x.Season,
                        principalTable: "clan_war_league_groups",
                        principalColumn: "Season",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clan_war_league_player_performances",
                columns: table => new
                {
                    WarTag = table.Column<string>(type: "text", nullable: false, collation: "C"),
                    PlayerTag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, collation: "C"),
                    MapPosition = table.Column<short>(type: "smallint", nullable: false),
                    TownHallLevel = table.Column<short>(type: "smallint", nullable: false),
                    AttackStars = table.Column<short>(type: "smallint", nullable: true),
                    DefenderTag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, collation: "C"),
                    AttackDestruction = table.Column<short>(type: "smallint", nullable: true),
                    AttackDuration = table.Column<short>(type: "smallint", nullable: true),
                    OpponentPosition = table.Column<short>(type: "smallint", nullable: true),
                    OpponentTownHallLevel = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clan_war_league_player_performances", x => new { x.WarTag, x.PlayerTag });
                    table.ForeignKey(
                        name: "FK_clan_war_league_player_performances_clan_members_PlayerTag",
                        column: x => x.PlayerTag,
                        principalTable: "clan_members",
                        principalColumn: "Tag");
                    table.ForeignKey(
                        name: "FK_clan_war_league_player_performances_clan_war_league_wars_Wa~",
                        column: x => x.WarTag,
                        principalTable: "clan_war_league_wars",
                        principalColumn: "WarTag",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clan_war_league_player_performances_PlayerTag",
                table: "clan_war_league_player_performances",
                column: "PlayerTag");

            migrationBuilder.CreateIndex(
                name: "IX_clan_war_league_wars_Season",
                table: "clan_war_league_wars",
                column: "Season");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clan_war_league_player_performances");

            migrationBuilder.DropTable(
                name: "clan_war_league_wars");

            migrationBuilder.DropTable(
                name: "clan_war_league_groups");
        }
    }
}
