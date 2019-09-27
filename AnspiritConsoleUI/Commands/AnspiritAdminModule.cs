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
    public class AnspiritAdminModule : AnspiritModuleBase
    {
        public AnspiritDatabaseService DbService { get; set; }

        [Command("warofficer add")]
        [Summary("Adds a discord user the the war officers, they can now run all the order related commands.")]
        public async Task AddWarOfficer(IUser user)
        {
            await DbService.AddWarOfficerAsync(user.Id);
            await ReplyNewEmbed($"Added {user.Username} to the war officers", Color.Green);   
        }
        [Command("warofficer remove")]
        [Alias("warofficer delete", "warofficer del", "warofficer rem")]
        [Summary("Removes a user from the war officer list.")]
        public async Task RemoveWarOfficer(IUser user)
        {
            await DbService.RemoveWarOfficerAsync(user.Id);
            await ReplyNewEmbed($"Removed {user.Username} from the war officers", Color.Green);
        }
        [Command("warofficer list")]
        [Alias("warofficer ls", "warofficer get")]
        [Summary("Returns a list of the current war officers, if the user has left the guild, it will show the users ID for easy removal.")]
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

        [Command("playerlink add")]
        [Summary("Adds a link between a users discord and in-game name, this is required for the !sendorders command. This should be done when a player joins the guild.")]
        public async Task AddPlayerlink(IUser user, string playername)
        {
            await DbService.AddIngamePlayerDiscordLinkAsync(user.Id, playername);
            await ReplyNewEmbed($"Added the in game player '{playername}' to discord user {user.Username}", Color.Green);
        }
        [Command("playerlink remove")]
        [Alias("playerlink delete", "playerlink del", "playerlink rem")]
        [Summary("Removes the link between a users discord and in-game name, this should be done when a player leaves the guild.")]
        public async Task RemovePlayerlink(string playername)
        {
            await DbService.RemoveIngamePlayerDiscordLinkAsync(playername);
            await ReplyNewEmbed($"Removed in game player {playername} from linked users", Color.Green);
        }
        [Command("playerlink list")]
        [Alias("playerlink ls", "playerlink get")]
        [Summary("Returns a list of the current links of ingame names with discord.")]
        public async Task ListPlayerlink()
        {
            var userLinks = DbService.GetInGamePlayerDiscordLinks().OrderBy(x => x.InGameName);
            var embedBuilder = new EmbedBuilder
            {
                Title = "Anzac Spirit In Game Links",
                Timestamp = DateTime.Now,
                Color = Color.Purple,
            };


            StringBuilder userLinkLinesBuilder = new StringBuilder();
            foreach (var userLink in userLinks)
            {

                var user = Context.Guild.Users.FirstOrDefault(x => x.Id == userLink.DiscordId);
                if (user == null)
                {
                    userLinkLinesBuilder.Append($"{userLink.InGameName} | User not found, id: {userLink.DiscordId}");
                }
                else
                {
                    userLinkLinesBuilder.Append($"{userLink.InGameName} | {user.Nickname ?? user.Username}#{user.Discriminator}");
                }

                userLinkLinesBuilder.Append(Environment.NewLine);
            }
            var output = userLinkLinesBuilder.ToString();

            if (output.Length < 2048)
            {
                embedBuilder.Description = output;
                await ReplyAsync(embed: embedBuilder.Build());
            }
            else
            {
                var newLineIndex = -1;
                for (int i = 2047; i > 0; i--)
                {
                    var searchedString = output.Substring(i, Environment.NewLine.Length);
                    if (searchedString == Environment.NewLine)
                    {
                        newLineIndex = i;
                        break;
                    }
                }

                if (newLineIndex == -1)
                {
                    // No newlines, just split it at the end
                    newLineIndex = 2047;
                }
                var firstDescription = output.Take(newLineIndex).ToArray();
                embedBuilder.Description = new string(firstDescription);
                embedBuilder.Timestamp = null;
                await ReplyAsync(embed: embedBuilder.Build());

                var secondDescription = output.Skip(newLineIndex + Environment.NewLine.Length).ToArray();
                var secondEmbedBuilder = new EmbedBuilder
                {
                    Color = Color.Purple,
                    Timestamp = DateTime.Now
                };
                secondEmbedBuilder.Description = new string(secondDescription);
                await ReplyAsync(embed: secondEmbedBuilder.Build());
            }
            

            
        }
    }
}
