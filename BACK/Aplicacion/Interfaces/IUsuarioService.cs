using Aplicacion.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponse> ObtenerAsync(int id);
        Task<UsuarioResponse> ActualizarAsync(int id, ActualizarUsuarioRequest request);
        Task<bool> EliminarAsync(int id);

        Task<UsuarioResponse> GuardarFotoAsync(int id, string fotoBase64);
    }
}
