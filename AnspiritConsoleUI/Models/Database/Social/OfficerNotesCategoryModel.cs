using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnspiritConsoleUI.Models.Database.Social
{
    public class OfficerNotesCategoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }
        [Column("categoryName", TypeName = "VARCHAR(32) NOT NULL")]
        public string CategoryName { get; set; }
    }
}
