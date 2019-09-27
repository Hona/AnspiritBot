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
        public static Embed GetModuleHelpEmbed(ModuleInfo module, ICommandContext context)
        {
            var title = $"Help: **({module.Name})**";
            var text = module.Commands.Where(x => x.CheckPreconditionsAsync(context).GetAwaiter().GetResult().IsSuccess).Aggregate("",
                (current, command) =>
                    current +
                    $"**__{'!' + command.Name}__**{Environment.NewLine}**{command.Summary}**. Parameters: {command.Parameters.Aggregate("", (currentString, nextParameter) => currentString + $"{nextParameter.Name} {GetSummaryString(nextParameter.Summary)}, ").TrimEnd(' ', ',')}{Environment.NewLine}");
            return new EmbedBuilder().WithTitle(title).WithDescription(text).Build();
        }

        private static string GetSummaryString(string summary) => string.IsNullOrEmpty(summary) ? "" : $"({summary})";
    }
}
