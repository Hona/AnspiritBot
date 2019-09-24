using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnspiritConsoleUI.Models.Database
{
    public class IngamePlayerDiscordLinkModel
    {
        [Key]
        [Column("ingameName", TypeName = "varchar(32)")]
        public string InGameName { get; set; }
        
        [Column("discordID", TypeName = "bigint(20) unsigned")]
        public ulong DiscordId { get; set; }
    }
}
