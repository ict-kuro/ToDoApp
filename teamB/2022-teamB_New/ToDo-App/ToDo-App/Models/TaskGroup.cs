using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToDo_App.Models
{
    [Table("TaskGroup")]
    public partial class TaskGroup
    {
        public TaskGroup()
        {
            Tasks = new HashSet<Task>();
        }

        [Key]
        [Column("groupID")]
        public int GroupId { get; set; }
        [Column("groupname")]
        [StringLength(255)]
        public string? Groupname { get; set; }

        [InverseProperty("Group")]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
