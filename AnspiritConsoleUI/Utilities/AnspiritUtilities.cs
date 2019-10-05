using AnspiritConsoleUI.Constants;
using AnspiritConsoleUI.Services;
using AnspiritConsoleUI.Services.Database;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Utilities
{
    public static class AnspiritUtilities
    {
        public static SocketRole GetUserGuildRole(IUser user)
        {
            if (user is SocketGuildUser guildUser)
            {
                return guildUser.Roles.FirstOrDefault(x => x.IsMemberRole() && x.NotDefaultCoalitionRole());
            }
            else
            {
                throw new Exception("The user is not part of a guild - are you running this inside the server?");
            }
        }
        public static async Task<IEnumerable<Embed>> GetOfficerNotesEmbedsAsync(IUser user, AnspiritDatabaseService dbService, string category = null)
        {
            var notes = dbService.GetOfficerNotes(user.Id);
            if (category != null)
            {
                var categoryID = await dbService.GetOfficerNotesCategoryIDFromNameAsync(category);
                notes = notes.Where(x => x.CategoryID == categoryID);
            }
            var membersRole = GetUserGuildRole(user);
            var embedColor = membersRole == null ? Color.Green : membersRole.Color;

            var stringBuilder = new StringBuilder();

            foreach (var note in notes)
            {
                var formattedDate = note.DateTimeEntry.ToString("dd/MM/yyyy");
                string categoryName;
                try
                {
                    categoryName = await dbService.GetOfficerNotesCategoryNameFromIDAsync(note.CategoryID);
                }
                catch
                {
                    categoryName = "<Not Found>";
                }

                stringBuilder.Append($"**{categoryName} | {formattedDate}**{Environment.NewLine}{note.Comments}");
                stringBuilder.Append(Environment.NewLine);
                if (note.Comments.Length > 56 || note.Comments.Contains(Environment.NewLine))
                {
                    stringBuilder.Append(Environment.NewLine);
                }
            }

            var embedDescriptions = DiscordMessageUtilities.GetSendableMessages(stringBuilder.ToString());
            var output = new List<Embed>();
            for (int i = 0; i < embedDescriptions.Length; i++)
            {
                var embedBuilder = new EmbedBuilder
                {
                    Title = i == 0 ? $"Officer Notes for {user.Username}" : null,
                    Timestamp = DateTime.Now,
                    Color = embedColor,
                    Description = embedDescriptions[i]
                };

                output.Add(embedBuilder.Build());
            }
            return output;
        }
        private static bool IsMemberRole(this SocketRole role) => role.Name.ToLower().Contains("member");
        private static bool NotDefaultCoalitionRole(this SocketRole role) => role.Id != DiscordConstants.CoalitionMembersRoleID;
        public static async Task SendAllAsync(this IEnumerable<Embed> embeds, IMessageChannel channel)
        {
            foreach (var embed in embeds)
            {
                await channel.SendMessageAsync(embed: embed);
            }
        }
    }
}
