using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Models
{
    [Table("user_table")]
    public partial class UserTable
    {
        public UserTable()
        {
            TaskTables = new HashSet<TaskTable>();
        }

        [Key]
        [Column("user_id")]
        [StringLength(12)]
        [Unicode(false)]
        public string UserId { get; set; } = null!;
        [Column("password")]
        [StringLength(64)]
        [Unicode(false)]
        public string Password { get; set; } = null!;

        [InverseProperty("User")]
        public virtual ICollection<TaskTable> TaskTables { get; set; }
    }
}
