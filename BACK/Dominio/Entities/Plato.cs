using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entities
{
    public class Plato
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Ingredientes { get; set; } = string.Empty;
        public string Receta { get; set; } = string.Empty;

        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public ICollection<DiaPlato> DiasPlato { get; set; } = new List<DiaPlato>();
    }
}
