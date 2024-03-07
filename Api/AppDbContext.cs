using FormaaS.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormaaS
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormField> FormFields { get; set; }
    }
}
