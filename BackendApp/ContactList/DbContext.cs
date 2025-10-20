using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ContactList.Entities;

public class DbContext : IdentityDbContext<User>
{
    public DbContext(DbContextOptions<DbContext> options)
        : base(options) { }

    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Contact>()
            .Property(c => c.Category)
            .HasConversion<string>();
    }
}