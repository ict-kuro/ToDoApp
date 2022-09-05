using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Models
{
    [Table("color_table")]
    public partial class ColorTable
    {
        public ColorTable()
        {
            TaskTables = new HashSet<TaskTable>();
        }

        [Key]
        [Column("color_id")]
        [StringLength(12)]
        [Unicode(false)]
        public string ColorId { get; set; } = null!;
        [Column("color_name")]
        [StringLength(10)]
        public string? ColorName { get; set; }

        [InverseProperty("Color")]
        public virtual ICollection<TaskTable> TaskTables { get; set; }
    }
}
