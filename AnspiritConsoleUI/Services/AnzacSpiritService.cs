using AnspiritConsoleUI.Constants;
using AnspiritConsoleUI.Models;
using AnspiritConsoleUI.Services.Database;
using AnspiritConsoleUI.Services.Google;
using Discord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AnspiritConsoleUI.Services
{
    public class AnzacSpiritService
    {
        private AnspiritSheetsService _anspiritSheetsService;
        private AnspiritDatabaseService _dbService;
        public AnzacSpiritService(AnspiritSheetsService anspiritSheetsService, AnspiritDatabaseService dbService)
        {
            _anspiritSheetsService = anspiritSheetsService;
            _dbService = dbService;
        }
        public WarZones GetWarZones()
        {
            var data = _anspiritSheetsService.GetWarPlacementValues();

            var warZones = new WarZones();

            for (int column = 0; column < data.Count; column += 2)
            {
                for (int row = 1; row < data[column].Count; row++)
                {
                    warZones[column].Add(new Deployment
                    {
                        Player = (string)data[column][row],
                        Team = (string)data[column + 1][row]
                    });
                }
            }

            return warZones;
        }
        public List<Tuple<string, Deployment>> GetWarOrdersSortedByZone(WarZones orders)
        {
            PropertyInfo[] properties = typeof(WarZones).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var allOrders = new List<Tuple<string, Deployment>>();
            foreach (PropertyInfo p in properties)
            {
                // Only work with strings
                if (p.PropertyType == typeof(List<Deployment>) && p.CanRead && p.CanWrite)
                {
                    var zone = (List<Deployment>)p.GetValue(orders);
                    var data = zone.Select(x => new Tuple<string, Deployment>(p.Name, x));
                    allOrders.AddRange(data);
                }

            }

            return allOrders;
        }
        public void PrintDistictPlayers(List<Tuple<string, Deployment>> orders)
        {
            var allPlayers = orders.Select(x => x.Item2.Player).Distinct().ToList();
            Console.WriteLine("Players: " + allPlayers.Count);
            Console.WriteLine(string.Join(Environment.NewLine, allPlayers));
        }
        public Dictionary<ulong, List<Tuple<string, Deployment>>> GetWarOrdersSortedByDiscordUser()
        {
            var orders = GetWarOrdersSortedByZone(GetWarZones());

            var allPlayers = orders.Select(x => x.Item2.Player).Distinct().ToList();

            var output = new Dictionary<ulong, List<Tuple<string, Deployment>>>();

            var playerDiscords = _dbService.GetInGamePlayerDiscordLinks();
            foreach (var player in allPlayers)
            {
                var deploymentOrders = orders.Where(x => x.Item2.Player == player).ToList();
                var discordId = playerDiscords.First(x => x.InGameName.ToLower() == player.ToLower()).DiscordId;
                if (output.ContainsKey(discordId))
                {
                    // Already added, so its an alt account
                    deploymentOrders = deploymentOrders.Select(x => new Tuple<string, Deployment>(x.Item1, new Deployment { Player = x.Item2.Player, Team = x.Item2.Team + $" ({player})" })).ToList();
                    output[discordId].AddRange(deploymentOrders);
                }
                else
                {
                    output.Add(discordId, deploymentOrders);

                }
            }
            return output;
        }
        public Embed GetPlayerOrdersEmbed(KeyValuePair<ulong, List<Tuple<string, Deployment>>> playerOrder)
        {
            var embedBuilder = new EmbedBuilder()
            {
                Color = Color.Purple,
                Title = playerOrder.Value[0].Item2.Player,
                Timestamp = DateTime.Now
            };

            var zones = playerOrder.Value.Select(x => x.Item1).Distinct();
            foreach (var zone in zones)
            {
                embedBuilder.AddField(zone, string.Join(", ", playerOrder.Value.Where(x => x.Item1 == zone).Select(x => x.Item2.Team)));
            }
            return embedBuilder.Build();
        }
    }
}
