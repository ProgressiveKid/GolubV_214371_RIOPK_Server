using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CorporateRiskManagementSystemBack.Domain.Entites;

namespace CorporateRiskManagementSystemBack.Data
{
    public partial class RiskDbContext : DbContext
    {
        public RiskDbContext(DbContextOptions<RiskDbContext> options)
            : base(options)
        {
            if (Database.CanConnect())
            {
                //Database.EnsureDeleted();
                //Database.EnsureCreated();
                Console.WriteLine("sssss");
            }
            else
            {
                //Database.EnsureCreated();
            }
        }

        public virtual DbSet<AuditReport> AuditReports { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Risk> Risks { get; set; } = null!;
        public virtual DbSet<RiskAssessment> RiskAssessments { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("audit_reports_pkey");

                entity.ToTable("audit_reports", "corp_risk_management");

                entity.HasIndex(e => e.AuthorId, "idx_audit_author");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.AuditReports)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("audit_reports_author_id_fkey");

                entity.HasOne(d => d.Department) // ✅ новая связь
                   .WithMany(p => p.AuditReports)
                   .HasForeignKey(d => d.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict) // или SetNull, как хочешь
                   .HasConstraintName("audit_reports_department_id_fkey");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("departments", "corp_risk_management");

                entity.HasIndex(e => e.Name, "departments_name_key")
                    .IsUnique();

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Risk>(entity =>
            {
                entity.ToTable("risks", "corp_risk_management");

                entity.HasIndex(e => e.CreatedById, "idx_risks_created_by");

                entity.Property(e => e.RiskId).HasColumnName("risk_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Likelihood)
                    .HasMaxLength(20)
                    .HasColumnName("likelihood");

                entity.Property(e => e.Severity)
                    .HasMaxLength(20)
                    .HasColumnName("severity");

                entity.Property(e => e.Title)
                    .HasMaxLength(150)
                    .HasColumnName("title");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.Risks)
                    .HasForeignKey(d => d.CreatedById)
                    .HasConstraintName("risks_created_by_id_fkey");

                entity.HasMany(d => d.Departments)
                    .WithMany(p => p.Risks)
                    .UsingEntity<Dictionary<string, object>>(
                        "RiskDepartment",
                        l => l.HasOne<Department>().WithMany().HasForeignKey("DepartmentId").HasConstraintName("risk_departments_department_id_fkey"),
                        r => r.HasOne<Risk>().WithMany().HasForeignKey("RiskId").HasConstraintName("risk_departments_risk_id_fkey"),
                        j =>
                        {
                            j.HasKey("RiskId", "DepartmentId").HasName("risk_departments_pkey");

                            j.ToTable("risk_departments", "corp_risk_management");

                            j.IndexerProperty<int>("RiskId").HasColumnName("risk_id");

                            j.IndexerProperty<int>("DepartmentId").HasColumnName("department_id");
                        });
            });

            modelBuilder.Entity<RiskAssessment>(entity =>
            {
                entity.HasKey(e => e.AssessmentId)
                    .HasName("risk_assessments_pkey");

                entity.ToTable("risk_assessments", "corp_risk_management");

                entity.HasIndex(e => e.RiskId, "idx_assessment_risk");

                entity.HasIndex(e => e.AssessedById, "idx_assessment_user");

                entity.Property(e => e.AssessmentId).HasColumnName("assessment_id");

                entity.Property(e => e.AssessedById).HasColumnName("assessed_by_id");

                entity.Property(e => e.AssessmentDate)
                    .HasColumnName("assessment_date")
                    .HasDefaultValueSql("CURRENT_DATE");

                entity.Property(e => e.ImpactScore).HasColumnName("impact_score");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.ProbabilityScore).HasColumnName("probability_score");

                entity.Property(e => e.RiskId).HasColumnName("risk_id");

                entity.HasOne(d => d.AssessedBy)
                    .WithMany(p => p.RiskAssessments)
                    .HasForeignKey(d => d.AssessedById)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("risk_assessments_assessed_by_id_fkey");

                entity.HasOne(d => d.Risk)
                    .WithMany(p => p.RiskAssessments)
                    .HasForeignKey(d => d.RiskId)
                    .HasConstraintName("risk_assessments_risk_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "corp_risk_management");

                entity.HasIndex(e => e.Email, "users_email_key")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "users_username_key")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(200)
                    .HasColumnName("full_name");

                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
