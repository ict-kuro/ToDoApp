using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToDo_App.Models
{
    [Table("Task")]
    public partial class Task
    {
        [Column("userID")]
        [StringLength(255)]
        [Unicode(false)]
        public string UserId { get; set; } = null!;
        [Column("register_date", TypeName = "datetime")]
        public DateTime RegisterDate { get; set; }
        [Column("deadline_date", TypeName = "datetime")]
        public DateTime DeadlineDate { get; set; }
        [Column("start_date", TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column("priority")]
        public int Priority { get; set; }
        [Column("image_path")]
        [StringLength(255)]
        [Unicode(false)]
        public string? ImagePath { get; set; }
        [Column("comment")]
        [StringLength(4000)]
        public string? Comment { get; set; }
        [Column("taskname")]
        [StringLength(255)]
        public string Taskname { get; set; } = null!;
        [Key]
        [Column("taskID")]
        public int TaskId { get; set; }
        [Column("task_status")]
        public bool? TaskStatus { get; set; }
        [Column("groupID")]
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty("Tasks")]
        public virtual TaskGroup Group { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("Tasks")]
        public virtual User User { get; set; } = null!;
    }
}
