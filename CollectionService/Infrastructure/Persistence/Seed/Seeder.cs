using Domain.Aggregate;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seed
{
    public static class Seeder
    {
        public static async Task SeedAsync(CollectionDBContext context)
        {
            // Seed CollectorProfiles
            if (!await context.CollectorProfiles.AnyAsync())
            {
                var collector1 = new CollectorProfile(
                    userId: Guid.NewGuid(),
                    contactInfo: "0123456789"
                );
                var collector2 = new CollectorProfile(
                    userId: Guid.NewGuid(),
                    contactInfo: "0987654321"
                );

                await context.CollectorProfiles.AddRangeAsync(collector1, collector2);
                await context.SaveChangesAsync();
            }

            // Seed CollectionTasks (FK CollectorProfileID)
            if (!await context.CollectionTasks.AnyAsync())
            {
                var collectorProfile = await context.CollectorProfiles.FirstAsync();
                var firstReportId = Guid.NewGuid();

                var tasks = new[]
                {
            new CollectionTask(
                collectionTaskId: Guid.NewGuid(),
                collectionReportId: firstReportId,
                collectorProfileId: collectorProfile.CollectorProfileID,
                note: "Thu nợ khách A",
                imageName: "image_a.jpg",
                amountEstimated: 5000000
            ),
            new CollectionTask(
                collectionTaskId: Guid.NewGuid(),
                collectionReportId: firstReportId,
                collectorProfileId: collectorProfile.CollectorProfileID,
                note: "Thu nợ khách B",
                imageName: null,
                amountEstimated: 3000000
            )
        };

                await context.CollectionTasks.AddRangeAsync(tasks);
                await context.SaveChangesAsync();
            }
        }
    }
}
