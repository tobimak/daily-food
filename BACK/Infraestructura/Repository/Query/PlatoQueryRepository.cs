using Dominio.Entities;
using Dominio.Interfaces_repository.Query;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Repository.Query
{
    public class PlatoQueryRepository : IPlatoQueryRepository
    {
        private readonly AppDbContext _context;

        public PlatoQueryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Plato?> ObtenerPorIdAsync(int id, int idUsuario)
        {
            return await _context.Platos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.IdUsuario == idUsuario);
        }

        public async Task<IEnumerable<Plato>> ListarPorUsuarioAsync(int idUsuario)
        {
            return await _context.Platos
                .AsNoTracking()
                .Where(p => p.IdUsuario == idUsuario)
                .ToListAsync();
        }
    }
}
