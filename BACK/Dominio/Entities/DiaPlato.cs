using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entities
{
    public class DiaPlato
    {
        public int IdPlato { get; set; }
        public Plato Plato { get; set; } = null!;

        public int IdDia { get; set; }
        public Dia Dia { get; set; } = null!;

        public TipoComida TipoComida { get; set; } // el atributo que faltaba en tu UML 😉
    }
}
