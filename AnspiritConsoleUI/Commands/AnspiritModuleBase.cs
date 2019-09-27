using AnspiritConsoleUI.Services;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands
{
    public abstract class AnspiritModuleBase : ModuleBase<SocketCommandContext>
    {
        public LogService Logger { get; set; }
        protected async Task<IUserMessage> ReplyNewEmbed(string text, Color color)
        {
            var embed = EmbedService.CreateEmbed(text, color);
            return await ReplyAsync(embed: embed);
        }
    }
}
