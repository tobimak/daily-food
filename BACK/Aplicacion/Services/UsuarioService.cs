using Aplicacion.DTOs;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces;
using Aplicacion.Mapper;
using Dominio.Interfaces_repository.Command;
using Dominio.Interfaces_repository.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioCommandRepository _command;
        private readonly IUsuarioQueryRepository _query;

        public UsuarioService(IUsuarioCommandRepository command, IUsuarioQueryRepository query)
        {
            _command = command;
            _query = query;
        }

        public async Task<UsuarioResponse> ObtenerAsync(int id)
        {
            var usuario = await _query.ObtenerPorIdAsync(id)
                ?? throw new NotFoundException("Usuario no encontrado.");
            return UsuarioMapper.ToResponse(usuario);
        }

        public async Task<UsuarioResponse> ActualizarAsync(int id, ActualizarUsuarioRequest request)
        {
            var usuario = await _query.ObtenerPorIdAsync(id)
                ?? throw new NotFoundException("Usuario no encontrado.");

            if (!usuario.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var otro = await _query.ObtenerPorEmailAsync(request.Email);
                if (otro is not null) throw new BusinessException("Ese email ya está en uso.");
            }

            usuario.Nombre = request.Nombre;
            usuario.Email = request.Email;
            if (!string.IsNullOrWhiteSpace(request.ContrasenaNueva))
                usuario.Contrasena = PasswordHasher.Hash(request.ContrasenaNueva);

            var actualizado = await _command.ModificarAsync(usuario);
            return UsuarioMapper.ToResponse(actualizado!);
        }

        public async Task<UsuarioResponse> GuardarFotoAsync(int id, string fotoBase64)
        {
            if (string.IsNullOrWhiteSpace(fotoBase64) || !fotoBase64.StartsWith("data:image"))
                throw new BusinessException("Formato de imagen no válido.");
            if (fotoBase64.Length > 500_000)
                throw new BusinessException("La imagen es demasiado grande.");

            var usuario = await _query.ObtenerPorIdAsync(id)
                ?? throw new NotFoundException("Usuario no encontrado.");

            usuario.Foto = fotoBase64;
            var actualizado = await _command.ModificarAsync(usuario);
            return UsuarioMapper.ToResponse(actualizado!);
        }

        public Task<bool> EliminarAsync(int id) => _command.EliminarAsync(id);
    }
}
