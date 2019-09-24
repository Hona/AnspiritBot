using AnspiritConsoleUI.Data;
using AnspiritConsoleUI.Services;
using AnspiritConsoleUI.Services.Database;
using AnspiritConsoleUI.Services.Google;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AnspiritConsoleUI.Discord
{
    public class DependencyInjection
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discordClient;
        public DependencyInjection(CommandService commands = null, DiscordSocketClient discordClient = null)
        {
            _commands = commands ?? new CommandService();
            _discordClient = discordClient ?? new DiscordSocketClient();
        }
        public IServiceProvider BuildServiceProvider()
        {
            var logger = new LogService(_discordClient);
            var dbContext = new AnspiritContext();
            return new ServiceCollection()
                .AddSingleton(_discordClient)
                .AddSingleton(_commands)
                .AddSingleton(logger)
                .AddSingleton<AnspiritSheetsService>()
                .AddSingleton<AnzacSpiritService>()
                .AddSingleton(dbContext)
                .AddSingleton(new AnspiritDatabaseService(logger))
                .BuildServiceProvider();
        }
    }
}
