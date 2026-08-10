using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Interfaces_repository.Command
{
    public interface IUsuarioCommandRepository
    {
        Task<Usuario> CrearAsync(Usuario usuario);
        Task<Usuario?> ModificarAsync(Usuario usuario);
        Task<bool> EliminarAsync(int id);
    }
}
