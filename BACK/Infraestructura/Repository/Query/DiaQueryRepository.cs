using Dominio.Entities;
using Dominio.Interfaces_repository.Query;
using Infraestructura.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Repository.Query
{
    public class DiaQueryRepository : IDiaQueryRepository
    {
        private readonly AppDbContext _context;

        public DiaQueryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dia?> ObtenerPorFechaAsync(DateTime fecha, int idUsuario)
        {
            return await _context.Dias
                .Include(d => d.DiasPlato)
                    .ThenInclude(dp => dp.Plato)  // cargamos los platos del día (para el calendario)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Fecha.Date == fecha.Date && d.IdUsuario == idUsuario);
        }

        public async Task<IEnumerable<Dia>> ListarPorMesAsync(int anio, int mes, int idUsuario)
        {
            // Devuelve todos los días de ese mes para ese usuario, con sus platos cargados
            return await _context.Dias
                .Include(d => d.DiasPlato)
                    .ThenInclude(dp => dp.Plato)
                .AsNoTracking()
                .Where(d => d.IdUsuario == idUsuario
                         && d.Fecha.Year == anio
                         && d.Fecha.Month == mes)
                .ToListAsync();
        }
    }
}
