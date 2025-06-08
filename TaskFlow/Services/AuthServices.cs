using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> LoginAsync(string correo, string contrasena)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Contrasena == contrasena);
        }

        public async Task<bool> RegistrarAsync(RegisterModel model)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Correo == model.Correo))
                return false;

            var nuevo = new Usuario
            {
                Nombre = model.Nombre,
                Correo = model.Correo,
                Contrasena = model.Contrasena
            };

            _context.Usuarios.Add(nuevo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
