using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.DTOs
{

    public record UsuarioResponse(int Id, string Nombre, string Email, DateTime FechaAlta, string? Foto);
    public record ActualizarUsuarioRequest(string Nombre, string Email, string? ContrasenaNueva);
    public record GuardarFotoRequest(string Foto);

}
