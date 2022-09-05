using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Models
{
    [Table("task_table")]
    public partial class TaskTable
    {
        [Key]
        [Column("task_id")]
        public int TaskId { get; set; }
        [Column("task_name")]
        [StringLength(40)]
        public string? TaskName { get; set; }
        [Column("start_time", TypeName = "datetime")]
        public DateTime? StartTime { get; set; }
        [Column("finish_time", TypeName = "datetime")]
        public DateTime? FinishTime { get; set; }
        [Column("comment")]
        [StringLength(100)]
        public string? Comment { get; set; }
        [Column("color_id")]
        [StringLength(12)]
        [Unicode(false)]
        public string ColorId { get; set; } = null!;
        [Column("user_id")]
        [StringLength(12)]
        [Unicode(false)]
        public string UserId { get; set; } = null!;
        [Column("created_time", TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }
        [Column("renewed_time", TypeName = "datetime")]
        public DateTime? RenewedTime { get; set; }
        [Column("my_priority")]
        [StringLength(4)]
        public string? MyPriority { get; set; }
        [Column("delete_flag")]
        public bool DeleteFlag { get; set; }
        [Column("complete_flag")]
        public bool CompleteFlag { get; set; }

        [ForeignKey("ColorId")]
        [InverseProperty("TaskTables")]
        public virtual ColorTable Color { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("TaskTables")]
        public virtual UserTable User { get; set; } = null!;
    }
}
