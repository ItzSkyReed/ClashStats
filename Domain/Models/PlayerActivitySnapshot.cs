using System.Text.Json.Serialization;

namespace Domain.Models;

public class PlayerActivitySnapshot
{
    public long Id { get; set; }

    public int MemberInternalId { get; set; }

    public DateTimeOffset SnapshotTime { get; set; }

    [JsonIgnore] public ClanMember? Member { get; set; }
}