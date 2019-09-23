using AnspiritConsoleUI.Services;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
using AnspiritConsoleUI.Models;
using System.Collections.Generic;
using System;
using Discord;

namespace AnspiritConsoleUI.Commands
{
    [RequireWarOfficerPrecondition]
    public class AnspiritModule : AnspiritModuleBase
    {
        public AnzacSpiritService AnzacSpiritService { get; set; }
        [Command("sendorders")]
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
        }

        [Command("sendorder")]
        public async Task SendOrder(string player)
        {
            var finalOrders = AnzacSpiritService.GetWarOrdersSortedByDiscordUser();
            var playerDiscords = AnzacSpiritService.GetPlayerDiscordLinks();
            var playerId = playerDiscords.First(x => x[1].Trim().ToLower() == player.Trim().ToLower())[0];
            
            var user = Context.Guild.Users.First(x => x.Id == ulong.Parse(playerId));
            var embed = AnzacSpiritService.GetPlayerOrdersEmbed(finalOrders.First(x => x.Key == ulong.Parse(playerId)));
            await DiscordService.DirectMessageUserAsync(embed, user);
        }

        [Command("getorder")]
        public async Task GetOrder(string player)
        {
            var finalOrders = AnzacSpiritService.GetWarOrdersSortedByDiscordUser();
            var playerDiscords = AnzacSpiritService.GetPlayerDiscordLinks();
            var playerId = playerDiscords.First(x => x[1].Trim().ToLower() == player.Trim().ToLower())[0];

            var embed = AnzacSpiritService.GetPlayerOrdersEmbed(finalOrders.First(x => x.Key == ulong.Parse(playerId)));
            await ReplyAsync(embed: embed);
        }

        [Command("verifyorders")]
        public async Task VerifyOrders()
        {
            var outputEmbed = new EmbedBuilder
            {
                Title = "Verify Orders",
                Timestamp = DateTime.Now,
                Color = Color.Green
            };
            var playerDiscords = AnzacSpiritService.GetPlayerDiscordLinks();

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

        [Command("error")]
        public async Task ThrowError()
        {
            throw new NotImplementedException();
        }
    }
}
