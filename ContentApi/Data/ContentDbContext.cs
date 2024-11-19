using Microsoft.EntityFrameworkCore;

public class ContentDbContext : DbContext
{
    public ContentDbContext(DbContextOptions<ContentDbContext> options) : base(options)
    {
    }

    public DbSet<Content> Contents { get; set; }
}