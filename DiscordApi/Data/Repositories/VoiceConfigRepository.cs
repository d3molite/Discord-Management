using DiscordApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordApi.Data.Repositories;

public static class VoiceConfigRepository
{
    public static List<VoiceChannelConfig> Get()
    {
        using var db = AppDBContext.Get();

        return db.VoiceChannelConfigs.ToList();
    }

    public static VoiceChannelConfig? Get(ulong guildId)
    {
        using var db = AppDBContext.Get();

        return db.VoiceChannelConfigs
            .Include(x => x.VoiceGuild)
            .FirstOrDefault(x => x.VoiceGuild.GuildID == guildId);
    }
}