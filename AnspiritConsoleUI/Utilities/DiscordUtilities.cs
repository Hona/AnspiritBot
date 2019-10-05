using AnspiritConsoleUI.Constants;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Utilities
{
    public static class DiscordUtilities
    {
        public static async Task DirectMessageUserAsync(Embed embed, IUser user)
        {
            var dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(embed: embed);
        }

        public static bool UserIsInCallingOfficersGuild(SocketGuildUser officer, SocketGuildUser user)
        {
            var officerCallableRoles = officer.Roles.Where(x => x.Name.ToLower().Contains(DiscordConstants.OfficerNotesRoleSearcher));
            var officerCallableGuilds = officerCallableRoles.Select(x => x.Name.Split(' ', 2)[0].ToLower());
            return user.Roles.Any(x => officerCallableGuilds.Contains(x.Name.Split(' ', 2)[0].ToLower()));
        }
    }
}
