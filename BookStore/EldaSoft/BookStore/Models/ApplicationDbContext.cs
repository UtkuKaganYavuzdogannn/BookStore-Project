using BookStore.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<customers> Customers { get; set; }
    public DbSet<sellers> Sellers { get; set; }
    public DbSet<books> Books { get; set; }
    public DbSet<orders> Orders { get; set; }
    public DbSet<admin> Admin { get; set; }

}
