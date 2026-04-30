using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace lake7.Infrastructure.Context
{
    public class Lake7DbContextFactory : IDesignTimeDbContextFactory<Lake7DbContext>
    {
        public Lake7DbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Lake7DbContext>();

            // Replace with your actual connection string
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=Lake7DB;Trusted_Connection=True;");

            return new Lake7DbContext(optionsBuilder.Options);
        }
    }
}
