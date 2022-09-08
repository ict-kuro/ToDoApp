using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoApp_Master.Models
{
    [Table("User")]
    public partial class User
    {
        [Key]
        [StringLength(255)]
        [Unicode(false)]
        public string UserId { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
    }
}
