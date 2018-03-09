using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebAppCore.Models
{
    public partial class Btu_DatabaseContext : DbContext
    {
        public virtual DbSet<Batch> Batch { get; set; }
        public virtual DbSet<BatchTest> BatchTest { get; set; }
        public virtual DbSet<Ecu> Ecu { get; set; }
        public virtual DbSet<Procedure> Procedure { get; set; }
        public virtual DbSet<Requirement> Requirement { get; set; }
        public virtual DbSet<Simulator> Simulator { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<TestProc> TestProc { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=(localdb)\ProjectsV13;Database=Btu Database;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batch>(entity =>
            {
                entity.HasKey(e => new { e.BatchId, e.BatchVersion });

                entity.Property(e => e.AuthorUserId).HasColumnName("Author_UserId");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateRun).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.TesterUserId).HasColumnName("Tester_UserId");

                entity.HasOne(d => d.AuthorUser)
                    .WithMany(p => p.BatchAuthorUser)
                    .HasForeignKey(d => d.AuthorUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Batch__Author_Us__32E0915F");

                entity.HasOne(d => d.Sim)
                    .WithMany(p => p.Batch)
                    .HasForeignKey(d => d.SimId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Batch__SimId__34C8D9D1");

                entity.HasOne(d => d.TesterUser)
                    .WithMany(p => p.BatchTesterUser)
                    .HasForeignKey(d => d.TesterUserId)
                    .HasConstraintName("FK__Batch__Tester_Us__33D4B598");
            });

            modelBuilder.Entity<BatchTest>(entity =>
            {
                entity.HasKey(e => new { e.BatchId, e.BatchVersion, e.TestId, e.TestVersion });

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.BatchTest)
                    .HasForeignKey(d => new { d.BatchId, d.BatchVersion })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BatchTest__35BCFE0A");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.BatchTest)
                    .HasForeignKey(d => new { d.TestId, d.TestVersion })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BatchTest__36B12243");
            });

            modelBuilder.Entity<Ecu>(entity =>
            {
                entity.ToTable("ECU");

                entity.Property(e => e.EcuId).ValueGeneratedNever();

                entity.Property(e => e.EcuModel)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Procedure>(entity =>
            {
                entity.HasKey(e => e.ProcId);

                entity.Property(e => e.ProcId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Script)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Requirement>(entity =>
            {
                entity.HasKey(e => e.ReqId);

                entity.Property(e => e.ReqId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Requirement)
                    .HasForeignKey(d => new { d.TestId, d.TestVersion })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Requirement__37A5467C");
            });

            modelBuilder.Entity<Simulator>(entity =>
            {
                entity.HasKey(e => e.SimId);

                entity.Property(e => e.SimId).ValueGeneratedNever();

                entity.HasOne(d => d.Ecu)
                    .WithMany(p => p.Simulator)
                    .HasForeignKey(d => d.EcuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Simulator__EcuId__38996AB5");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasKey(e => new { e.TestId, e.TestVersion });

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateRun).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.HasOne(d => d.Ecu)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.EcuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Test__EcuId__3A81B327");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Test__UserId__398D8EEE");
            });

            modelBuilder.Entity<TestProc>(entity =>
            {
                entity.HasKey(e => new { e.TestId, e.TestVersion, e.ProcId, e.BatchId, e.BatchVersion });

                entity.Property(e => e.Parameters)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.Proc)
                    .WithMany(p => p.TestProc)
                    .HasForeignKey(d => d.ProcId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestProc__ProcId__3C69FB99");

                entity.HasOne(d => d.Req)
                    .WithMany(p => p.TestProc)
                    .HasForeignKey(d => d.ReqId)
                    .HasConstraintName("FK__TestProc__ReqId__3D5E1FD2");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.TestProc)
                    .HasForeignKey(d => new { d.BatchId, d.BatchVersion })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestProc__3E52440B");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.TestProc)
                    .HasForeignKey(d => new { d.TestId, d.TestVersion })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TestProc__3B75D760");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Privilege)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });
        }
    }
}
