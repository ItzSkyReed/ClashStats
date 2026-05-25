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
                CREATE MATERIALIZED VIEW mv_clan_war_player_summaries AS
                
                WITH ended_war_performances AS (
                    SELECT 
                        cwpp.*, 
                        cw.""StartTime""
                    FROM public.clan_war_player_performances cwpp
                    JOIN public.clan_wars cw ON cw.""Id"" = cwpp.""WarId""
                    WHERE cw.""State"" = 'warEnded'
                ),

                unpivoted_attacks AS (
                    SELECT 
                        ""WarId"", ""PlayerTag"", ""MapPosition"", ""TownHallLevel"",
                        ""Attack1Stars"" AS stars, 
                        ""Attack1Destruction"" AS destruction, 
                        ""Opponent1Position"" AS opponent_position, 
                        ""Opponent1TownHallLevel"" AS opponent_th
                    FROM ended_war_performances
                    WHERE ""Attack1Stars"" IS NOT NULL
                    
                    UNION ALL
                    
                    SELECT 
                        ""WarId"", ""PlayerTag"", ""MapPosition"", ""TownHallLevel"",
                        ""Attack2Stars"" AS stars, 
                        ""Attack2Destruction"" AS destruction, 
                        ""Opponent2Position"" AS opponent_position, 
                        ""Opponent2TownHallLevel"" AS opponent_th
                    FROM ended_war_performances
                    WHERE ""Attack2Stars"" IS NOT NULL
                ),
                
                attack_metrics AS (
                    SELECT 
                        ""PlayerTag"",
                        AVG(opponent_position)::real AS avg_opponent_map_position,
                        AVG(opponent_th)::real AS avg_opponent_th,
                        AVG(destruction)::real AS avg_destruction,
                        
                        (COUNT(*) FILTER (WHERE opponent_position = ""MapPosition"")::real / 
                            NULLIF(COUNT(*), 0)) * 100 AS mirror_rate,
                            
                        (COUNT(*) FILTER (WHERE stars = 3 AND opponent_th = ""TownHallLevel"")::real / 
                            NULLIF(COUNT(*) FILTER (WHERE opponent_th = ""TownHallLevel""), 0)) * 100 AS same_th_3star_rate,
                            
                        AVG(opponent_th - ""TownHallLevel"")::real AS avg_th_mismatch
                    FROM unpivoted_attacks
                    GROUP BY ""PlayerTag""
                ),

                player_war_stats AS (
                    SELECT 
                        ""PlayerTag"",
                        ""StartTime"",
                        ""MapPosition"",
                        ""TownHallLevel"",
                        (CASE WHEN ""Attack1Stars"" IS NOT NULL THEN 1 ELSE 0 END + 
                         CASE WHEN ""Attack2Stars"" IS NOT NULL THEN 1 ELSE 0 END)::smallint AS attacks_used,
                        CASE WHEN ""Attack1Stars"" IS NOT NULL THEN 1 ELSE 0 END AS first_attack_used,
                        CASE WHEN ""Attack2Stars"" IS NOT NULL THEN 1 ELSE 0 END AS second_attack_used,
                        COALESCE(""Attack1Stars"", 0) + COALESCE(""Attack2Stars"", 0) AS total_stars
                    FROM ended_war_performances
                ),
                
                war_metrics AS (
                    SELECT 
                        ""PlayerTag"",
                        AVG(""MapPosition"")::real AS avg_map_position,
                        AVG(""TownHallLevel"")::real AS avg_th,
                        (SUM(attacks_used)::real / NULLIF(COUNT(*) * 2, 0)) * 100 AS attack_participation_rate,
                        AVG(total_stars)::real AS avg_stars_per_war,
                        ((SUM(first_attack_used)::real / NULLIF(COUNT(*), 0)) * 100)::real AS first_attack_participation_rate,
                        ((SUM(second_attack_used)::real / NULLIF(COUNT(*), 0)) * 100)::real AS second_attack_participation_rate
                    FROM player_war_stats
                    GROUP BY ""PlayerTag""
                ),
                
                recent_wars_array AS (
                    SELECT 
                        ""PlayerTag"",
                        ARRAY_AGG(attacks_used ORDER BY war_rank ASC) FILTER (WHERE war_rank <= 5) AS recent_wars_attacks
                    FROM (
                        SELECT 
                            ""PlayerTag"", 
                            attacks_used,
                            ROW_NUMBER() OVER (PARTITION BY ""PlayerTag"" ORDER BY ""StartTime"" DESC) as war_rank
                        FROM player_war_stats
                    ) ranked_wars
                    GROUP BY ""PlayerTag""
                )

                SELECT 
                    cm.""Tag"" AS tag,
                    cm.""Name"" AS name,
                    COALESCE(wm.avg_map_position, 0) AS average_map_position,
                    COALESCE(am.avg_opponent_map_position, 0) AS average_opponent_map_position,
                    COALESCE(wm.avg_th, cm.""TownHallLevel"") AS average_town_hall_level,
                    COALESCE(am.avg_opponent_th, 0) AS average_opponent_town_hall_level,
                    COALESCE(wm.attack_participation_rate, 0) AS attack_participation_rate,
                    COALESCE(am.avg_destruction, 0) AS average_destruction_percentage,
                    COALESCE(wm.avg_stars_per_war, 0) AS average_stars_per_war,
                    COALESCE(wm.first_attack_participation_rate, 0)::real AS first_attack_participation_rate,
                    COALESCE(wm.second_attack_participation_rate, 0)::real AS second_attack_participation_rate,
                    COALESCE(am.mirror_rate, 0)::real AS mirror_attacks_rate,
                    COALESCE(am.same_th_3star_rate, 0)::real AS three_star_rate_against_same_th,
                    COALESCE(am.avg_th_mismatch, 0) AS average_th_mismatches,
                    COALESCE(rwa.recent_wars_attacks, ARRAY[]::smallint[]) AS recent_wars_attacks
                FROM public.clan_members cm
                LEFT JOIN war_metrics wm ON wm.""PlayerTag"" = cm.""Tag""
                LEFT JOIN attack_metrics am ON am.""PlayerTag"" = cm.""Tag""
                LEFT JOIN recent_wars_array rwa ON rwa.""PlayerTag"" = cm.""Tag""
                WHERE cm.""IsNowInClan"" = true;

                CREATE UNIQUE INDEX idx_mv_clan_war_player_summaries_tag 
                ON mv_clan_war_player_summaries(tag);
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP MATERIALIZED VIEW IF EXISTS mv_clan_war_player_summaries;");
        }
    }
}
