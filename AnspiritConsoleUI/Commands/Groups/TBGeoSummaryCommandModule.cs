using AnspiritConsoleUI.Commands.Preconditions;
using AnspiritConsoleUI.Constants;
using AnspiritConsoleUI.Services;
using AnspiritConsoleUI.Services.Database;
using AnspiritConsoleUI.Utilities;
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
    [RequireAnspiritDiscordOfficerPrecondition(Group = "TBGeoSummaryDebug")]
    [RequireOwner(Group = "TBGeoSummaryDebug")]
    [Group("tbsummary geo")]
    [Alias("tbs geo")]
    public class TBGeoSummaryCommandModule : AnspiritModuleBase
    {
        public AnspiritDatabaseService DbService { get; set; }
        [Command("add")]
        [Summary("Adds a geo tb note")]
        public async Task AddTBSummaryNote([Remainder] string notes)
        {
            notes = notes
                .Replace("Nth", "**North Zone**")
                .Replace("Mid", "**Middle Zone**")
                .Replace("Sth", "**South Zone**");

            await DbService.AddOfficerNote(SwgohConstants.DarkSideTBCategory, Context.User.Id, notes);
            await ReplyNewEmbed("Added the note successfully", Color.Green);
        }
        [Command("remove")]
        [Alias("delete", "del", "rem")]
        [Summary("Removes a geo tb note")]
        public async Task RemoveTBSummaryNote([Summary("dd/mm/yyyy")] string dateTime, [Remainder] string comment = null)
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

            await DbService.RemoveOfficerNotesAsync(Context.User.Id, SwgohConstants.DarkSideTBCategory, new DateTime(year, month, day), comment);
            await ReplyNewEmbed($"Removed the note successfully", Color.Green);

        }
        [Command("list")]
        [Alias("ls", "get", "")]
        [Summary("Returns a list of the officer notes against a user")]
        public async Task ListTBSummaryNotes(IUser battleCommander = null)
        {
            var outputEmbeds = await AnspiritUtilities.GetOfficerNotesEmbedsAsync(battleCommander == null ? Context.User : battleCommander, DbService, SwgohConstants.DarkSideTBCategory);
            await outputEmbeds.SendAllAsync(Context.Channel);
        }
    }
}
