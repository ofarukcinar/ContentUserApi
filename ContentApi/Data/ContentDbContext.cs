using Microsoft.EntityFrameworkCore;
using System;

public class ContentDbContext : DbContext
{
    public ContentDbContext(DbContextOptions<ContentDbContext> options) : base(options)
    {
    }

    public DbSet<Content> Contents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Content>().Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");

        modelBuilder.Entity<Content>().HasData(
            new Content { Id = 1, Title = "Welcome to Content API", Body = "This is the first content.", CreatedAt = DateTime.UtcNow },
            new Content { Id = 2, Title = "Second Content", Body = "This is another piece of content.", CreatedAt = DateTime.UtcNow }
        );
    }
}
