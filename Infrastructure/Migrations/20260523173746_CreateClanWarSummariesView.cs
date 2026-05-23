using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateClanWarSummariesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE MATERIALIZED VIEW mv_clan_war_summaries AS
                WITH opponent_bases AS (
                    SELECT ""WarId"", ""Defender1Tag"" AS tag, ""Opponent1TownHallLevel"" AS th
                    FROM public.clan_war_player_performances 
                    WHERE ""Defender1Tag"" IS NOT NULL
                    UNION
                    SELECT ""WarId"", ""Defender2Tag"" AS tag, ""Opponent2TownHallLevel"" AS th
                    FROM public.clan_war_player_performances 
                    WHERE ""Defender2Tag"" IS NOT NULL
                ),
                opponent_avg_th AS (
                    SELECT ""WarId"", AVG(th)::real AS avg_opp_th
                    FROM opponent_bases
                    GROUP BY ""WarId""
                ),
                our_stats AS (
                    SELECT 
                        ""WarId"",
                        AVG(""TownHallLevel"")::real AS avg_our_th,
                        SUM(
                            (CASE WHEN ""Attack1Stars"" = 3 THEN 1 ELSE 0 END) + 
                            (CASE WHEN ""Attack2Stars"" = 3 THEN 1 ELSE 0 END)
                        )::smallint AS three_stars_count
                    FROM public.clan_war_player_performances
                    GROUP BY ""WarId""
                )
                SELECT 
                    cw.""Id"",
                    cw.""StartTime"",
                    cw.""EndTime"",
                    cw.""TeamSize"",
                    cw.""OpponentClanName"",
                    cw.""OpponentClanLevel"",
                    cw.""OpponentAttacks"",
                    cw.""OurAttacks"",
                    cw.""OurStars"",
                    cw.""OpponentStars"",
                    cw.""OurDestructionPercentage"",
                    cw.""OpponentDestructionPercentage"",
                    COALESCE(cw.""ExpEarned"", 0::smallint) AS ""ExpEarned"",
                    
                    COALESCE(os.avg_our_th, 0) AS ""AverageOurTownHall"",
                    COALESCE(oa.avg_opp_th, 0) AS ""AverageOpponentTownHall"",
                    COALESCE(os.avg_our_th, 0) - COALESCE(oa.avg_opp_th, 0) AS ""TownHallAdvantage"",
                    
                    (cw.""OurAttacks""::real / NULLIF(cw.""TeamSize"" * 2, 0)) * 100 AS ""ParticipationRate"",
                    COALESCE(os.three_stars_count, 0) AS ""OurThreeStarsCount"",
                    (cw.""OurStars""::real / NULLIF(cw.""OurAttacks"", 0)) AS ""StarsPerAttack""

                FROM public.clan_wars cw
                LEFT JOIN our_stats os ON os.""WarId"" = cw.""Id""
                LEFT JOIN opponent_avg_th oa ON oa.""WarId"" = cw.""Id""
                
                WHERE cw.""State"" = 'ended';

                CREATE UNIQUE INDEX idx_mv_clan_war_summaries_id ON mv_clan_war_summaries(""Id"");
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP MATERIALIZED VIEW IF EXISTS mv_clan_war_summaries;");
        }
    }
}
