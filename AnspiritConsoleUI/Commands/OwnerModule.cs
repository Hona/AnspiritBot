using AnspiritConsoleUI.Services.Database;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands
{
    [RequireOwner]
    public class OwnerModule : AnspiritModuleBase
    {
        public AnspiritDatabaseService DbService { get; set; }
        [Command("spiritadmin add")]
        public async Task AddSpiritAdmin(IUser user)
        {
            await DbService.AddAnspiritAdminAsync(user.Id);
            await ReplyNewEmbed($"Added {user.Username} to the Anzac Spirit admins", Color.Green);
        }
        [Command("spiritadmin remove")]
        [Alias("spiritadmin delete", "spiritadmin del", "spiritadmin rem")]
        public async Task RemoveSpiritAdmin(IUser user)
        {
            await DbService.RemoveAnspiritAdminAsync(user.Id);
            await ReplyNewEmbed($"Removed {user.Username} from the Anzac Spirit admins", Color.Green);
        }
        [Command("spiritadmin list")]
        [Alias("spiritadmin ls", "spiritadmin get")]
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
