using AnspiritConsoleUI.Constants;
using AnspiritConsoleUI.Models.Database;
using AnspiritConsoleUI.Services;
using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands
{
    [RequireOwner]
    [Group("dev")]
    public class DeveloperModule : AnspiritModuleBase
    {
        public AnspiritDatabaseService DbService { get; set; }
        public AnzacSpiritService AnzacSpiritService { get; set; }
        [Command("portplayers")]
        [Summary("Ports the text file to the MySQL database")]
        [Obsolete]
        public async Task PortPlayers()
        {
            // Redundant code now
            var usersLines = File.ReadAllLines(DiscordConstants.AnzacSpiritPlayers).Select(x => x.Split(' ', 2));
            var users = usersLines.Select(x => new IngamePlayerDiscordLinkModel
            {
                DiscordId = ulong.Parse(x[0]),
                InGameName = x[1]
            });
            var count = 0;
            Parallel.ForEach(users, async (user) =>
            {
                count++;
                await DbService.AddIngamePlayerDiscordLinkAsync(user.DiscordId, user.InGameName);
            });

            await ReplyAsync("Done porting to db, " + count);
        }

        
    }
}
