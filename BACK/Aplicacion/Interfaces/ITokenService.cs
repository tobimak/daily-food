using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Interfaces
{
    public interface ITokenService
    {
        string GenerarToken(Usuario usuario);
    }
}
