using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace cks_kaas.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }
        public new DbSet<UserToken> UserTokens { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<NodePool> NodePools { get; set; }
        public DbSet<Namespace> Namespaces { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<BillingPeriod> BillingPeriods { get; set; }
        public DbSet<BillingUsage> BillingUsages { get; set; }
        public DbSet<RbacRole> RbacRoles { get; set; }
        public DbSet<RbacPermission> RbacPermissions { get; set; }
        public DbSet<RbacRolePermission> RbacRolePermissions { get; set; }
        public DbSet<RbacUserRole> RbacUserRoles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Doc> Docs { get; set; }
        public DbSet<HelpTicket> HelpTickets { get; set; }
        public DbSet<QAQuestion> QAQuestions { get; set; }
    // public DbSet<QAAnswer> QAAnswers { get; set; } // QAAnswer model fully removed
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Set decimal precision for billing fields
            builder.Entity<BillingPeriod>()
                .Property(x => x.TotalCost)
                .HasColumnType("decimal(18,2)");
            builder.Entity<BillingUsage>()
                .Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");
            builder.Entity<BillingUsage>()
                .Property(x => x.Cost)
                .HasColumnType("decimal(18,2)");

            // Composite keys and relationships
            builder.Entity<RbacRolePermission>().HasKey(x => new { x.RoleId, x.PermissionId });
            builder.Entity<RbacUserRole>().HasKey(x => new { x.UserId, x.RoleId, x.ClusterId, x.NamespaceId });
            builder.Entity<UserPreferences>().HasKey(x => x.UserId);
            builder.Entity<UserPreferences>()
                .HasOne(x => x.User)
                .WithOne(x => x.Preferences)
                .HasForeignKey<UserPreferences>(x => x.UserId);
            builder.Entity<UserToken>()
                .HasOne(x => x.User)
                .WithMany(x => x.Tokens)
                .HasForeignKey(x => x.UserId);
            builder.Entity<UserSession>()
                .HasOne(x => x.User)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.UserId);
            builder.Entity<RbacUserRole>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId);
            builder.Entity<RbacUserRole>()
                .HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId);
            builder.Entity<RbacRolePermission>()
                .HasOne(x => x.Role)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.RoleId);
            builder.Entity<RbacRolePermission>()
                .HasOne(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.PermissionId);
            // QAAnswer entity configuration fully removed
            // builder.Entity<QAQuestion>()
            //     .HasOne(x => x.User)
            //     .WithMany()
            //     .HasForeignKey(x => x.UserId);
            builder.Entity<HelpTicket>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
            builder.Entity<Notification>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
            builder.Entity<ActivityLog>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
            builder.Entity<Cluster>()
                .HasMany(x => x.NodePools)
                .WithOne(x => x.Cluster)
                .HasForeignKey(x => x.ClusterId);
            builder.Entity<Cluster>()
                .HasMany(x => x.Namespaces)
                .WithOne(x => x.Cluster)
                .HasForeignKey(x => x.ClusterId);
        }
    }
}
