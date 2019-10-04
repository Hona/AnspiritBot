using AnspiritConsoleUI.Constants;
using Discord.Commands;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands
{
    public class GeneralModule : AnspiritModuleBase
    {
        [Command("zoneoverview")]
        [Summary("Sends an image of the ANZAC Spirit zone terms")]
        public async Task SendZoneOverviewAsync()
        {
            await Context.Channel.SendFileAsync(DiscordConstants.ZoneOverviewFilePath);
        }
    }
}
