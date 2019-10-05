using AnspiritConsoleUI.Commands.Preconditions;
using AnspiritConsoleUI.Services.Database;
using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands
{
    [RequireAnspiritAdminPrecondition]
    [Group("warofficer")]
    [Alias("wo")]
    public class WarOfficerCommandModule : AnspiritModuleBase
    {
        public AnspiritDatabaseService DbService { get; set; }

        [Command("add")]
        [Summary("Adds a discord user the the war officers, they can now run all the order related commands")]
        public async Task AddWarOfficer(IUser discordUser)
        {
            await DbService.AddWarOfficerAsync(discordUser.Id);
            await ReplyNewEmbed($"Added {discordUser.Username} to the war officers", Color.Green);   
        }
        [Command("remove")]
        [Alias("warofficer delete", "warofficer del", "warofficer rem")]
        [Summary("Removes a user from the war officer list")]
        public async Task RemoveWarOfficer(IUser discordUser)
        {
            await DbService.RemoveWarOfficerAsync(discordUser.Id);
            await ReplyNewEmbed($"Removed {discordUser.Username} from the war officers", Color.Green);
        }
        [Command("list")]
        [Alias("warofficer ls", "warofficer get")]
        [Summary("Returns a list of the current war officers, if the user has left the guild, it will show the users ID for easy removal")]
        public async Task ListWarOfficer()
        {
            var officers = DbService.GetWarOfficers();
            var embedBuilder = new EmbedBuilder
            {
                Title = "Anzac Spirit War Officers",
                Timestamp = DateTime.Now,
                Color = Color.Purple,
            };


            StringBuilder officerLinesBuilder = new StringBuilder();
            foreach (var officer in officers)
            {
                
                var user = Context.Guild.Users.FirstOrDefault(x => x.Id == officer.DiscordId);
                if (user == null)
                {
                    officerLinesBuilder.Append($"User with id {officer.DiscordId} not found in the guild, but is registerd as a war officer");
                }
                else
                {
                    officerLinesBuilder.Append(user.Nickname ?? user.Username);
                }

                officerLinesBuilder.Append(Environment.NewLine);
            }

            embedBuilder.Description = officerLinesBuilder.ToString();

            await ReplyAsync(embed: embedBuilder.Build());
        }
    }
}
