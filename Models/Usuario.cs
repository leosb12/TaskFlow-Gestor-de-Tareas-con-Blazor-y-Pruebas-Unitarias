﻿using System.Collections.Generic;

namespace TaskFlow.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;

        public int RolId { get; set; }
        public Rol? Rol { get; set; }
    }

}
