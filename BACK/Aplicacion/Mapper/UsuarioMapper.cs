using Aplicacion.DTOs;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Mapper
{
    public static class UsuarioMapper
    {
        public static UsuarioResponse ToResponse(Usuario u) => new(u.Id, u.Nombre, u.Email, u.FechaAlta, u.Foto);
    }
}
