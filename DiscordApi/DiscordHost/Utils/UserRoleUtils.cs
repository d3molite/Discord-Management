using Discord.WebSocket;

namespace DiscordApi.DiscordHost.Utils
{
    public class UserRoleUtils
    {
        public async Task RemoveAllRoles(SocketGuildUser user)
        {
            var roles = user.Roles;
            await user.RemoveRolesAsync(roles);
        }

        public async Task TimeOutUser(SocketGuildUser user, SocketRole timeoutRole)
        {
            var roles = user.Roles;
            await user.RemoveRolesAsync(roles);
            await user.AddRoleAsync(timeoutRole);
        }
    }
}
