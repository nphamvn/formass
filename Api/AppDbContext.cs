using FormaaS.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormaaS
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormField> FormFields { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FormField>()
                .OwnsOne(x => x.DetailsJsonColumn, b =>
                {
                    b.ToJson();
                });
        }
    }
}
