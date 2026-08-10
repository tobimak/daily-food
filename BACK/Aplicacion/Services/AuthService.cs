using Aplicacion.DTOs;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces;
using Dominio.Entities;
using Dominio.Interfaces_repository.Command;
using Dominio.Interfaces_repository.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioCommandRepository _usuarioCommand;
        private readonly IUsuarioQueryRepository _usuarioQuery;
        private readonly ITokenService _tokenService;

        public AuthService(IUsuarioCommandRepository usuarioCommand,
                           IUsuarioQueryRepository usuarioQuery,
                           ITokenService tokenService)
        {
            _usuarioCommand = usuarioCommand;
            _usuarioQuery = usuarioQuery;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> RegistrarAsync(RegistroRequest request)
        {
            var existente = await _usuarioQuery.ObtenerPorEmailAsync(request.Email);
            if (existente is not null)
                throw new BusinessException("Ya existe una cuenta con ese email.");

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Email = request.Email,
                Contrasena = PasswordHasher.Hash(request.Contrasena) // 🔒 nunca en plano
            };

            var creado = await _usuarioCommand.CrearAsync(usuario);
            return new AuthResponse(_tokenService.GenerarToken(creado),
                                    creado.Id, creado.Nombre, creado.Email, creado.FechaAlta);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var usuario = await _usuarioQuery.ObtenerPorEmailAsync(request.Email);
            if (usuario is null || !PasswordHasher.Verify(request.Contrasena, usuario.Contrasena))
                throw new UnauthorizedException("Email o contraseña incorrectos.");

            return new AuthResponse(_tokenService.GenerarToken(usuario),
                                    usuario.Id, usuario.Nombre, usuario.Email, usuario.FechaAlta);
        }
    }
}
