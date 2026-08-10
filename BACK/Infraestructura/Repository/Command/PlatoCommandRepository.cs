using Dominio.Entities;
using Dominio.Interfaces_repository.Command;
using Infraestructura.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repository.Command
{
    public class PlatoCommandRepository : IPlatoCommandRepository
    {
        private readonly AppDbContext _context;

        public PlatoCommandRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Plato> CrearAsync(Plato plato)
        {
            _context.Platos.Add(plato);
            await _context.SaveChangesAsync();
            return plato;
        }

        public async Task<Plato?> ModificarAsync(Plato plato)
        {
            var existente = await _context.Platos.FindAsync(plato.Id);
            if (existente is null || existente.IdUsuario != plato.IdUsuario) return null;

            existente.Nombre = plato.Nombre;
            existente.Ingredientes = plato.Ingredientes;
            existente.Receta = plato.Receta;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> EliminarAsync(int id, int idUsuario)
        {
            var existente = await _context.Platos.FirstOrDefaultAsync(p => p.Id == id && p.IdUsuario == idUsuario);
            if (existente is null) return false;

            _context.Platos.Remove(existente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
