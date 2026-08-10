using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Dominio.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty; // SIEMPRE hash, nunca plana
        public DateTime FechaAlta { get; set; } = DateTime.UtcNow;

        public string? Foto { get; set; }

        public ICollection<Plato> Platos { get; set; } = new List<Plato>();
        public ICollection<Dia> Dias { get; set; } = new List<Dia>();
    }
}
