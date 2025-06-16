using TaskFlow.Models;

public interface IAuthStateService
{
    Usuario? UsuarioActual { get; }
    bool SesionCargada { get; }
    bool EstaLogueado { get; }

    Task IniciarSesionAsync(Usuario usuario);
    Task RestaurarSesionAsync();
    Task CerrarSesionAsync();
}
