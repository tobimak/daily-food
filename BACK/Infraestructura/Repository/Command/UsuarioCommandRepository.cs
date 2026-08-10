using Dominio.Entities;
using Dominio.Interfaces_repository.Command;
using Infraestructura.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repository.Command
{
    public class UsuarioCommandRepository : IUsuarioCommandRepository
    {
        private readonly AppDbContext _context;

        public UsuarioCommandRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> CrearAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> ModificarAsync(Usuario usuario)
        {
            var existente = await _context.Usuarios.FindAsync(usuario.Id);
            if (existente is null) return null;

            existente.Nombre = usuario.Nombre;
            existente.Email = usuario.Email;
            existente.Contrasena = usuario.Contrasena;
            existente.Foto = usuario.Foto;// ya viene hasheada desde Aplicación

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var existente = await _context.Usuarios.FindAsync(id);
            if (existente is null) return false;

            _context.Usuarios.Remove(existente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
