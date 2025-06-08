using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class AuthStateService
    {
        private readonly ProtectedSessionStorage _sessionStorage;

        public Usuario? UsuarioActual { get; private set; }

        public AuthStateService(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task IniciarSesionAsync(Usuario usuario)
        {
            UsuarioActual = usuario;
            await _sessionStorage.SetAsync("usuarioId", usuario.Id);
            await _sessionStorage.SetAsync("usuarioNombre", usuario.Nombre);
        }

        public async Task RestaurarSesionAsync()
        {
            var id = await _sessionStorage.GetAsync<int>("usuarioId");
            var nombre = await _sessionStorage.GetAsync<string>("usuarioNombre");

            if (id.Success && nombre.Success)
            {
                UsuarioActual = new Usuario { Id = id.Value, Nombre = nombre.Value };
            }
        }

        public async Task CerrarSesionAsync()
        {
            UsuarioActual = null;
            await _sessionStorage.DeleteAsync("usuarioId");
            await _sessionStorage.DeleteAsync("usuarioNombre");
        }

        public bool EstaLogueado => UsuarioActual != null;
    }
}
