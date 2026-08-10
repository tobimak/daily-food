using Dominio.Entities;
using Dominio.Interfaces_repository.Query;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Repository.Query
{
    public class UsuarioQueryRepository : IUsuarioQueryRepository
    {
        private readonly AppDbContext _context;

        public UsuarioQueryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Usuario>> ListarAsync()
        {
            return await _context.Usuarios
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
