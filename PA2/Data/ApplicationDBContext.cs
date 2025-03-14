using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PA2.Models;


namespace PA2.Data
{
    public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        //represents the Parties Table in teh database
    public DbSet<Party> Parties { get; set; }
        //Represents invitation table in database
    public DbSet<Invitation> Invitations { get; set; }
        //Sets up enum to string conversion for Invitation status
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invitation>()
            .Property(i => i.Status)
            .HasConversion<string>();  // Stores enum as a string

        base.OnModelCreating(modelBuilder);
    }
}
}
