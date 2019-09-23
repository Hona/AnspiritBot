using AnspiritConsoleUI.Discord;
using System;
using System.IO;
using AnspiritConsoleUI.Constants;

namespace AnspiritConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check for invalid argument length
            if (args.Length > 1)
            {
                Console.WriteLine("Too many arguments parsed in.");
                return;
            }

            string discordToken;
            // Only argument is the discord token
            if (args.Length == 1)
            {
                // Check if the argument is a file path
                if (File.Exists(args[0]))
                {
                    discordToken = File.ReadAllText(args[0]);
                }
                else
                {
                    // Otherwise assume argument is a token
                    discordToken = args[0];
                }
            }
            else
            {
                // No discord token passed in the arguments, so load it from the default location
                if (File.Exists(DiscordConstants.DiscordTokenFilePath))
                {
                    discordToken = File.ReadAllText(DiscordConstants.DiscordTokenFilePath);
                }
                else
                {
                    Console.WriteLine($"No discord token file exists, expected it at: '{DiscordConstants.DiscordTokenFilePath}'");
                    return;
                }
               
            }

            var bot = new Anspirit(discordToken);
            var closingException = bot.RunAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            if (closingException == null)
            {
                Console.WriteLine("Application quitting safely.");
            }
            else
            {
                Console.WriteLine("Application quitting unsafely, exception information:");
                Console.WriteLine(closingException.ToString());
            }
        }
    }
}
