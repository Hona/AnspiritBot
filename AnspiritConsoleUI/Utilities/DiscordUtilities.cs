using Discord;
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
    }
}
