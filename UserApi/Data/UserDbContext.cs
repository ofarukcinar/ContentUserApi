using Microsoft.EntityFrameworkCore;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserDetails> UserDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Role -> User (1-N)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> UserDetails (1-1)
        modelBuilder.Entity<UserDetails>()
            .HasOne(ud => ud.User)
            .WithOne(u => u.UserDetails)
            .HasForeignKey<UserDetails>(ud => ud.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Varsayılan değerler
        modelBuilder.Entity<Role>().Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("NOW()");
        modelBuilder.Entity<UserDetails>().Property(ud => ud.CreatedAt).HasDefaultValueSql("NOW()");

        // **Seed Data Ekleniyor**

        // Role Seed Data
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", CreatedAt = DateTime.UtcNow },
            new Role { Id = 2, Name = "User", CreatedAt = DateTime.UtcNow }
        );

        // User Seed Data
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1, Name = "John Doe", Email = "john@example.com", RoleId = 1, CreatedAt = DateTime.UtcNow,
                Password = "Test"
            },
            new User
            {
                Id = 2, Name = "Jane Doe", Email = "jane@example.com", RoleId = 2, CreatedAt = DateTime.UtcNow,
                Password = "Test"
            }
        );

        // UserDetails Seed Data
        modelBuilder.Entity<UserDetails>().HasData(
            new UserDetails { Id = 1, UserId = 1, Address = "123 Main St", CreatedAt = DateTime.UtcNow },
            new UserDetails { Id = 2, UserId = 2, Address = "456 Elm St", CreatedAt = DateTime.UtcNow }
        );
    }
}