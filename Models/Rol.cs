namespace TaskFlow.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        // Navegación inversa (opcional)
        public ICollection<Usuario>? Usuarios { get; set; }
    }
}
