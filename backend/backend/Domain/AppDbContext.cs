using Domain.model;
using Microsoft.EntityFrameworkCore;

public class KanjiDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Belt> Belts { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemReading> ItemReadings { get; set; }
    public DbSet<Review> Reviews { get; set; }

    public KanjiDbContext()
    {
    }

    public KanjiDbContext(
        DbContextOptions<KanjiDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;" +
                "Database=KanjiDB;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;"
            );
        }
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Card>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}