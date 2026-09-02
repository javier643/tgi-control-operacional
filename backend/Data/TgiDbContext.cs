using Microsoft.EntityFrameworkCore;
using TgiControl.Models;

namespace TgiControl.Data;

public class TgiDbContext : DbContext
{
    public TgiDbContext(DbContextOptions<TgiDbContext> options) : base(options) { }

    public DbSet<Permit> Permits => Set<Permit>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Permit
        modelBuilder.Entity<Permit>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Number).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.RequestedBy).IsRequired().HasMaxLength(200);
        });

        // Configure Shift
        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ShiftType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.OperationalCenter).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Company).IsRequired().HasMaxLength(100);
            entity.Property(e => e.HandoverNotes).HasMaxLength(2000);
        });

        // Configure User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }
}