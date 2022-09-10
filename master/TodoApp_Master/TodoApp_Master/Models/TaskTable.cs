using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoApp_Master.Models
{
    [Table("TaskTable")]
    public partial class TaskTable
    {
        [Key]
        public int TaskId { get; set; }
        [StringLength(50)]
        public string TaskName { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DeadLine { get; set; }
        [StringLength(200)]
        public string? Comment { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string UserId { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RenewedTime { get; set; }
        public int? Priority { get; set; }
        public bool TaskStatus { get; set; }
        public bool DeleteFlag { get; set; }
        public int? GroupId { get; set; }
        public int? ColorId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("TaskTables")]
        public virtual User User { get; set; } = null!;
    }
}
