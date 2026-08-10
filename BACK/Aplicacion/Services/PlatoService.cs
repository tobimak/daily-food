using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Aplicacion.DTOs;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces;
using Aplicacion.Mapper;
using Dominio.Interfaces_repository.Command;
using Dominio.Interfaces_repository.Query;

namespace Aplicacion.Services
{
    public class PlatoService : IPlatoService
    {
        private readonly IPlatoCommandRepository _command;
        private readonly IPlatoQueryRepository _query;

        public PlatoService(IPlatoCommandRepository command, IPlatoQueryRepository query)
        {
            _command = command;
            _query = query;
        }

        public async Task<PlatoResponse> CrearAsync(CrearPlatoRequest request, int idUsuario)
        {
            var creado = await _command.CrearAsync(PlatoMapper.ToEntity(request, idUsuario));
            return PlatoMapper.ToResponse(creado);
        }

        public async Task<PlatoResponse> ModificarAsync(int id, ActualizarPlatoRequest request, int idUsuario)
        {
            var plato = await _query.ObtenerPorIdAsync(id, idUsuario)
                ?? throw new NotFoundException("Plato no encontrado.");

            plato.Nombre = request.Nombre;
            plato.Ingredientes = request.Ingredientes;
            plato.Receta = request.Receta;

            var actualizado = await _command.ModificarAsync(plato);
            return PlatoMapper.ToResponse(actualizado!);
        }

        public async Task<bool> EliminarAsync(int id, int idUsuario)
        {
            try
            {
                return await _command.EliminarAsync(id, idUsuario);
            }
            catch (DbException) // la FK con Restrict impide borrar si está en un día
            {
                throw new BusinessException("No puedes eliminar un plato asignado a un día. Quítalo del calendario primero.");
            }
        }

        public async Task<PlatoResponse> ObtenerAsync(int id, int idUsuario)
        {
            var plato = await _query.ObtenerPorIdAsync(id, idUsuario)
                ?? throw new NotFoundException("Plato no encontrado.");
            return PlatoMapper.ToResponse(plato);
        }

        public async Task<IEnumerable<PlatoResponse>> ListarAsync(int idUsuario)
        {
            var platos = await _query.ListarPorUsuarioAsync(idUsuario);
            return platos.Select(PlatoMapper.ToResponse);
        }
    }
}
