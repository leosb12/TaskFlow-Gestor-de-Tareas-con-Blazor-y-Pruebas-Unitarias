using TaskFlow.Models;

namespace TaskFlow.Services
{
    public interface ITareaService
    {
        Task<List<Tarea>> ObtenerTareasAsync();
        Task<List<Tarea>> ObtenerTareasPorUsuarioAsync(int usuarioId);
        Task CrearTareaAsync(Tarea tarea);
        Task ActualizarTareaAsync(Tarea tarea);
        Task EliminarTareaAsync(int id);
    }

}
