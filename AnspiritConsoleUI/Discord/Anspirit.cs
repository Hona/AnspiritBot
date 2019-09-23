using AnspiritConsoleUI.Services;
using AnspiritConsoleUI.Services.Google;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Discord
{
    public class Anspirit
    {
        private readonly string _discordToken;
        private DiscordSocketClient _discordClient;
        private AnspiritCommandService _anspiritCommandService;
        private DependencyInjection _dependencyInjection;
        private IServiceProvider _services;
        private LogService _logger;
        // todo: do logging
        public Anspirit(string discordToken)
        {
            _discordToken = discordToken;

            var discordClientConfig = new DiscordSocketConfig
            {
                AlwaysDownloadUsers = false
            };
            _discordClient = new DiscordSocketClient(discordClientConfig);

            var commands = AnspiritCommandService.BuildCommandService();
            _dependencyInjection = new DependencyInjection(commands, _discordClient);
            _services = _dependencyInjection.BuildServiceProvider();
            _anspiritCommandService = new AnspiritCommandService(_discordClient, commands, _services);

            _logger = _services.GetService(typeof(LogService)) as LogService;
        }

        internal async Task<Exception> RunAsync()
        {
            try
            {
                // Register commands
                await _anspiritCommandService.InitializeAsync();

                // Login and start bot
                await _discordClient.LoginAsync(TokenType.Bot, _discordToken, validateToken: true);
                await _discordClient.StartAsync();

                AnspiritSheetsService asdf = new AnspiritSheetsService(_logger);
                // Block the task indefinately
                await Task.Delay(-1);
            }
            catch (Exception e)
            {
                return e;
            }
            return null;
        }
    }
}
