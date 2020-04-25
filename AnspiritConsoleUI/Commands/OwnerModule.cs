using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnspiritConsoleUI.Services;

namespace AnspiritConsoleUI.Commands
{
    [RequireOwner]
    public class OwnerModule : AnspiritModuleBase
    {
        public AnspiritDatabaseService DbService { get; set; }
        [Command("spiritadmin add")]
        [Summary("Adds a discord user to the spirit admin list")]
        public async Task AddSpiritAdmin(IUser discordUser)
        {
            await DbService.AddAnspiritAdminAsync(discordUser.Id);
            await ReplyNewEmbed($"Added {discordUser.Username} to the Anzac Spirit admins", Color.Green);
        }
        [Command("spiritadmin remove")]
        [Alias("spiritadmin delete", "spiritadmin del", "spiritadmin rem")]
        [Summary("Removes a player from the spirit admin list")]
        public async Task RemoveSpiritAdmin(IUser discordUser)
        {
            await DbService.RemoveAnspiritAdminAsync(discordUser.Id);
            await ReplyNewEmbed($"Removed {discordUser.Username} from the Anzac Spirit admins", Color.Green);
        }
        [Command("spiritadmin list")]
        [Alias("spiritadmin ls", "spiritadmin get")]
        [Summary("Returns a list of the current spirit admins")]
        public async Task ListSpiritAdmin()
        {
            var admins = DbService.GetAnspiritAdmins();
            var embedBuilder = new EmbedBuilder
            {
                Title = "Anzac Spirit Admins",
                Timestamp = DateTime.Now,
                Color = Color.Purple,
            };


            StringBuilder adminLinesBuilder = new StringBuilder();
            foreach (var admin in admins)
            {

                var user = Context.Guild.Users.FirstOrDefault(x => x.Id == admin.DiscordId);
                if (user == null)
                {
                    adminLinesBuilder.Append($"User with id {admin.DiscordId} not found in the guild, but is registerd as an Anzac Spirit admin");
                }
                else
                {
                    adminLinesBuilder.Append(user.Nickname ?? user.Username);
                }

                adminLinesBuilder.Append(Environment.NewLine);
            }

            embedBuilder.Description = adminLinesBuilder.ToString();

            await ReplyAsync(embed: embedBuilder.Build());
        }
    }
}
