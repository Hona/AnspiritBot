using AnspiritConsoleUI.Services;
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
        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(_discordClient)
            .AddSingleton(_commands)
            .AddSingleton<LogService>()
            .AddSingleton<AnspiritSheetsService>()
            .AddSingleton<AnzacSpiritService>()
            .BuildServiceProvider();
    }
}
