using System;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "⚠️Debe escribir su tarea⚠️")]
        [StringLength(100, ErrorMessage = "⚠️Máx. 100 caracteres⚠️")]

        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaLimite { get; set; }

        public bool Completada { get; set; } = false;

        // Relación con usuario (si corresponde)
        public int UsuarioId { get; set; }
    }


}
