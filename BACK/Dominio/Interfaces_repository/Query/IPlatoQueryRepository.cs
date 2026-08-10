using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Interfaces_repository.Query
{
    public interface IPlatoQueryRepository
    {
        Task<Plato?> ObtenerPorIdAsync(int id, int idUsuario);
        Task<IEnumerable<Plato>> ListarPorUsuarioAsync(int idUsuario);
    }
}
