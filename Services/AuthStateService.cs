using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class AuthStateService
    {
        private readonly ProtectedLocalStorage _storage;
        private readonly ApplicationDbContext _context;

        public Usuario? UsuarioActual { get; private set; }
        public bool SesionCargada { get; private set; } = false;

        public AuthStateService(ProtectedLocalStorage storage, ApplicationDbContext context)
        {
            _storage = storage;
            _context = context;
        }

        public async Task IniciarSesionAsync(Usuario usuario)
        {
            UsuarioActual = usuario;
            await _storage.SetAsync("usuarioId", usuario.Id);
        }

        public async Task RestaurarSesionAsync()
        {
            try
            {
                var id = await _storage.GetAsync<int>("usuarioId");
                if (id.Success)
                {
                    UsuarioActual = await _context.Usuarios
                        .Include(u => u.Rol)
                        .FirstOrDefaultAsync(u => u.Id == id.Value);
                }
            }
            catch
            {
                // Ignorar errores
            }

            SesionCargada = true;
        }

        public async Task CerrarSesionAsync()
        {
            UsuarioActual = null;
            await _storage.DeleteAsync("usuarioId");
        }

        public bool EstaLogueado => UsuarioActual != null;
    }
}
