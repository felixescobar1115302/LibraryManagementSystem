using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.DataAccess.Context;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(c => c.Description)
                  .HasMaxLength(250);

            entity.Property(c => c.CreatedAt)
                  .IsRequired();

            entity.Property(c => c.UpdatedAt)
                  .IsRequired(false);

            entity.HasIndex(c => c.Name)
                  .IsUnique();
        });
    }
}