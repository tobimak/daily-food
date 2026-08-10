using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.DTOs
{

    public record RegistroRequest(string Nombre, string Email, string Contrasena);
    public record LoginRequest(string Email, string Contrasena);
    public record AuthResponse(string Token, int UsuarioId, string Nombre, string Email, DateTime FechaAlta);
}
