using AnspiritConsoleUI.Constants;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands
{
    public class GeneralModule : AnspiritModuleBase
    {
        [Command("zoneoverview")]
        public async Task SendZoneOverviewAsync()
        {
            await Context.Channel.SendFileAsync(DiscordConstants.ZoneOverviewFilePath);
        }
    }
}
