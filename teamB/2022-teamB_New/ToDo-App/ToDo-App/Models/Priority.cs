using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToDo_App.Models
{
    [Table("Priority")]
    public partial class Priority
    {
        [Key]
        [Column("priorityID")]
        public int PriorityId { get; set; }
        [Column("priorityname")]
        [StringLength(255)]
        public string? Priorityname { get; set; }
    }
}
