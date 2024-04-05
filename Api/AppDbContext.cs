using System.ComponentModel.DataAnnotations;
using FormaaS.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormaaS
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Form> Forms { get; set; }
        
        public DbSet<FormField> FormFields { get; set; }
        
        public DbSet<FormFieldValue> FormFieldValues { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FormField>()
                .OwnsOne(x => x.Details, b =>
                {
                    b.ToJson();
                    b.OwnsOne(x => x.NumberInput);
                    b.OwnsOne(x => x.TextInput);
                });
        }

        public override int SaveChanges()
        {
            Validate();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Validate();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void Validate()
        {
            var entities = from e in ChangeTracker.Entries()
                where e.State == EntityState.Added
                      || e.State == EntityState.Modified
                select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(
                    entity,
                    validationContext,
                    validateAllProperties: true);
            }
        }
    }
}
