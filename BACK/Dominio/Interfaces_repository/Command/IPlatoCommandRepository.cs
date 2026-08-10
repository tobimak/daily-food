using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Interfaces_repository.Command
{
    public interface IPlatoCommandRepository
    {
        Task<Plato> CrearAsync(Plato plato);
        Task<Plato?> ModificarAsync(Plato plato);
        Task<bool> EliminarAsync(int id, int idUsuario);
    }
}
