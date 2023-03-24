using Discord;

namespace BotModule.Extensions.Logging;

public record LoggingInfo(IGuild Guild, LoggingExtension Logger);