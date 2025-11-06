using Microsoft.EntityFrameworkCore;
using RepositoryManager.Models;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<TypeRepository> TypeRepository => Set<TypeRepository>();
    public DbSet<Repository> Repository => Set<Repository>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TypeRepository>().ToTable("repository_type");
        modelBuilder.Entity<TypeRepository>().HasKey(u => u.id_type);
        
        modelBuilder.Entity<Repository>().ToTable("repository");
        modelBuilder.Entity<Repository>().HasKey(u => u.id_repository);
    }
}