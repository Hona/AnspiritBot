using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnspiritConsoleUI.Data;
using AnspiritConsoleUI.Models.Database;
using AnspiritConsoleUI.Models.Database.Social;

namespace AnspiritConsoleUI.Services
{
    public class AnspiritDatabaseService
    {
        private readonly LogService _logger;
        public AnspiritDatabaseService(LogService logger)
        {
            _logger = logger;
        }
        #region AnspiritAdmins
        public async Task AddAnspiritAdminAsync(ulong discordId)
        {
            var db = new AnspiritContext();

            await _logger.LogInfoAsync("AnspiritDB", $"Adding {discordId} to the admin table");
            db.AnspiritAdmins.Add(new AnspiritAdminModel
            {
                DiscordId = discordId
            });
            await db.SaveChangesAsync();

        }
        public IEnumerable<AnspiritAdminModel> GetAnspiritAdmins()
        {
            var db = new AnspiritContext();
            return db.AnspiritAdmins;
        }
        public async Task RemoveAnspiritAdminAsync(ulong discordId)
        {
            var db = new AnspiritContext();

            var admin = db.AnspiritAdmins.Find(discordId);

            if (admin != null)
            {
                await _logger.LogInfoAsync("AnspiritDB", $"Removing {discordId} from the admin table");
                db.AnspiritAdmins.Remove(admin);
                await db.SaveChangesAsync();
            }

        }
        #endregion
        #region WarOfficers
        public async Task AddWarOfficerAsync(ulong discordId)
        {
            var db = new AnspiritContext();

            await _logger.LogInfoAsync("AnspiritDB", $"Adding {discordId} to the war officers table");
            db.WarOfficers.Add(new WarOfficersModel
            {
                DiscordId = discordId
            });
            await db.SaveChangesAsync();

        }
        public IEnumerable<WarOfficersModel> GetWarOfficers()
        {
            var db = new AnspiritContext();
            return db.WarOfficers;
        }
        public async Task RemoveWarOfficerAsync(ulong discordId)
        {
            var db = new AnspiritContext();

            var admin = db.WarOfficers.Find(discordId);

            if (admin != null)
            {
                await _logger.LogInfoAsync("AnspiritDB", $"Removing {discordId} from the war officers table");
                db.WarOfficers.Remove(admin);
                await db.SaveChangesAsync();
            }

        }
        #endregion
        #region IngamePlayerDiscordLinks
        public async Task AddIngamePlayerDiscordLinkAsync(ulong discordId, string inGameName)
        {
            var db = new AnspiritContext();

            await _logger.LogInfoAsync("AnspiritDB", $"Adding {inGameName} ({discordId}) to the in game discord link table");
            var existingUser = db.IngamePlayerDiscordLinks.FirstOrDefault(x => x.InGameName == inGameName);
            if (existingUser == null)
            {
                db.IngamePlayerDiscordLinks.Add(new IngamePlayerDiscordLinkModel
                {
                    DiscordId = discordId,
                    InGameName = inGameName
                });
            }
            else
            {
                await RemoveIngamePlayerDiscordLinkAsync(inGameName);
                await db.SaveChangesAsync();
                await AddIngamePlayerDiscordLinkAsync(discordId, inGameName);
            }

            await db.SaveChangesAsync();

        }
        public IEnumerable<IngamePlayerDiscordLinkModel> GetInGamePlayerDiscordLinks()
        {
            var db = new AnspiritContext();
            return db.IngamePlayerDiscordLinks;
        }
        public async Task RemoveIngamePlayerDiscordLinkAsync(string playerName)
        {
            var db = new AnspiritContext();

            var player = db.IngamePlayerDiscordLinks.FirstOrDefault(x => x.InGameName.ToLower() == playerName.ToLower());

            if (player != null)
            {
                await _logger.LogInfoAsync("AnspiritDB", $"Removing {playerName} ({player.DiscordId}) from the in game discord link table");
                db.IngamePlayerDiscordLinks.Remove(player);
                await db.SaveChangesAsync();
            }
        }
        #endregion
        #region OfficerNotesCategories
        public async Task AddOfficerNotesCategory(string categoryName)
        {
            if (categoryName.Length > 32)
            {
                throw new ArgumentOutOfRangeException(nameof(categoryName), "Category name must be 32 or less characters.");
            }

            if (categoryName.Contains(' '))
            {
                throw new ArgumentOutOfRangeException(nameof(categoryName), "Category name cannot contain spaces.");

            }

            var db = new AnspiritContext();

            await _logger.LogInfoAsync("AnspiritDB", $"Adding {categoryName} to the officer notes category table");
            db.OfficerNotesCategories.Add(new OfficerNotesCategoryModel
            {
                CategoryName = categoryName
            });
            await db.SaveChangesAsync();
        }
        public IEnumerable<OfficerNotesCategoryModel> GetOfficerNotesCategories()
        {
            var db = new AnspiritContext();
            return db.OfficerNotesCategories;
        }
        public async Task RemoveOfficerNotesCategoriesAsync(int id)
        {
            var db = new AnspiritContext();

            var category = db.OfficerNotesCategories.Find(id);

            if (category != null)
            {
                await _logger.LogInfoAsync("AnspiritDB", $"Removing {category.CategoryName} from the officer notes categories table");
                db.OfficerNotesCategories.Remove(category);
                await db.SaveChangesAsync();
            }

        }
        public async Task<int> GetOfficerNotesCategoryIDFromNameAsync(string categoryName)
        {
            var db = new AnspiritContext();
            var categories = GetOfficerNotesCategories();
            var category = categories.FirstOrDefault(x => categoryName.ToLower() == x.CategoryName.ToLower());

            if (category != null)
            {
                return category.CategoryID;
            }

            throw new ArgumentOutOfRangeException(nameof(categoryName), $"No category found with name: '{categoryName}'");
        }
        public async Task<string> GetOfficerNotesCategoryNameFromIDAsync(int categoryId)
        {
            var db = new AnspiritContext();
            var categories = GetOfficerNotesCategories();
            var category = categories.FirstOrDefault(x => categoryId == x.CategoryID);

            if (category != null)
            {
                return category.CategoryName;
            }

            throw new ArgumentOutOfRangeException(nameof(categoryId), $"No category found with id: '{categoryId}'");
        }
        #endregion
        #region OfficerNotes
        public async Task AddOfficerNote(string categoryName, ulong discordId, string comment)
        {
            if (comment.Length > 1024)
            {
                throw new ArgumentOutOfRangeException(nameof(comment), "Comment must be 1024 or less characters.");
            }

            var categoryId = await GetOfficerNotesCategoryIDFromNameAsync(categoryName);
            var db = new AnspiritContext();

            db.OfficerNotes.Add(new OfficerNotesModel
            {
                CategoryID = categoryId,
                Comments = comment,
                DateTimeEntry = DateTime.Now,
                DiscordId = discordId
            });
            await db.SaveChangesAsync();
        }
        public IEnumerable<OfficerNotesModel> GetOfficerNotes(ulong? discordID = null)
        {
            var db = new AnspiritContext();

            return discordID.HasValue ? db.OfficerNotes.Where(x => x.DiscordId == discordID.Value) : db.OfficerNotes;
        }
        public async Task RemoveOfficerNotesAsync(ulong discordId, string categoryName, DateTime dateTime, string content = null)
        {
            var db = new AnspiritContext();

            var categoryId = await GetOfficerNotesCategoryIDFromNameAsync(categoryName);
            var notes = db.OfficerNotes.Where(x => x.DiscordId == discordId && x.CategoryID == categoryId && 
                x.DateTimeEntry.Year == dateTime.Year && x.DateTimeEntry.Month == dateTime.Month && x.DateTimeEntry.Day == dateTime.Day);

            if (content != null)
            {
                notes = notes.Where(x => x.Comments == content);
            }

            if (notes.Count() == 1)
            {
                db.OfficerNotes.Remove(notes.First());
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Too many officer note entries found for those parameters, contact Hona to delete.");
            }
        }

        #endregion
    }
}
