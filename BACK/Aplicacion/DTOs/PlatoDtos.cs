using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.DTOs
{

    public record CrearPlatoRequest(string Nombre, string Ingredientes, string Receta);
    public record ActualizarPlatoRequest(string Nombre, string Ingredientes, string Receta);
    public record PlatoResponse(int Id, string Nombre, string Ingredientes, string Receta);
}
