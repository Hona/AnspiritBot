using AnspiritConsoleUI.Commands.Preconditions;
using AnspiritConsoleUI.Constants;
using AnspiritConsoleUI.Services;
using AnspiritConsoleUI.Services.Database;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands
{
    [RequireAnspiritDiscordOfficerPrecondition(Group = "DiscordOfficerDebug")]
    [RequireOwner(Group = "DiscordOfficerDebug")]
    public class AnzacDiscordOfficerModule : AnspiritModuleBase
    {
        public AnspiritDatabaseService DbService { get; set; }
        [Command("officernotes add")]
        [Summary("Adds an officers notes categories")]
        public async Task AddOfficerNote(IUser discordUser, string categoryName, [Remainder] string notes)
        {
            await DbService.AddOfficerNote(categoryName, discordUser.Id, notes);
            await ReplyNewEmbed($"Added the note successfully", Color.Green);
        }
        [Command("officernotes remove")]
        [Alias("officernotes delete", "officernotes del", "officernotes rem")]
        [Summary("Removes an officer note")]
        public async Task RemoveOfficerNote(IUser discordUser, string categoryName, [Summary("dd/mm/yyyy")] string dateTime)
        {
            // TODO: This could be parsed simply by DateTime.Parse
            var datetimeParts = dateTime.Split('/');
            var dayString = datetimeParts[0];
            var monthString = datetimeParts[1];
            var yearString = datetimeParts[2];

            if (!int.TryParse(dayString, out var day))
            {
                throw new Exception("Could not parse the day from the datetime string, are you using dd/mm/yyyy format?");
            }
            if (!int.TryParse(monthString, out var month))
            {
                throw new Exception("Could not parse the month from the datetime string, are you using dd/mm/yyyy format?");
            }
            if (!int.TryParse(yearString, out var year))
            {
                throw new Exception("Could not parse the year from the datetime string, are you using dd/mm/yyyy format?");
            }

            await DbService.RemoveOfficerNotesAsync(discordUser.Id, categoryName, new DateTime(year, month, day));
            await ReplyNewEmbed($"Removed the note successfully", Color.Green);
        }
        [Command("officernotes list")]
        [Alias("officernotes ls", "officernotes get")]
        [Summary("Returns a list of the officer notes against a user")]
        public async Task ListOfficerNotes(IUser user)
        {
            var notes = DbService.GetOfficerNotes(user);

            var membersRole = (user as SocketGuildUser).Roles.FirstOrDefault(x => x.Name.ToLower().Contains("member") && x.Id != DiscordConstants.CoalitionMembersRoleID);

            var embedColor = membersRole == null ? Color.Green : membersRole.Color;

            var stringBuilder = new StringBuilder();

            foreach (var note in notes)
            {
                var formattedDate = note.DateTimeEntry.ToString("dd/MM/yyyy");
                string categoryName;
                try
                {
                    categoryName = await DbService.GetOfficerNotesCategoryNameFromIDAsync(note.CategoryID);
                }
                catch
                {
                    categoryName = "<Not Found>";
                }

                stringBuilder.Append($"{formattedDate} | {categoryName} | {note.Comments}");
                stringBuilder.Append(Environment.NewLine);
            }
            
            var embedDescriptions = DiscordMessageUtilities.GetSendableMessages(stringBuilder.ToString());

            for (int i = 0; i < embedDescriptions.Length; i++)
            {
                var embedBuilder = new EmbedBuilder
                {
                    Title = i == 0 ? $"Officer Notes for {user.Username}" : null,
                    Timestamp = DateTime.Now,
                    Color = embedColor,
                    Description = embedDescriptions[i]
                };

                await ReplyAsync(embed: embedBuilder.Build());
            }
        }
    }
}
