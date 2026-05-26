using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatePlayerSummariesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        migrationBuilder.Sql(@"
                CREATE MATERIALIZED VIEW ""mv_clan_war_player_summaries"" AS
                
                WITH ""EndedWarPerformances"" AS (
                    SELECT 
                        Cwpp.*, 
                        Cw.""StartTime""
                    FROM public.clan_war_player_performances Cwpp
                    JOIN public.clan_wars Cw ON Cw.""Id"" = Cwpp.""WarId""
                    WHERE Cw.""State"" = 'warEnded'
                ),

                ""UnpivotedAttacks"" AS (
                    SELECT 
                        ""WarId"", ""PlayerTag"", ""MapPosition"", ""TownHallLevel"",
                        ""Attack1Stars"" AS ""Stars"", 
                        ""Attack1Destruction"" AS ""Destruction"", 
                        ""Opponent1Position"" AS ""OpponentPosition"", 
                        ""Opponent1TownHallLevel"" AS ""OpponentTh""
                    FROM ""EndedWarPerformances""
                    WHERE ""Attack1Stars"" IS NOT NULL
                    
                    UNION ALL
                    
                    SELECT 
                        ""WarId"", ""PlayerTag"", ""MapPosition"", ""TownHallLevel"",
                        ""Attack2Stars"" AS ""Stars"", 
                        ""Attack2Destruction"" AS ""Destruction"", 
                        ""Opponent2Position"" AS ""OpponentPosition"", 
                        ""Opponent2TownHallLevel"" AS ""OpponentTh""
                    FROM ""EndedWarPerformances""
                    WHERE ""Attack2Stars"" IS NOT NULL
                ),
                
                ""AttackMetrics"" AS (
                    SELECT 
                        ""PlayerTag"",
                        AVG(""OpponentPosition"")::real AS ""AvgOpponentMapPosition"",
                        AVG(""OpponentTh"")::real AS ""AvgOpponentTh"",
                        AVG(""Destruction"")::real AS ""AvgDestruction"",
                        
                        (COUNT(*) FILTER (WHERE ""OpponentPosition"" = ""MapPosition"")::real / 
                            NULLIF(COUNT(*), 0)) * 100 AS ""MirrorRate"",
                            
                        (COUNT(*) FILTER (WHERE ""Stars"" = 3 AND ""OpponentTh"" = ""TownHallLevel"")::real / 
                            NULLIF(COUNT(*) FILTER (WHERE ""OpponentTh"" = ""TownHallLevel""), 0)) * 100 AS ""SameTh3StarRate"",
                            
                        AVG(""OpponentTh"" - ""TownHallLevel"")::real AS ""AvgThMismatch""
                    FROM ""UnpivotedAttacks""
                    GROUP BY ""PlayerTag""
                ),

                ""PlayerWarStats"" AS (
                    SELECT 
                        ""PlayerTag"",
                        ""StartTime"",
                        ""MapPosition"",
                        ""TownHallLevel"",
                        (CASE WHEN ""Attack1Stars"" IS NOT NULL THEN 1 ELSE 0 END + 
                         CASE WHEN ""Attack2Stars"" IS NOT NULL THEN 1 ELSE 0 END)::smallint AS ""AttacksUsed"",
                        CASE WHEN ""Attack1Stars"" IS NOT NULL THEN 1 ELSE 0 END AS ""FirstAttackUsed"",
                        CASE WHEN ""Attack2Stars"" IS NOT NULL THEN 1 ELSE 0 END AS ""SecondAttackUsed"",
                        COALESCE(""Attack1Stars"", 0) + COALESCE(""Attack2Stars"", 0) AS ""TotalStars""
                    FROM ""EndedWarPerformances""
                ),
                
                ""WarMetrics"" AS (
                    SELECT 
                        ""PlayerTag"",
                        AVG(""MapPosition"")::real AS ""AvgMapPosition"",
                        AVG(""TownHallLevel"")::real AS ""AvgTh"",
                        (SUM(""AttacksUsed"")::real / NULLIF(COUNT(*) * 2, 0)) * 100 AS ""AttackParticipationRate"",
                        AVG(""TotalStars"")::real AS ""AvgStarsPerWar"",
                        ((SUM(""FirstAttackUsed"")::real / NULLIF(COUNT(*), 0)) * 100)::real AS ""FirstAttackParticipationRate"",
                        ((SUM(""SecondAttackUsed"")::real / NULLIF(COUNT(*), 0)) * 100)::real AS ""SecondAttackParticipationRate""
                    FROM ""PlayerWarStats""
                    GROUP BY ""PlayerTag""
                ),
                
                ""RecentWarsArray"" AS (
                    SELECT 
                        ""PlayerTag"",
                        ARRAY_AGG(""AttacksUsed"" ORDER BY ""WarRank"" ASC) FILTER (WHERE ""WarRank"" <= 5) AS ""RecentWarsAttacks""
                    FROM (
                        SELECT 
                            ""PlayerTag"", 
                            ""AttacksUsed"",
                            ROW_NUMBER() OVER (PARTITION BY ""PlayerTag"" ORDER BY ""StartTime"" DESC) as ""WarRank""
                        FROM ""PlayerWarStats""
                    ) ""RankedWars""
                    GROUP BY ""PlayerTag""
                )

                SELECT 
                    Cm.""Tag"" AS ""Tag"",
                    Cm.""Name"" AS ""Name"",
                    COALESCE(Wm.""AvgMapPosition"", 0) AS ""AverageMapPosition"",
                    COALESCE(Am.""AvgOpponentMapPosition"", 0) AS ""AverageOpponentMapPosition"",
                    COALESCE(Wm.""AvgTh"", Cm.""TownHallLevel"") AS ""AverageTownHallLevel"",
                    COALESCE(Am.""AvgOpponentTh"", 0) AS ""AverageOpponentTownHallLevel"",
                    COALESCE(Wm.""AttackParticipationRate"", 0) AS ""AttackParticipationRate"",
                    COALESCE(Am.""AvgDestruction"", 0) AS ""AverageDestructionPercentage"",
                    COALESCE(Wm.""AvgStarsPerWar"", 0) AS ""AverageStarsPerWar"",
                    COALESCE(Wm.""FirstAttackParticipationRate"", 0)::real AS ""FirstAttackParticipationRate"",
                    COALESCE(Wm.""SecondAttackParticipationRate"", 0)::real AS ""SecondAttackParticipationRate"",
                    COALESCE(Am.""MirrorRate"", 0)::real AS ""MirrorAttacksRate"",
                    COALESCE(Am.""SameTh3StarRate"", 0)::real AS ""ThreeStarRateAgainstSameTh"",
                    COALESCE(Am.""AvgThMismatch"", 0) AS ""AverageThMismatches"",
                    COALESCE(Rwa.""RecentWarsAttacks"", ARRAY[]::smallint[]) AS ""RecentWarsAttacks""
                FROM public.clan_members Cm
                LEFT JOIN ""WarMetrics"" Wm ON Wm.""PlayerTag"" = Cm.""Tag""
                LEFT JOIN ""AttackMetrics"" Am ON Am.""PlayerTag"" = Cm.""Tag""
                LEFT JOIN ""RecentWarsArray"" Rwa ON Rwa.""PlayerTag"" = Cm.""Tag""
                WHERE Cm.""IsNowInClan"" = true;

                CREATE UNIQUE INDEX idx_mv_clan_war_player_summaries_tag 
                ON mv_clan_war_player_summaries(""Tag"");
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP MATERIALIZED VIEW IF EXISTS mv_clan_war_player_summaries;");
        }
    }
}
