using DemoLibrary.Business.Models;
using DemoLibrary.Domain.Models;
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
        // person hasMany pictures
        modelBuilder.Entity<PersonModel>()
            .HasMany(p => p.Pictures)
            .WithOne(b => b.Person)
            .HasForeignKey(b => b.PersonId)
            .OnDelete(DeleteBehavior.Cascade);



        modelBuilder.Entity<PersonModel>()
            // person hasMany bands
            .HasMany(p => p.Bands)
            // bands can haveMany people/members
            .WithMany(b => b.Members);

        // bands have a creator
        modelBuilder.Entity<BandModel>()
            .HasOne(b => b.Creator);
    }

    public DbSet<PersonModel> People { get; set; }
    public DbSet<PictureModel> Picture { get; set; }
    public DbSet<BandModel> Band { get; set; }
}