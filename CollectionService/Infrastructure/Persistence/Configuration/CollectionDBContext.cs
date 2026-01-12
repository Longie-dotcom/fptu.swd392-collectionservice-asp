using Domain.Aggregate;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class CollectionDBContext : DbContext
    {
        public CollectionDBContext(DbContextOptions<CollectionDBContext> options)
            : base(options) { }

        // ====================
        // Aggregate Roots
        // ====================
        public DbSet<CollectorProfile> CollectorProfiles => Set<CollectorProfile>();

        // ====================
        // Internal Entities
        // ====================
        public DbSet<CollectionTask> CollectionTasks => Set<CollectionTask>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ====================
            // CollectorProfile (Aggregate Root)
            // ====================
            modelBuilder.Entity<CollectorProfile>(entity =>
            {
                entity.HasKey(cp => cp.CollectorProfileID);

                entity.Property(cp => cp.UserID)
                      .IsRequired();

                entity.Property(cp => cp.ContactInfo)
                      .IsRequired();

                entity.Property(cp => cp.IsActive)
                      .HasDefaultValue(true);

                // Backing field mapping for CollectionTasks
                entity.HasMany(cp => cp.CollectionTasks)
                      .WithOne()
                      .HasForeignKey(ct => ct.CollectorProfileID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(cp => cp.UserID)
                      .IsUnique();
            });

            // ====================
            // CollectionTask (Entity)
            // ====================
            modelBuilder.Entity<CollectionTask>(entity =>
            {
                entity.HasKey(ct => ct.CollectionTaskID);

                entity.Property(ct => ct.CollectionReportID)
                      .IsRequired();

                entity.Property(ct => ct.Note);

                entity.Property(ct => ct.ImageName);

                entity.Property(ct => ct.AmountEstimated)
                      .IsRequired();

                entity.Property(ct => ct.Status)
                      .IsRequired();

                entity.Property(ct => ct.AssignedAt)
                      .IsRequired();

                entity.Property(ct => ct.StartedAt);

                entity.Property(ct => ct.CompletedAt);

                entity.Property(ct => ct.CollectorProfileID)
                      .IsRequired();
            });
        }
    }
}
