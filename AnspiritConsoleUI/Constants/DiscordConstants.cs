using System;
using System.IO;

namespace AnspiritConsoleUI.Constants
{
    internal static class DiscordConstants
    {
        internal static readonly string ConfigFolderPath = Path.Combine(Environment.CurrentDirectory, "config");
        internal static readonly string DiscordTokenFilePath = Path.Combine(ConfigFolderPath, "discordToken.txt");
        internal static readonly string GoogleAPIKeyFilePath = Path.Combine(ConfigFolderPath, "googleapikey.txt");
        internal static readonly string AnzacSpiritSheetsID = Path.Combine(ConfigFolderPath, "sheetsID.txt");
        internal static readonly string AnzacSpiritSheetsRange = Path.Combine(ConfigFolderPath, "sheetsRange.txt");
        internal static readonly string AnzacSpiritWarOfficers = Path.Combine(ConfigFolderPath, "authorisedWarOfficers.txt");
        internal static readonly string AnzacSpiritPlayers = Path.Combine(ConfigFolderPath, "players.txt");
        internal static readonly char DiscordCommandPrefix = '!';
        internal const int LogPaddingLength = 10;
    }
}
