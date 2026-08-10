using Aplicacion.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Interfaces
{
    public interface IPlatoService
    {
        Task<PlatoResponse> CrearAsync(CrearPlatoRequest request, int idUsuario);
        Task<PlatoResponse> ModificarAsync(int id, ActualizarPlatoRequest request, int idUsuario);
        Task<bool> EliminarAsync(int id, int idUsuario);
        Task<PlatoResponse> ObtenerAsync(int id, int idUsuario);
        Task<IEnumerable<PlatoResponse>> ListarAsync(int idUsuario);
    }
}
