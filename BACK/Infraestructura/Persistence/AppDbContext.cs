using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Plato> Platos => Set<Plato>();
        public DbSet<Dia> Dias => Set<Dia>();
        public DbSet<DiaPlato> DiasPlato => Set<DiaPlato>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- USUARIO ----------
            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("usuarios");
                e.HasKey(u => u.Id);
                e.Property(u => u.Nombre).HasMaxLength(100).IsRequired();
                e.Property(u => u.Email).HasMaxLength(150).IsRequired();
                e.Property(u => u.Contrasena).IsRequired();
                e.HasIndex(u => u.Email).IsUnique(); // ⚠️ no permitir emails duplicados
            });

            // ---------- PLATO ----------
            modelBuilder.Entity<Plato>(e =>
            {
                e.ToTable("platos");
                e.HasKey(p => p.Id);
                e.Property(p => p.Nombre).HasMaxLength(120).IsRequired();
                e.Property(p => p.Ingredientes).HasMaxLength(500);
                e.Property(p => p.Receta).HasMaxLength(2000);
                e.HasOne(p => p.Usuario)
                 .WithMany(u => u.Platos)
                 .HasForeignKey(p => p.IdUsuario)
                 .OnDelete(DeleteBehavior.Cascade); // si se borra el usuario, sus platos también
            });

            // ---------- DIA ----------
            modelBuilder.Entity<Dia>(e =>
            {
                e.ToTable("dias");
                e.HasKey(d => d.Id);
                e.Property(d => d.Nota).HasMaxLength(500);
                e.HasOne(d => d.Usuario)
                 .WithMany(u => u.Dias)
                 .HasForeignKey(d => d.IdUsuario)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(d => new { d.IdUsuario, d.Fecha }).IsUnique(); // 1 día por usuario y fecha
            });

            // ---------- DIA_PLATO (N:M con tipo de comida) ----------
            modelBuilder.Entity<DiaPlato>(e =>
            {
                e.ToTable("dias_platos");
                e.HasKey(dp => new { dp.IdDia, dp.IdPlato, dp.TipoComida });
                e.Property(dp => dp.TipoComida).HasConversion<string>().HasMaxLength(20);

                e.HasOne(dp => dp.Dia)
                 .WithMany(d => d.DiasPlato)
                 .HasForeignKey(dp => dp.IdDia)
                 .OnDelete(DeleteBehavior.Cascade); // ✔ Mantén esto

                e.HasOne(dp => dp.Plato)
                 .WithMany(p => p.DiasPlato)
                 .HasForeignKey(dp => dp.IdPlato)
                 .OnDelete(DeleteBehavior.Restrict); // ✅ CAMBIA ESTO
            });
        }
    }
}
