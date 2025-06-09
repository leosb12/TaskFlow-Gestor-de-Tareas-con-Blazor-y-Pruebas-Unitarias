using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class AuthStateService
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly ApplicationDbContext _context;

        public Usuario? UsuarioActual { get; private set; }
        public bool SesionCargada { get; private set; } = false;

        public AuthStateService(ProtectedLocalStorage localStorage, ApplicationDbContext context)
        {
            _localStorage = localStorage;
            _context = context;
        }

        public async Task IniciarSesionAsync(Usuario usuario)
        {
            UsuarioActual = usuario;
            await _localStorage.SetAsync("usuarioId", usuario.Id);
        }

        public async Task RestaurarSesionAsync()
        {
            try
            {
                var id = await _localStorage.GetAsync<int>("usuarioId");
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
            await _localStorage.DeleteAsync("usuarioId");
        }

        public bool EstaLogueado => UsuarioActual != null;
    }
}
