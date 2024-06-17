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
        modelBuilder.Entity<Person>()
            .HasMany(p => p.Pictures)
            .WithOne(b => b.Person)
            .HasForeignKey(b => b.PersonId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Person>()
            // person hasMany bands
            .HasMany(p => p.Bands)
            // bands can haveMany people/members
            .WithMany(b => b.Members);

        // bands have a creator
        modelBuilder.Entity<Band>()
            .HasOne(b => b.Creator);
    }

    public DbSet<Person> People { get; set; }
    public DbSet<Picture> Picture { get; set; }
    public DbSet<Band> Band { get; set; }
    public DbSet<ConfirmationToken> ConfirmationToken { get; set; }
    public DbSet<Blacklist> Blacklist { get; set; }
}