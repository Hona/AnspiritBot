using Discord;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Services
{
    public static class DiscordService
    {
        public static async Task DirectMessageUserAsync(Embed embed, IUser user)
        {
            var dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(embed: embed);
        }
    }
}
