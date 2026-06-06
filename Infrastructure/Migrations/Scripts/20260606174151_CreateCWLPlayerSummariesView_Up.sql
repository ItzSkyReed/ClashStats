DROP MATERIALIZED VIEW IF EXISTS "mv_clan_war_leagues_player_summaries";

CREATE MATERIALIZED VIEW "mv_clan_war_leagues_player_summaries" AS

WITH "CwlPerformances" AS (
    -- Собираем все сыгранные матчи ЛВК, связываясь по WarTag
    SELECT Cwlpp.*,
           Cwlw."Round",
           Cwlw."Season"
    FROM public.clan_war_league_player_performances Cwlpp
             JOIN public.clan_war_league_wars Cwlw ON Cwlw."WarTag" = Cwlpp."WarTag"
    WHERE Cwlw."State" = 'warEnded'),

     "CwlMetrics" AS (SELECT "Season",
                             "PlayerTag",
                             AVG("MapPosition")::real                                                                                           AS "AvgMapPosition",
                             AVG("OpponentPosition")::real                                                                                      AS "AvgOpponentMapPosition",
                             AVG("TownHallLevel")::real                                                                                         AS "AvgTh",
                             AVG("OpponentTownHallLevel")::real                                                                                 AS "AvgOpponentTh",
                             AVG("OpponentTownHallLevel" - "TownHallLevel")::real                                                               AS "AvgThMismatch",

                             COUNT(*)::integer                                                                                                  AS "TotalRounds",

                             (COUNT(*) FILTER (WHERE "AttackStars" IS NOT NULL)::real / NULLIF(COUNT(*), 0)) * 100                              AS "ParticipationRate",
                             AVG("AttackDestruction")::real                                                                                     AS "AvgDestruction",
                             AVG("AttackStars")::real                                                                                           AS "AvgStarsPerAttack",

                             (COUNT(*) FILTER (WHERE "AttackStars" = 3 AND "OpponentTownHallLevel" = "TownHallLevel")::real /
                              NULLIF(COUNT(*) FILTER (WHERE "OpponentTownHallLevel" = "TownHallLevel" AND "AttackStars" IS NOT NULL), 0)) * 100 AS "SameTh3StarRate",

                             (COUNT(*) FILTER (WHERE "OpponentPosition" = "MapPosition" AND "AttackStars" IS NOT NULL)::real /
                              NULLIF(COUNT(*) FILTER (WHERE "AttackStars" IS NOT NULL), 0)) * 100                                               AS "MirrorRate"

                      FROM "CwlPerformances"
                      GROUP BY "Season", "PlayerTag"),

     "DistinctSeasons" AS (SELECT DISTINCT "Season"
                           FROM "CwlPerformances"),

     "RoundsGenerator" AS (SELECT Ds."Season",
                                  Cm."Tag" AS "PlayerTag",
                                  R."Rnd"  AS "RoundNum"
                           FROM public.clan_members Cm
                                    CROSS JOIN "DistinctSeasons" Ds
                                    CROSS JOIN (SELECT generate_series(1, 7) AS "Rnd") R
                           WHERE Cm."IsNowInClan" = true),

     "CwlHistoryPerSeason" AS (SELECT Rg."Season",
                                      Rg."PlayerTag",
                                      COALESCE(
                                              CASE
                                                  WHEN Cp."PlayerTag" IS NULL THEN -1::smallint
                                                  WHEN Cp."AttackStars" IS NULL THEN 0::smallint
                                                  ELSE Cp."AttackStars"::smallint
                                                  END,
                                              -2::smallint
                                      ) AS "RoundResult",
                                      Rg."RoundNum"
                               FROM "RoundsGenerator" Rg
                                        LEFT JOIN "CwlPerformances" Cp ON Cp."PlayerTag" = Rg."PlayerTag"
                                   AND Cp."Round" = Rg."RoundNum"
                                   AND Cp."Season" = Rg."Season"),

     "CwlRecentArray" AS (SELECT "Season",
                                 "PlayerTag",
                                 ARRAY_AGG("RoundResult" ORDER BY "RoundNum") AS "RecentCwlRoundsAttacks"
                          FROM "CwlHistoryPerSeason"
                          GROUP BY "Season", "PlayerTag")

SELECT M."Season"                                                   AS "Season",
       Cm."Tag"                                                     AS "Tag",
       Cm."Name"                                                    AS "Name",
       COALESCE(M."AvgMapPosition", 0)                              AS "AverageMapPosition",
       COALESCE(M."AvgOpponentMapPosition", 0)                      AS "AverageOpponentMapPosition",
       COALESCE(M."AvgTh", Cm."TownHallLevel")                      AS "AverageTownHallLevel",
       COALESCE(M."AvgOpponentTh", 0)                               AS "AverageOpponentTownHallLevel",
       COALESCE(M."AvgThMismatch", 0)                               AS "AverageThMismatches",
       COALESCE(M."TotalRounds", 0)                                 AS "TotalRoundsOnMap",
       COALESCE(M."ParticipationRate", 0)                           AS "AttackParticipationRate",
       COALESCE(M."AvgDestruction", 0)                              AS "AverageDestructionPercentage",
       COALESCE(M."AvgStarsPerAttack", 0)                           AS "AverageStarsPerAttack",
       COALESCE(M."SameTh3StarRate", 0)::real                       AS "ThreeStarRateAgainstSameTh",
       COALESCE(M."MirrorRate", 0)::real                            AS "MirrorAttacksRate",
       COALESCE(Rwa."RecentCwlRoundsAttacks", ARRAY []::smallint[]) AS "RecentCwlRoundsAttacks"
FROM public.clan_members Cm
         JOIN "CwlMetrics" M ON M."PlayerTag" = Cm."Tag"
         LEFT JOIN "CwlRecentArray" Rwa ON Rwa."PlayerTag" = Cm."Tag" AND Rwa."Season" = M."Season"
WHERE Cm."IsNowInClan" = true;

CREATE UNIQUE INDEX idx_mv_clan_war_leagues_player_summaries_season_tag
    ON mv_clan_war_leagues_player_summaries ("Season", "Tag");