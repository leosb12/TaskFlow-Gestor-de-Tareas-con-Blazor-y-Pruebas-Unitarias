using TaskFlow.Models;

namespace TaskFlow.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ObtenerTodosConRolAsync();
        Task<List<Rol>> ObtenerRolesAsync();
        Task ActualizarAsync(Usuario usuario);
        Task EliminarAsync(int id);
    }

}
