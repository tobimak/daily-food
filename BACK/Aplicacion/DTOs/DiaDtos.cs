using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Entities;

namespace Aplicacion.DTOs
{

    public record AnadirPlatoADiaRequest(DateTime Fecha, int IdPlato, TipoComida TipoComida);
    public record PlatoDelDiaResponse(int IdPlato, string Nombre, TipoComida TipoComida);
    public record DiaResponse(int Id, DateTime Fecha, string? Nota, List<PlatoDelDiaResponse> Platos);
    public record SugerenciaResponse(int IdPlato, string NombrePlato, TipoComida TipoComida, string Motivo);

    public record GuardarNotaRequest(DateTime Fecha, string Nota);
}
