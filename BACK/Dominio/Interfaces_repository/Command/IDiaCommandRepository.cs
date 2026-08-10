using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Interfaces_repository.Command
{
    public interface IDiaCommandRepository
    {
        Task<Dia> CrearAsync(Dia dia);
        Task<bool> GuardarNotaAsync(int id, int idUsuario, string nota);
        Task<bool> AnadirPlatoAsync(int idDia, int idPlato, TipoComida tipo);
        Task<bool> QuitarPlatoAsync(int idDia, int idPlato, TipoComida tipo);
    }
}
