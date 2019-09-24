using AnspiritConsoleUI.Data;
using AnspiritConsoleUI.Models.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Services.Database
{
    public class AnspiritDatabaseService
    {
        private readonly LogService _logger;
        public AnspiritDatabaseService(LogService logger)
        {
            _logger = logger;
        }
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
        public async Task AddIngamePlayerDiscordLinkAsync(ulong discordId, string inGameName)
        {
            var db = new AnspiritContext();


            await _logger.LogInfoAsync("AnspiritDB", $"Adding {inGameName} ({discordId}) to the in game discord link table");
            var existingUser = db.IngamePlayerDiscordLinks.FirstOrDefault(x => x.DiscordId == discordId);
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
    }
}
