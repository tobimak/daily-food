using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Interfaces_repository.Query
{
    public interface IUsuarioQueryRepository
    {
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario?> ObtenerPorEmailAsync(string email); // para login con JWT
        Task<IEnumerable<Usuario>> ListarAsync();
    }
}
