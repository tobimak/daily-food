using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entities
{
    public class Dia
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Nota { get; set; }

        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public ICollection<DiaPlato> DiasPlato { get; set; } = new List<DiaPlato>();
    }
}
