using Application.DTOs.Clans.ClanWarLeagues;
using Application.DTOs.Clans.ClanWars;
using Application.DTOs.Players;
using Domain.Constants;
using Domain.Models;
using Domain.Models.ClanWarLeagues;
using Domain.Models.ClanWars;

namespace Application.Extensions;

public static class ClanModelsMappingExtensions
{
    public static void UpdateFromPlayerDto(this ClanMember entity, in PlayerDto playerApiData)
    {
        entity.Name = playerApiData.Name;
        entity.Trophies = (short)playerApiData.Trophies;
        entity.BestBuilderBaseTrophies = (short)playerApiData.BestBuilderBaseTrophies;
        entity.BuilderBaseTrophies = (short)playerApiData.BuilderBaseTrophies;
        entity.ExpLevel = (short)playerApiData.ExpLevel;
        entity.Role = playerApiData.Role;
        entity.TownHallLevel = (short)playerApiData.TownHallLevel;
        entity.BuilderHallLevel = (short)playerApiData.BuilderHallLevel;
        entity.WarPreference = playerApiData.WarPreference;
        entity.ClanCapitalContributions = playerApiData.ClanCapitalContributions;
        entity.WarStars = playerApiData.WarStars;
    }

    public static void UpdateFromClanWarMemberDto(
        this ClanWarLeaguePlayerPerformance entity,
        ClanWarMemberDto memberDto,
        Dictionary<string, ClanWarMemberDto> opponentsLookup)
    {
        entity.MapPosition = (short)memberDto.MapPosition;
        entity.TownHallLevel = (short)memberDto.TownhallLevel;

        // В CWL всегда 1 атака, берем первую или null
        var attack = memberDto.Attacks?.FirstOrDefault();

        entity.AttackStars = (short?)attack?.Stars;
        entity.DefenderTag = attack?.DefenderTag;
        entity.AttackDestruction = (short?)attack?.DestructionPercentage;
        entity.AttackDuration = (short?)attack?.Duration;

        // Ищем противника, которого атаковал игрок
        if (attack?.DefenderTag != null && opponentsLookup.TryGetValue(attack.DefenderTag, out var defender))
        {
            entity.OpponentPosition = (short)defender.MapPosition;
            entity.OpponentTownHallLevel = (short)defender.TownhallLevel;
        }
        else
        {
            entity.OpponentPosition = null;
            entity.OpponentTownHallLevel = null;
        }
    }


    public static void UpdateFromClanWarLeaguerWarDto(this ClanWarLeagueWar entity, in ClanWarLeaguerWarDto apiData, string ourClanTag)
    {
        entity.State = apiData.State!;
        entity.EndTime = apiData.EndTime;
        entity.StartTime = apiData.StartTime;
        entity.WarStartTime = apiData.WarStartTime;

        // Определяем, какая сторона наш клан, а какая противник
        var our = apiData.Clan.Tag == ourClanTag ? apiData.Clan : apiData.Opponent;
        var opp = apiData.Clan.Tag == ourClanTag ? apiData.Opponent : apiData.Clan;

        entity.OpponentAttacks = (short)opp.Attacks;
        entity.OpponentClanLevel = (short)opp.ClanLevel;
        entity.OpponentClanName = opp.Name!;
        entity.OpponentClanTag = opp.Tag!;
        entity.OpponentDestructionPercentage = opp.DestructionPercentage;
        entity.OpponentStars = (short)opp.Stars;

        entity.OurAttacks = (short)our.Attacks;
        entity.OurStars = (short)our.Stars;
        entity.OurDestructionPercentage = our.DestructionPercentage;
    }

    extension(ClanWar entity)
    {
        public void UpdateStuckWarFromWarLogEntryDto(in ClanWarLogEntryDto apiData)
        {
            entity.State = ClanWarState.WarEnded;
            entity.EndTime = apiData.EndTime;
            if (entity.StartTime == DateTime.MinValue)
                entity.StartTime = apiData.EndTime - new TimeSpan(48, 0, 0); // Approximately

            entity.ExpEarned = (short?)apiData.Clan.ExpEarned;
            entity.TeamSize = (short)apiData.TeamSize;

            entity.OpponentAttacks = (short)apiData.Opponent.Attacks;
            entity.OpponentClanLevel = (short)apiData.Opponent.ClanLevel;
            entity.OpponentClanName = apiData.Opponent.Name;
            entity.OpponentClanTag = apiData.Opponent.Tag;
            entity.OpponentDestructionPercentage = apiData.Opponent.DestructionPercentage;
            entity.OpponentStars = (short)apiData.Opponent.Stars;

            entity.OurAttacks = (short)apiData.Clan.Attacks;
            entity.OurDestructionPercentage = apiData.Clan.DestructionPercentage;
            entity.OurStars = (short)apiData.Clan.Stars;
        }

        public void UpdateFromClanWarDto(in ClanWarDto apiData)
        {
            entity.State = apiData.State;
            entity.EndTime = apiData.EndTime;
            entity.StartTime = apiData.StartTime;
            entity.ExpEarned = (short?)apiData.Clan.ExpEarned;
            entity.TeamSize = (short)apiData.TeamSize!;

            entity.OpponentAttacks = (short)apiData.Opponent!.Attacks;
            entity.OpponentClanLevel = (short)apiData.Opponent.ClanLevel;
            entity.OpponentClanName = apiData.Opponent.Name;
            entity.OpponentClanTag = apiData.Opponent.Tag;
            entity.OpponentDestructionPercentage = apiData.Opponent.DestructionPercentage;
            entity.OpponentStars = (short)apiData.Opponent.Stars;

            entity.OurAttacks = (short)apiData.Clan.Attacks;
            entity.OurDestructionPercentage = apiData.Clan.DestructionPercentage;
            entity.OurStars = (short)apiData.Clan.Stars;
        }
    }
}