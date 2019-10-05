using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnspiritConsoleUI.Models.Database.Social
{
    public class OfficerNotesModel
    {
        [Column("categoryID", TypeName = "INT NOT NULL")]
        public int CategoryID { get; set; }
        [Column("discordID", TypeName = "bigint(20) unsigned")]
        public ulong DiscordId { get; set; }
        [Column("datetimeEntry", TypeName = "DATETIME NOT NULL")]
        public DateTime DateTimeEntry { get; set; }
        [Column("comments", TypeName = "VARCHAR(1024) NOT NULL")]
        public string Comments { get; set; }
    }
}
