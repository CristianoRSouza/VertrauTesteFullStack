using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.Sobrenome)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);
                
            entity.Property(e => e.Senha)
                .IsRequired();
                
            entity.HasIndex(e => e.Email)
                .IsUnique();
                
            entity.Property(e => e.Genero)
                .HasConversion<int>();
                
            entity.Property(e => e.Role)
                .HasConversion<int>();

            entity.OwnsOne(e => e.Endereco);
        });
    }
}
