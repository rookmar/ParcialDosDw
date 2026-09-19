using ApiProductos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProductos.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Marcas> Marcas => Set<Marcas>();
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Marcas>()
            .HasIndex(m => m.Nombre)
            .IsUnique();

        modelBuilder.Entity<Marcas>().Property(m => m.Nombre).IsUnicode(false);
        modelBuilder.Entity<Producto>().Property(p => p.Nombre).IsUnicode(false);
        modelBuilder.Entity<Producto>().Property(p => p.Descripcion).IsUnicode(false);

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Marca)
            .WithMany(m => m.Productos)
            .HasForeignKey(p => p.IdMarca)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
