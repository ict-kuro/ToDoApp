using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ToDo_App.Models
{
    public partial class TeamBContext : DbContext
    {
        public TeamBContext()
        {
        }

        public TeamBContext(DbContextOptions<TeamBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PriorityTable> PriorityTables { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<TaskGroup> TaskGroups { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=10.0.0.12,50500;initial catalog=TeamB;User ID=TeamB;Password=internTeamB;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PriorityTable>(entity =>
            {
                entity.HasKey(e => e.PriorityId)
                    .HasName("PK__Priority__58E3F07AEB2B89E4");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.TaskStatus).HasDefaultValueSql("('FALSE')");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Task__groupID__4CA06362");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Task__userID__4BAC3F29");
            });

            modelBuilder.Entity<TaskGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK__TaskGrou__88C102ADC1AB1F33");

                entity.Property(e => e.GroupId).ValueGeneratedNever();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.DeleteDate).HasDefaultValueSql("((180))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
