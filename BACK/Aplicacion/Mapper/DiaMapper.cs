using Aplicacion.DTOs;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Mapper
{
    public static class DiaMapper
    {
        public static DiaResponse ToResponse(Dia d) => new(
            d.Id, d.Fecha, d.Nota,
            d.DiasPlato.Select(dp => new PlatoDelDiaResponse(dp.IdPlato, dp.Plato.Nombre, dp.TipoComida)).ToList());
    }
}
