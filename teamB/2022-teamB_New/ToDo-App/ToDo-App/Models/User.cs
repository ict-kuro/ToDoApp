using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToDo_App.Models
{
    public partial class User
    {
        public User()
        {
            Tasks = new HashSet<Task>();
        }

        [Key]
        [Column("userID")]
        [StringLength(255)]
        [Unicode(false)]
        public string UserId { get; set; } = null!;
        [Column("password")]
        [StringLength(255)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        [Column("last_log_date", TypeName = "date")]
        public DateTime LastLogDate { get; set; }
        [Column("delete_date")]
        public int? DeleteDate { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
