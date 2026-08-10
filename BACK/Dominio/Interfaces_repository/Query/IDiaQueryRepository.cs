using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Interfaces_repository.Query
{
    public interface IDiaQueryRepository
    {
        Task<Dia?> ObtenerPorFechaAsync(DateTime fecha, int idUsuario);      // con sus platos cargados
        Task<IEnumerable<Dia>> ListarPorMesAsync(int anio, int mes, int idUsuario); // para el calendario
    }
}
