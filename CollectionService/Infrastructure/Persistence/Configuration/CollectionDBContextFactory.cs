using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence.Configuration
{
    public class CollectionDBContextFactory : IDesignTimeDbContextFactory<CollectionDBContext>
    {
        public CollectionDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CollectionDBContext>();

            optionsBuilder.UseSqlServer(
                "Server=.;Database=CollectionDB;Trusted_Connection=True;TrustServerCertificate=True");

            return new CollectionDBContext(optionsBuilder.Options);
        }
    }
}