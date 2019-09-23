using AnspiritConsoleUI.Constants;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Services
{
    public class LogService
    {
        public LogService(DiscordSocketClient discordClient)
        {
            discordClient.Log += LogAsync;
        }
        internal async Task LogAsync(LogMessage logMessage)
        {
            await Task.Run(() => 
            {
                if (logMessage.Message == null)
                    logMessage = new LogMessage(logMessage.Severity, logMessage.Source, "", logMessage.Exception);
                if (logMessage.Source == null)
                    logMessage = new LogMessage(logMessage.Severity, "", logMessage.Message, logMessage.Exception);

                switch (logMessage.Severity)
                {
                    case LogSeverity.Critical:
                    case LogSeverity.Error:
                        Console.ForegroundColor = ColorConstants.ErrorLogColor;
                        break;
                    case LogSeverity.Warning:
                        Console.ForegroundColor = ColorConstants.WarningLogColor;
                        break;
                    case LogSeverity.Info:
                    case LogSeverity.Verbose:
                    case LogSeverity.Debug:
                        Console.ForegroundColor = ColorConstants.InfoLogColor;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Console.WriteLine(
                    $"{logMessage.Severity.ToString().PadRight(DiscordConstants.LogPaddingLength)}    {logMessage.Source.PadRight(DiscordConstants.LogPaddingLength)}    {logMessage.Message.PadRight(DiscordConstants.LogPaddingLength)}    {logMessage.Exception}");
                Console.ForegroundColor = ColorConstants.InfoLogColor;
            });
        }
        internal async Task LogInfoAsync(string source, string message) => await LogAsync(new LogMessage(LogSeverity.Info, source,
        message));
        internal async Task LogError(string source, string message) => await LogAsync(new LogMessage(LogSeverity.Error, source,
        message));
        internal async Task LogWarning(string source, string message) => await LogAsync(new LogMessage(LogSeverity.Warning,
            source, message));
    }
}
