using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class AuthService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public AuthService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Usuario?> LoginAsync(string correo, string contrasena)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Usuarios
            .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Contrasena == contrasena);

        }

        public async Task<bool> RegistrarAsync(RegisterModel model)
        {
            using var context = _contextFactory.CreateDbContext();

            if (await context.Usuarios.AnyAsync(u => u.Correo == model.Correo))
                return false;

            var nuevo = new Usuario
            {
                Nombre = model.Nombre,
                Correo = model.Correo,
                Contrasena = model.Contrasena,
                RolId = 1
            };

            context.Usuarios.Add(nuevo);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
