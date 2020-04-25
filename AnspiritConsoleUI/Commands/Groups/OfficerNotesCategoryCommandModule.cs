using System;
using System.Linq;
using System.Threading.Tasks;
using AnspiritConsoleUI.Commands.Preconditions;
using AnspiritConsoleUI.Constants;
using AnspiritConsoleUI.Services;
using Discord;
using Discord.Commands;

namespace AnspiritConsoleUI.Commands.Groups
{
    [RequireAnspiritAdminPrecondition]
    [Group("officernotescategory")]
    [Alias("onc")]
    public class OfficerNotesCategoryCommandModule : AnspiritModuleBase
    {
        public AnspiritDatabaseService DbService { get; set; }

        [Command("add")]
        [Summary("Adds a cateogry the the officers notes categories")]
        public async Task AddOfficerCategory(string categoryName)
        {
            await DbService.AddOfficerNotesCategory(categoryName);
            await ReplyNewEmbed($"Added {categoryName} to the officers notes categories", Color.Green);
        }
        [Command("remove")]
        [Alias("delete", "del", "rem")]
        [Summary("Removes a category from the officer notes categories")]
        public async Task RemoveOfficerCategory(string categoryName)
        {
            if (categoryName == SwgohConstants.DarkSideTBCategory)
            {
                await ReplyNewEmbed($"Cannot remove a system category", Color.Orange);
                return;
            }
            var categoryID = await DbService.GetOfficerNotesCategoryIDFromNameAsync(categoryName);
            await DbService.RemoveOfficerNotesCategoriesAsync(categoryID);
            await ReplyNewEmbed($"Removed {categoryName} from the officer notes categories.", Color.Green);
        }
        [Command("list")]
        [Alias("ls", "get")]
        [Summary("Returns a list of the current officer notes categories")]
        public async Task ListOfficerNotesCategories()
        {
            var categories = DbService.GetOfficerNotesCategories();
            var embedBuilder = new EmbedBuilder
            {
                Title = "Officer Notes Categories",
                Timestamp = DateTime.Now,
                Color = Color.Purple,
            };

            embedBuilder.Description = string.Join(Environment.NewLine, categories
                .Where(x => x.CategoryName != SwgohConstants.DarkSideTBCategory)
                .Select(x => x.CategoryName));

            await ReplyAsync(embed: embedBuilder.Build());
        }
    }
}
