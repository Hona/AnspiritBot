using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnspiritConsoleUI.Services
{
    public static class HelpCommandService
    {
        public static Embed GetModuleHelpEmbed(ModuleInfo module, ICommandContext context, IServiceProvider services)
        {
            var title = $"Help: **({module.Name})**" ;
            var validForCurrentUserCommands = module.Commands.Where(x => x.CheckPreconditionsAsync(context, services).GetAwaiter().GetResult().IsSuccess);

            var embedBuilder = new EmbedBuilder().WithTitle(title);
            foreach(var command in validForCurrentUserCommands)
            {
                embedBuilder.AddField($"**{'!' + command.Name}** " + GetParametersString(command).TrimEnd(' ', ','), $"{(command.Summary == string.Empty ? "No description" : command.Summary)}. " );
            }

            //var desc = modules.Aggregate("",
            //    (current, command) =>
            //        current +
            //        $"**{'!' + command.Name}**{Environment.NewLine}" +
            //        $"{command.Summary}. " + GetParametersString(command)).TrimEnd(' ', ',') + Environment.NewLine;
            return embedBuilder.Build();
        }

        private static string GetSummaryString(string summary) => string.IsNullOrEmpty(summary) ? "" : $"({summary})";
        private static string GetParametersString(CommandInfo command)
        {
            var output = $"{command.Parameters.Aggregate("", (currentString, nextParameter) => currentString + $"[{nextParameter.Name}{(GetSummaryString(nextParameter.Summary) == string.Empty ? "" : GetSummaryString(nextParameter.Summary))}] ")}";
            return output.Trim() == "Parameters:" ? "" : output;
        }
    }
}