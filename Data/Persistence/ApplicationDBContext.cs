using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<VisitEntity> Visits { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PersonEntity>(entity =>
            {
                entity.ToTable("Persons");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);
                entity.Ignore(e => e.FullName);
                entity.Property<DateTime>("CreatedAt").IsRequired().HasDefaultValueSql("GETUTCDATE()");
                entity.Property<DateTime>("UpdatedAt").IsRequired().HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<VisitEntity>(entity =>
            {
                entity.ToTable("Visits");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.PersonId).IsRequired();
                entity.Property(e => e.EntryTime).IsRequired();
                entity.Property(e => e.ExitTime).IsRequired(false);
                entity.HasOne(e => e.Person).WithMany().HasForeignKey(e => e.PersonId).OnDelete(DeleteBehavior.Restrict);
                entity.Ignore(e => e.IsActive);
                entity.Ignore(e => e.Duration);

                entity.HasIndex(e => e.PersonId);
                entity.HasIndex(e => e.EntryTime);
                entity.HasIndex(e => new { e.PersonId, e.ExitTime });

                entity.Property<DateTime>("CreatedAt").IsRequired().HasDefaultValueSql("GETUTCDATE()");
                entity.Property<DateTime>("UpdatedAt").IsRequired().HasDefaultValueSql("GETUTCDATE()");
            });

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateDatetimes();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateDatetimes()
        {
            var entries = ChangeTracker.Entries().Where(e => e.State.Equals(EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.Metadata.FindProperty("UpdatedAt") != null)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
