DROP MATERIALIZED VIEW IF EXISTS "mv_clan_war_league_group_summaries";

CREATE MATERIALIZED VIEW "mv_clan_war_league_group_summaries" AS

WITH "WarMetrics" AS (SELECT "Season",
                             COUNT(*)::smallint                    AS "RoundsCompleted",
                             COUNT(*) FILTER (
                                 WHERE "OurStars" > "OpponentStars"
                                     OR ("OurStars" = "OpponentStars" AND "OurDestructionPercentage" > "OpponentDestructionPercentage")
                                 )::smallint                       AS "TotalWins",
                             COUNT(*) FILTER (
                                 WHERE "OpponentStars" > "OurStars"
                                     OR ("OurStars" = "OpponentStars" AND "OpponentDestructionPercentage" > "OurDestructionPercentage")
                                 )::smallint                       AS "TotalLosses",
                             SUM("OurAttacks")::smallint           AS "TotalOurAttacks",
                             SUM("OurStars")::smallint             AS "TotalOurStars",
                             SUM("OpponentAttacks")::smallint      AS "TotalOpponentAttacks",
                             SUM("OpponentStars")::smallint        AS "TotalOpponentStars",
                             AVG("OurDestructionPercentage")::real AS "AverageOurDestructionPercentage"
                      FROM public.clan_war_league_wars
                      WHERE "State" = 'warEnded'
                      GROUP BY "Season"),

     "PerformanceMetrics" AS (SELECT Cwlw."Season",
                                     COUNT(*) FILTER (WHERE Cwlpp."AttackStars" = 3)::smallint AS "TotalOurThreeStars",
                                     AVG(Cwlpp."AttackStars")::real                            AS "AverageStarsPerAttack",
                                     AVG(Cwlpp."TownHallLevel")::real                          AS "AverageOurTownHall",
                                     AVG(Cwlpp."OpponentTownHallLevel")::real                  AS "AverageOpponentTownHall"
                              FROM public.clan_war_league_player_performances Cwlpp
                                       JOIN public.clan_war_league_wars Cwlw ON Cwlw."WarTag" = Cwlpp."WarTag"
                              WHERE Cwlw."State" = 'warEnded'
                              GROUP BY Cwlw."Season")

SELECT G."Season",
       G."State",
       G."TeamSize",
       G."Place",
       COALESCE(W."TotalWins", 0)::smallint                                    AS "TotalWins",
       COALESCE(W."TotalLosses", 0)::smallint                                  AS "TotalLosses",
       COALESCE(W."TotalOurAttacks", 0)::smallint                              AS "TotalOurAttacks",
       COALESCE(W."TotalOurStars", 0)::smallint                                AS "TotalOurStars",
       COALESCE(P."TotalOurThreeStars", 0)::smallint                           AS "TotalOurThreeStars",
       COALESCE(W."TotalOpponentAttacks", 0)::smallint                         AS "TotalOpponentAttacks",
       COALESCE(W."TotalOpponentStars", 0)::smallint                           AS "TotalOpponentStars",
       COALESCE(
               (W."TotalOurAttacks"::real / NULLIF(W."RoundsCompleted" * G."TeamSize", 0)) * 100,
               0)::real                                                        AS "ParticipationRate",
       COALESCE(P."AverageStarsPerAttack", 0)::real                            AS "AverageStarsPerAttack",
       COALESCE(W."AverageOurDestructionPercentage", 0)::real                  AS "AverageOurDestructionPercentage",
       COALESCE(P."AverageOurTownHall", 0)::real                               AS "AverageOurTownHall",
       COALESCE(P."AverageOpponentTownHall", 0)::real                          AS "AverageOpponentTownHall",
       COALESCE(P."AverageOurTownHall" - P."AverageOpponentTownHall", 0)::real AS "TownHallAdvantage"
FROM public.clan_war_league_groups G
         LEFT JOIN "WarMetrics" W ON W."Season" = G."Season"
         LEFT JOIN "PerformanceMetrics" P ON P."Season" = G."Season";

CREATE UNIQUE INDEX idx_mv_clan_war_league_group_summaries_season
    ON mv_clan_war_league_group_summaries ("Season");