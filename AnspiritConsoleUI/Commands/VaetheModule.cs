using AnspiritConsoleUI.Commands.Preconditions;
using AnspiritConsoleUI.Services;
using Discord;
using Discord.Commands;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands
{
    [RequireVaethePrecondition(Group = "VaetheModuleGroup")]
    [RequireOwner(Group = "VaetheModuleGroup")]
    public class VaetheModule : AnspiritModuleBase
    {
        [Command("pm2")]
        [Summary("What")]
        public async Task PM2Commands([Remainder]string arguments)
        {
            var cmd = "pm2 " + arguments;
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                }
            };

            var lines = string.Empty;

            process.Start();
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                lines += line + Environment.NewLine;
            }
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(lines))
            {
                var outputMessages = DiscordMessageUtilities.GetSendableMessages(lines, codeBlock: true);
                foreach (var message in outputMessages)
                {
                    await ReplyAsync(message);
                }
            }
            else
            {
                await ReplyNewEmbed("No output, presume program ran successfully", Color.Purple);
            }

            process.Dispose();
        }
    }
}
