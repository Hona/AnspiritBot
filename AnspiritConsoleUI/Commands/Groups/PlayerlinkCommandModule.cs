using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnspiritConsoleUI.Commands.Preconditions;
using AnspiritConsoleUI.Services;
using Discord;
using Discord.Commands;

namespace AnspiritConsoleUI.Commands.Groups
{
    [RequireAnspiritAdminPrecondition]
    [Group("playerlink")]
    [Alias("pl")]
    public class PlayerlinkCommandModule : AnspiritModuleBase
    {
        public AnspiritDatabaseService DbService { get; set; }
        [Command("add")]
        [Summary("Adds a link between a users discord and in-game name, this is required for the !sendorders command. This should be done when a player joins the guild")]
        public async Task AddPlayerlink(IUser discordUser, string playerInGameName)
        {
            await DbService.AddIngamePlayerDiscordLinkAsync(discordUser.Id, playerInGameName);
            await ReplyNewEmbed($"Added the in game player '{playerInGameName}' to discord user {discordUser.Username}", Color.Green);
        }
        [Command("remove")]
        [Alias("delete", "del", "rem")]
        [Summary("Removes the link between a users discord and in-game name, this should be done when a player leaves the guild")]
        public async Task RemovePlayerlink(string playerInGameName)
        {
            await DbService.RemoveIngamePlayerDiscordLinkAsync(playerInGameName);
            await ReplyNewEmbed($"Removed in game player {playerInGameName} from linked users", Color.Green);
        }
        [Command("list")]
        [Alias("ls", "get")]
        [Summary("Returns a list of the current links of ingame names with discord")]
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

                var user = Context.Guild?.Users.FirstOrDefault(x => x.Id == userLink.DiscordId);
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
