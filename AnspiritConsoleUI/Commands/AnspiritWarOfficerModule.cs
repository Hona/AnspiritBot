using AnspiritConsoleUI.Services;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
using AnspiritConsoleUI.Models;
using System.Collections.Generic;
using System;
using Discord;
using AnspiritConsoleUI.Services.Database;

namespace AnspiritConsoleUI.Commands
{
    [RequireWarOfficerPrecondition]
    public class AnspiritWarOfficerModule : AnspiritModuleBase
    {
        public AnzacSpiritService AnzacSpiritService { get; set; }
        public AnspiritDatabaseService DbService { get; set; }
        public CommandService CommandService { get; set; }
        public IServiceProvider Services { get; set; }

        [Command("sendorders")]
        [Summary("DM's each user in the guild their orders (only if they have deployments), it is recommended to run !verifyorders first.")]
        public async Task SendOrders()
        {
            var finalOrders = AnzacSpiritService.GetWarOrdersSortedByDiscordUser();

            // DM to each user
            Parallel.ForEach(finalOrders, async (KeyValuePair<ulong, List<Tuple<string, Deployment>>> playerOrder) => 
            {
                var user = Context.Guild.Users.First(x => x.Id == playerOrder.Key);
                var embed = AnzacSpiritService.GetPlayerOrdersEmbed(playerOrder);
                await DiscordService.DirectMessageUserAsync(embed, user);
            });

            await ReplyNewEmbed("Success", Color.Purple);
        }

        [Command("sendorder")]
        [Summary("Did a player not get their order/you want to remind them/updated their orders? Run this command to only send one user their orders.")]
        public async Task SendOrder(string player)
        {
            var finalOrders = AnzacSpiritService.GetWarOrdersSortedByDiscordUser();
            var playerDiscords = DbService.GetInGamePlayerDiscordLinks();
            // TODO: Refactor this searching for id
            var playerId = playerDiscords.First(x => x.InGameName.Trim().ToLower() == player.Trim().ToLower()).DiscordId;
            
            var user = Context.Guild.Users.First(x => x.Id == playerId);
            var embed = AnzacSpiritService.GetPlayerOrdersEmbed(finalOrders.First(x => x.Key == playerId));
            await DiscordService.DirectMessageUserAsync(embed, user);
        }

        [Command("getorder")]
        [Summary("Returns a players orders in the current channel (will not send it to them, useful for seeing a particular players orders)")]
        public async Task GetOrder(string player)
        {
            var finalOrders = AnzacSpiritService.GetWarOrdersSortedByDiscordUser();
            var playerDiscords = DbService.GetInGamePlayerDiscordLinks();
            var playerId = playerDiscords.First(x => x.InGameName.Trim().ToLower() == player.Trim().ToLower()).DiscordId;

            var embed = AnzacSpiritService.GetPlayerOrdersEmbed(finalOrders.First(x => x.Key == playerId));
            await ReplyAsync(embed: embed);
        }

        [Command("verifyorders")]
        [Summary("Checks that there are no duplicate deployments for a players team (each team can only be deployed once). More features will come.")]
        public async Task VerifyOrders()
        {
            var outputEmbed = new EmbedBuilder
            {
                Title = "Verify Orders",
                Timestamp = DateTime.Now,
                Color = Color.Green
            };

            var finalOrders = AnzacSpiritService.GetWarOrdersSortedByDiscordUser();

            // Loop through each user
            foreach (var order in finalOrders)
            {
                var duplicates = order.Value.GroupBy(x => x.Item2.Team).Where(y => y.Skip(1).Any()).SelectMany(z=>z);
                if (duplicates.Any())
                {
                    // There is a duplicate in the orders, making the orders invalid
                    if (outputEmbed.Color != Color.Red)
                    {
                        outputEmbed.Color = Color.Red;
                        outputEmbed.Title += " - Invalid";
                    }

                    outputEmbed.AddField(duplicates.First().Item2.Player, string.Join(", ", duplicates.Select(x => x.Item2.Team + $" ({x.Item1})")));
                }
            }

            if (!outputEmbed.Fields.Any())
            {
                outputEmbed.Title += " - Valid";
                outputEmbed.Description = "No duplicate orders, ready to send.";
            }

            await ReplyAsync(embed: outputEmbed.Build());
        }
        [Command("help")]
        [Summary("The command you are running")]
        public async Task HelpAsync()
        {
            var message = await ReplyNewEmbed("Building the help command... This message will be deleted when all help messages are sent", Color.Purple);
            foreach(var module in CommandService.Modules.Where(x => !x.Name.Contains("ModuleBase")))
            {
                var moduleHelpEmbed = HelpCommandService.GetModuleHelpEmbed(module, Context, Services);
                if (moduleHelpEmbed.Fields.Length > 0)
                {
                    await ReplyAsync(embed: moduleHelpEmbed);

                }
            }
            await message.DeleteAsync();
        }
    }
}
