using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clan_members",
                columns: table => new
                {
                    Tag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, collation: "C"),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TownHallLevel = table.Column<short>(type: "smallint", nullable: false),
                    BuilderHallLevel = table.Column<short>(type: "smallint", nullable: false),
                    ExpLevel = table.Column<short>(type: "smallint", nullable: false),
                    Trophies = table.Column<short>(type: "smallint", nullable: false),
                    WarStars = table.Column<int>(type: "integer", nullable: false),
                    BuilderBaseTrophies = table.Column<short>(type: "smallint", nullable: false),
                    BestBuilderBaseTrophies = table.Column<short>(type: "smallint", nullable: false),
                    Role = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, collation: "C"),
                    ClanCapitalContributions = table.Column<int>(type: "integer", nullable: false),
                    IsNowInClan = table.Column<bool>(type: "boolean", nullable: false),
                    WarPreference = table.Column<string>(type: "text", nullable: false, collation: "C")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clan_members", x => x.Tag);
                });

            migrationBuilder.CreateTable(
                name: "clan_wars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    State = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, collation: "C"),
                    StartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TeamSize = table.Column<short>(type: "smallint", nullable: false),
                    OpponentClanTag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, collation: "C"),
                    OpponentClanName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    OpponentClanLevel = table.Column<short>(type: "smallint", nullable: false),
                    OpponentAttacks = table.Column<short>(type: "smallint", nullable: false),
                    OurAttacks = table.Column<short>(type: "smallint", nullable: false),
                    OurStars = table.Column<short>(type: "smallint", nullable: false),
                    OpponentStars = table.Column<short>(type: "smallint", nullable: false),
                    OurDestructionPercentage = table.Column<float>(type: "real", nullable: false),
                    OpponentDestructionPercentage = table.Column<float>(type: "real", nullable: false),
                    ExpEarned = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clan_wars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "season_stats",
                columns: table => new
                {
                    SeasonDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PlayerTag = table.Column<string>(type: "character varying(10)", nullable: false),
                    Donations = table.Column<int>(type: "integer", nullable: false),
                    DonationsReceived = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_season_stats", x => new { x.SeasonDate, x.PlayerTag });
                    table.ForeignKey(
                        name: "FK_season_stats_clan_members_PlayerTag",
                        column: x => x.PlayerTag,
                        principalTable: "clan_members",
                        principalColumn: "Tag",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clan_war_player_performances",
                columns: table => new
                {
                    WarId = table.Column<int>(type: "integer", nullable: false),
                    PlayerTag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, collation: "C"),
                    MapPosition = table.Column<short>(type: "smallint", nullable: false),
                    TownHallLevel = table.Column<short>(type: "smallint", nullable: false),
                    Attack1Stars = table.Column<short>(type: "smallint", nullable: true),
                    Defender1Tag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, collation: "C"),
                    Attack1Destruction = table.Column<short>(type: "smallint", nullable: true),
                    Attack1Duration = table.Column<short>(type: "smallint", nullable: true),
                    Opponent1Position = table.Column<short>(type: "smallint", nullable: true),
                    Opponent1TownHallLevel = table.Column<short>(type: "smallint", nullable: true),
                    Attack2Stars = table.Column<short>(type: "smallint", nullable: true),
                    Defender2Tag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, collation: "C"),
                    Attack2Duration = table.Column<short>(type: "smallint", nullable: true),
                    Attack2Destruction = table.Column<short>(type: "smallint", nullable: true),
                    Opponent2Position = table.Column<short>(type: "smallint", nullable: true),
                    Opponent2TownHallLevel = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clan_war_player_performances", x => new { x.WarId, x.PlayerTag });
                    table.ForeignKey(
                        name: "FK_clan_war_player_performances_clan_wars_WarId",
                        column: x => x.WarId,
                        principalTable: "clan_wars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clan_wars_OpponentClanTag",
                table: "clan_wars",
                column: "OpponentClanTag");

            migrationBuilder.CreateIndex(
                name: "IX_clan_wars_StartTime_EndTime",
                table: "clan_wars",
                columns: new[] { "StartTime", "EndTime" });

            migrationBuilder.CreateIndex(
                name: "IX_season_stats_PlayerTag",
                table: "season_stats",
                column: "PlayerTag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clan_war_player_performances");

            migrationBuilder.DropTable(
                name: "season_stats");

            migrationBuilder.DropTable(
                name: "clan_wars");

            migrationBuilder.DropTable(
                name: "clan_members");
        }
    }
}
