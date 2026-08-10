using Aplicacion.DTOs;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Mapper
{
    public static class PlatoMapper
    {
        public static Plato ToEntity(CrearPlatoRequest r, int idUsuario) => new()
        {
            Nombre = r.Nombre,
            Ingredientes = r.Ingredientes,
            Receta = r.Receta,
            IdUsuario = idUsuario
        };

        public static PlatoResponse ToResponse(Plato p) => new(p.Id, p.Nombre, p.Ingredientes, p.Receta);
    }
}
