using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TSM.Core.Models;

namespace TSM.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }
        public DbSet<TechnologyStatus> TechnologyStatus { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }
        public DbSet<ProjectTeamMember> ProjectTeamMembers { get; set; }
        public DbSet<TechnologySkill> TechnologySkills { get; set; }
        public DbSet<UserTechnology> UserTechnologies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Technology>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Technologies)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<TechnologyStatus>()
                .HasOne(ts => ts.Team)
                .WithMany(t => t.TechnologyStatuses)
                .HasForeignKey(ts => ts.TeamId);

            modelBuilder.Entity<TechnologyStatus>()
                .HasOne(ts => ts.Technology)
                .WithMany(t => t.TechnologyStatuses)
                .HasForeignKey(ts => ts.TechnologyId);

            modelBuilder.Entity<TechnologyStatus>()
                .HasOne(ts => ts.Status)
                .WithMany(s => s.TechnologyStatuses)
                .HasForeignKey(ts => ts.StatusId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Team)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TeamId);

            // Configure composite keys for junction tables
            modelBuilder.Entity<ProjectTechnology>()
                .HasKey(pt => new { pt.ProjectId, pt.TechnologyId });

            modelBuilder.Entity<ProjectTechnology>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTechnologies)
                .HasForeignKey(pt => pt.ProjectId);

            modelBuilder.Entity<ProjectTechnology>()
                .HasOne(pt => pt.Technology)
                .WithMany(t => t.ProjectTechnologies)
                .HasForeignKey(pt => pt.TechnologyId);

            modelBuilder.Entity<ProjectTeamMember>()
                .HasKey(ptm => new { ptm.ProjectId, ptm.MemberId });

            modelBuilder.Entity<ProjectTeamMember>()
                .HasOne(ptm => ptm.Project)
                .WithMany(p => p.ProjectTeamMembers)
                .HasForeignKey(ptm => ptm.ProjectId);

            modelBuilder.Entity<ProjectTeamMember>()
                .HasOne(ptm => ptm.TeamMember)
                .WithMany(tm => tm.ProjectTeamMembers)
                .HasForeignKey(ptm => ptm.MemberId);

            modelBuilder.Entity<TechnologySkill>()
                .HasOne(ts => ts.TeamMember)
                .WithMany(tm => tm.TechnologySkills)
                .HasForeignKey(ts => ts.MemberId);

            modelBuilder.Entity<TechnologySkill>()
                .HasOne(ts => ts.Technology)
                .WithMany(t => t.TechnologySkills)
                .HasForeignKey(ts => ts.TechnologyId);

            // Configure TeamMember and ApplicationUser relationship
            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.User)
                .WithMany()
                .HasForeignKey(tm => tm.UserId);

            // Unique constraint for TechnologyStatus (TeamId, TechnologyId)
            modelBuilder.Entity<TechnologyStatus>()
                .HasIndex(ts => new { ts.TeamId, ts.TechnologyId })
                .IsUnique();

            modelBuilder.Entity<UserTechnology>()
                .HasKey(ut => new { ut.UserId, ut.TechnologyId });
        }
    }
} 