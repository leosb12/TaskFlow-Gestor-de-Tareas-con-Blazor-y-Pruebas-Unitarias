using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskFlow.Models;
using TaskFlow.Data;
using TaskFlow.Services;
using System.Collections.Generic;
using System.Linq;

namespace TaskFlow.Tests
{
    public class TareaServiceTests
    {
        private TareaService CrearTareaService(out ApplicationDbContext context)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(options);

            context.Usuarios.Add(new Usuario { Id = 1, Nombre = "UsuarioTest", Correo = "correo@test.com" });

            context.Tareas.AddRange(
                new Tarea { Id = 1, Descripcion = "Tarea 1", Completada = false, UsuarioId = 1 },
                new Tarea { Id = 2, Descripcion = "Tarea 2", Completada = true, UsuarioId = 1 }
            );

            context.SaveChanges();

            return new TareaService(context);
        }

        [Fact]
        public async Task ObtenerTareasAsync_DeberiaRetornarTodas()
        {
            var service = CrearTareaService(out var context);
            var tareas = await service.ObtenerTareasAsync();

            Assert.Equal(2, tareas.Count);
        }

        [Fact]
        public async Task CrearTareaAsync_DeberiaAgregarNuevaTarea()
        {
            var service = CrearTareaService(out var context);
            var nueva = new Tarea { Descripcion = "Nueva tarea", Completada = false, UsuarioId = 1 };

            await service.CrearTareaAsync(nueva);
            var tareas = await service.ObtenerTareasAsync();

            Assert.Equal(3, tareas.Count);
            Assert.Contains(tareas, t => t.Descripcion == "Nueva tarea");
        }

        [Fact]
        public async Task ActualizarTareaAsync_DeberiaActualizarDescripcion()
        {
            var service = CrearTareaService(out var context);
            var tarea = await context.Tareas.FindAsync(1);
            tarea.Descripcion = "Actualizada";

            await service.ActualizarTareaAsync(tarea);
            var actualizada = await context.Tareas.FindAsync(1);

            Assert.Equal("Actualizada", actualizada?.Descripcion);
        }

        [Fact]
        public async Task MarcarComoCompletadaAsync_DeberiaMarcarTrue()
        {
            var service = CrearTareaService(out var context);
            await service.MarcarComoCompletadaAsync(1);
            var tarea = await context.Tareas.FindAsync(1);

            Assert.True(tarea?.Completada);
        }

        [Fact]
        public async Task MarcarComoCompletadaAsync_NoDebeFallarSiIdInexistente()
        {
            var service = CrearTareaService(out var context);
            var ex = await Record.ExceptionAsync(() => service.MarcarComoCompletadaAsync(999));
            Assert.Null(ex);
        }

        [Fact]
        public async Task EliminarTareaAsync_DeberiaEliminarTarea()
        {
            var service = CrearTareaService(out var context);
            await service.EliminarTareaAsync(1);
            var tarea = await context.Tareas.FindAsync(1);

            Assert.Null(tarea);
        }

        [Fact]
        public async Task EliminarTareaAsync_NoDebeFallarSiTareaNoExiste()
        {
            var service = CrearTareaService(out var context);
            var ex = await Record.ExceptionAsync(() => service.EliminarTareaAsync(999));
            Assert.Null(ex);
        }

        [Fact]
        public async Task ObtenerTareasPorUsuarioAsync_DeberiaFiltrarPorUsuario()
        {
            var service = CrearTareaService(out var context);
            var tareas = await service.ObtenerTareasPorUsuarioAsync(1);

            Assert.All(tareas, t => Assert.Equal(1, t.UsuarioId));
            Assert.Equal(2, tareas.Count);
        }
    }
}
