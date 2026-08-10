using Dominio.Entities;
using Dominio.Interfaces_repository.Command;
using Infraestructura.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repository.Command
{
    public class DiaCommandRepository : IDiaCommandRepository
    {
        private readonly AppDbContext _context;

        public DiaCommandRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dia> CrearAsync(Dia dia)
        {
            _context.Dias.Add(dia);
            await _context.SaveChangesAsync();
            return dia;
        }

        public async Task<bool> GuardarNotaAsync(int id, int idUsuario, string nota)
        {
            var dia = await _context.Dias.FirstOrDefaultAsync(d => d.Id == id && d.IdUsuario == idUsuario);
            if (dia is null) return false;

            dia.Nota = nota;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AnadirPlatoAsync(int idDia, int idPlato, TipoComida tipo)
        {
            // Comprobamos que día y plato existen
            var dia = await _context.Dias.FindAsync(idDia);
            var plato = await _context.Platos.FindAsync(idPlato);
            if (dia is null || plato is null) return false;

            // Evitar duplicados (mismo día + plato + tipo)
            var yaExiste = await _context.DiasPlato.AnyAsync(dp =>
                dp.IdDia == idDia && dp.IdPlato == idPlato && dp.TipoComida == tipo);
            if (yaExiste) return false;

            _context.DiasPlato.Add(new DiaPlato
            {
                IdDia = idDia,
                IdPlato = idPlato,
                TipoComida = tipo
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> QuitarPlatoAsync(int idDia, int idPlato, TipoComida tipo)
        {
            var relacion = await _context.DiasPlato
                .FirstOrDefaultAsync(dp => dp.IdDia == idDia && dp.IdPlato == idPlato && dp.TipoComida == tipo);
            if (relacion is null) return false;

            _context.DiasPlato.Remove(relacion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
