using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ApplicationDbContext: IdentityDbContext<AdminUser>
{
    private readonly EncryptionService _encryptionService;

    public ApplicationDbContext(DbContextOptions options, EncryptionService encryptionService):base(options)
    {
        _encryptionService = encryptionService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AdminUser>()
                .HasMany(u => u.AppUsers)
                .WithOne(p => p.AdminUser)
                .HasForeignKey(u => u.AdminUserId)
                .IsRequired();

            modelBuilder.Entity<AppUser>()
            .HasMany(x => x.WebLists)
            .WithOne(x => x.AppUser)
            .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<WebList>()
                .Property(u => u.Password)
                .HasConversion(
                    v => _encryptionService.Encrypt(v), // Encrypt before saving to the database
                    v => _encryptionService.Decrypt(v)  // Decrypt when retrieving from the database
                );
    }
    public DbSet<AppUser> Users{ get; set; }
    public DbSet<WebList> WebLists{ get; set; }
}