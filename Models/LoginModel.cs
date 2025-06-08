﻿using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required]
        public string Contrasena { get; set; } = string.Empty;
    }
}
