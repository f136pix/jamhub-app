using DemoLibrary.Business.Models;
using DemoLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoLibrary.Infraestructure.DataAccess.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonModel>()
            .HasMany(p => p.Pictures)
            .WithOne(b => b.Person)
            .HasForeignKey(b => b.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }

    public DbSet<PersonModel> People { get; set; }
    public DbSet<PictureModel> Picture { get; set; }
}