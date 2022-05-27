using DiscordApi.Models;
using FluentValidation;

namespace DiscordApi.Validations;

public class ReactionRoleValidator : AbstractValidator<ReactionRoleConfig>
{
    public ReactionRoleValidator()
    {
        RuleFor(x => x.RelatedMessage)
            .NotNull()
            .WithMessage("Message must be selected.");

        RuleFor(x => x.RelatedEmoji)
            .NotNull()
            .WithMessage("Emoji must be selected.");

        RuleFor(x => x.RelatedRole)
            .NotNull()
            .WithMessage("Role must be selected.");
    }
}