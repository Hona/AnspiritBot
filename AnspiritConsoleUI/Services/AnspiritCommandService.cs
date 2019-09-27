using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Services
{
    public class AnspiritCommandService
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly CommandService _commands;
        private readonly IServiceProvider _serviceProvider;
        private readonly LogService _logger;

        public AnspiritCommandService(DiscordSocketClient discordClient, CommandService commands, IServiceProvider serviceProvider)
        {
            _discordClient = discordClient;
            _serviceProvider = serviceProvider;
            _commands = commands;
            _logger = (LogService)_serviceProvider.GetService(typeof(LogService));
        }
        internal async Task InitializeAsync()
        {
            // Main handler for command input
            _discordClient.MessageReceived += HandleCommandAsync;
            await _logger.LogInfoAsync("CommandService", "Registered MessageReceived event");

            // Post execution handler
            _commands.CommandExecuted += OnCommandExecutedAsync;
            await _logger.LogInfoAsync("CommandService", "Registered CommandExecuted event");

            // Install discord commands
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _serviceProvider);
            await _logger.LogInfoAsync("CommandService", $"Added {_commands.Modules.Count()} modules using reflection, with a total of {_commands.Commands.Count()} commands");
        }

        internal async Task HandleCommandAsync(SocketMessage inputMessage)
        {
            // Don't process the command if it was a system message
            if (inputMessage is SocketUserMessage message)
            {
                // Create a number to track where the prefix ends and the command begins
                int argPos = 0;

                // Determine if the message is a command based on the prefix and make sure no bots trigger commands
                if (!(message.HasCharPrefix('!', ref argPos) ||
                    message.HasMentionPrefix(_discordClient.CurrentUser, ref argPos)) ||
                    message.Author.IsBot)
                    return;

                // Create a WebSocket-based command context based on the message
                var context = new SocketCommandContext(_discordClient, message);

                // Low volume of commands, so it is safe to log it without getting flooded
                await _logger.LogInfoAsync("HandleCommand", $"User: {context.User.Username + "#" + context.User.Discriminator} ran: {Environment.NewLine}{context.Message.Content.Substring(argPos)}");

                // Execute the command with the command context we just
                // created, along with the service provider for precondition checks.
                await _commands.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: _serviceProvider);
            }
        }
        internal async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!string.IsNullOrEmpty(result?.ErrorReason))
            {
                var embedBuilder = new EmbedBuilder
                {
                    Timestamp = DateTime.Now,
                    Color = Color.Red
                };
                await context.Channel.SendMessageAsync(embed: embedBuilder.WithDescription(result.ErrorReason).Build());
                var commandName = command.IsSpecified ? command.Value.Name : "An unknown command";
                
                await _logger.LogAsync(new LogMessage(LogSeverity.Error,
                    "CommandService",
                    $"{commandName} threw an error at {DateTime.UtcNow}: {Environment.NewLine}{result.ErrorReason}"));
            }
        }
        internal static CommandService BuildCommandService()
        {
            // RunMode.Async makes commands run async, so that a long running command doesn't block the thread.
            var commandServiceConfig = new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                IgnoreExtraArgs = true,
                LogLevel = LogSeverity.Info
            };

            return new CommandService(commandServiceConfig);
        }
    }
}
