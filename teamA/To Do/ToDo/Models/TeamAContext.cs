using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ToDo.Models
{
    public partial class TeamAContext : DbContext
    {
        public TeamAContext()
        {
        }

        public TeamAContext(DbContextOptions<TeamAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ColorTable> ColorTables { get; set; } = null!;
        public virtual DbSet<TaskTable> TaskTables { get; set; } = null!;
        public virtual DbSet<UserTable> UserTables { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=192.168.28.62;initial catalog=TeamA;User ID=sa;Password=IctxIct0;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ColorTable>(entity =>
            {
                entity.HasKey(e => e.ColorId)
                    .HasName("PK__color_ta__1143CECBFD6AC86F");
            });

            modelBuilder.Entity<TaskTable>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("PK__task_tab__0492148D02D86E03");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.TaskTables)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tasks_FK2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaskTables)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tasks_FK1");
            });

            modelBuilder.Entity<UserTable>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__user_tab__B9BE370F4CDBDE8F");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
