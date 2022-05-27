using DiscordApi.Models;
using FluentValidation;

namespace DiscordApi.Validations;

public class LoggingConfigValidator : AbstractValidator<LoggingConfig>
{
    public LoggingConfigValidator()
    {
        RuleFor(x => x.LoggingChannelID)
            .Must(y => y.ToString().Length == 18)
            .WithMessage("Channel ID must be 18 characters long.");
    }
}