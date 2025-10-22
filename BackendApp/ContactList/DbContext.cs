using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ContactList.Entities;

/// <summary>
/// Db Context for Contacts application
/// </summary>
public class DbContext : IdentityDbContext<User>
{
    public DbContext(DbContextOptions<DbContext> options)
        : base(options) { }

    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Subcategory> Subcategories => Set<Subcategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Relationships
        modelBuilder.Entity<Contact>()
            .HasOne(c => c.Category)
            .WithMany()
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contact>()
            .HasOne(c => c.Subcategory)
            .WithMany()
            .HasForeignKey(c => c.SubcategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Subcategory>()
            .HasOne(s => s.Category)
            .WithMany(c => c.Subcategories)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed predefined categories
        var catPrivate = new Category { Id = 1, Name = "Private", VisibleName = "Private" };
        var catBusiness = new Category { Id = 2, Name = "Business", VisibleName = "Business" };
        var catOther = new Category { Id = 3, Name = "Other", VisibleName = "Other" };
        modelBuilder.Entity<Category>().HasData(catPrivate, catBusiness, catOther);

        // Seed predefined subcategories for Business
        modelBuilder.Entity<Subcategory>().HasData(
            new Subcategory { Id = 1, Name = "Boss", VisibleName = "Boss", CategoryId = catBusiness.Id },
            new Subcategory { Id = 2, Name = "Client", VisibleName = "Client", CategoryId = catBusiness.Id },
            new Subcategory { Id = 3, Name = "Coworker", VisibleName = "Coworker", CategoryId = catBusiness.Id }
        );
    }
}
