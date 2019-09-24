using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnspiritConsoleUI.Models.Database
{
    public class AnspiritAdminModel
    {
        [Key]
        [Column("discordID", TypeName = "bigint(20) unsigned")]
        public ulong DiscordId { get; set; }
    }
}
