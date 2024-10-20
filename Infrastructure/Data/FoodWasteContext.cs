using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class FoodWasteContext : IdentityDbContext
{
    public FoodWasteContext()
    {
    }

    public FoodWasteContext(DbContextOptions<FoodWasteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cantine> Cantines { get; set; }

    public virtual DbSet<Employ> Employs { get; set; }

    public virtual DbSet<Pakkage> Pakkages { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cantine>(entity =>
        {
            entity.HasKey(e => new { e.City, e.Location });

            entity.ToTable("Cantine");

            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Location)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Employ>(entity =>
        {
            entity.HasKey(e => e.EmployNr);

            entity.ToTable("Employ");

            entity.Property(e => e.EmployNr).ValueGeneratedNever();
            entity.Property(e => e.Cantine)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.CantineNavigation).WithMany(p => p.Employs)
                .HasForeignKey(d => new { d.City, d.Cantine })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employ_Cantine");
        });

        modelBuilder.Entity<Pakkage>(entity =>
        {
            entity.HasKey(e => e.Title);

            entity.ToTable("Pakkage");

            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Cantine)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.ExperationDate).HasColumnType("datetime");
            entity.Property(e => e.PickUpDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.ReservedForNavigation).WithMany(p => p.Pakkages)
                .HasForeignKey(d => d.ReservedFor)
                .HasConstraintName("FK_Pakkage_Student");

            entity.HasOne(d => d.CantineNavigation).WithMany(p => p.Pakkages)
                .HasForeignKey(d => new { d.City, d.Cantine })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pakkage_Cantine");

            entity.HasMany(d => d.Products).WithMany(p => p.Pakkages)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductPackage",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("Product")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductPackages_Product"),
                    l => l.HasOne<Pakkage>().WithMany()
                        .HasForeignKey("Pakkage")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductPackages_Pakkage"),
                    j =>
                    {
                        j.HasKey("Pakkage", "Product");
                        j.ToTable("ProductPackages");
                        j.IndexerProperty<string>("Pakkage").HasMaxLength(50);
                        j.IndexerProperty<string>("Product").HasMaxLength(50);
                    });
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Title);

            entity.ToTable("Product");

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentNr);

            entity.ToTable("Student");

            entity.Property(e => e.StudentNr).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
