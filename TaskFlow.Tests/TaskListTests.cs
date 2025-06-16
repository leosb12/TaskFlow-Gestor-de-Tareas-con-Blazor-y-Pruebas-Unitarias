using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskFlow.Models;
using TaskFlow.Services;
using Xunit;

namespace TaskFlow.Tests
{
    public class TaskListTests : TestContext
    {
        [Fact]
        public async Task Muestra_Mensaje_Cuando_No_Hay_Tareas()
        {
            // Arrange
            var mockTareaService = new Mock<ITareaService>();
            mockTareaService
                .Setup(x => x.ObtenerTareasPorUsuarioAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Tarea>()); // ahora sí coincide con el componente

            var mockAuth = new Mock<IAuthStateService>();
            mockAuth.Setup(x => x.EstaLogueado).Returns(true);
            mockAuth.Setup(x => x.SesionCargada).Returns(true);
            mockAuth.Setup(x => x.UsuarioActual).Returns(new Usuario
            {
                Id = 1,
                Nombre = "Tester",
                Rol = new Rol { Nombre = "Usuario" }
            });

            Services.AddSingleton(mockTareaService.Object);
            Services.AddSingleton(mockAuth.Object);

            // Act
            var comp = RenderComponent<TaskFlow.Pages.TaskList>();

            // Assert
            Assert.Contains("No hay tareas registradas", comp.Markup);
        }



        [Fact]
        public void Muestra_Tareas_AlRenderizarSiHaySesion()
        {
            var tareasMock = new List<Tarea>
    {
        new Tarea { Id = 1, Titulo = "Tarea 1", Completada = false },
        new Tarea { Id = 2, Titulo = "Tarea 2", Completada = true }
    };

            var mockTareaService = new Mock<ITareaService>();
            mockTareaService
                .Setup(x => x.ObtenerTareasPorUsuarioAsync(It.IsAny<int>()))
                .ReturnsAsync(tareasMock);

            var mockAuth = new Mock<IAuthStateService>();
            mockAuth.Setup(x => x.EstaLogueado).Returns(true);
            mockAuth.Setup(x => x.SesionCargada).Returns(true);
            mockAuth.Setup(x => x.UsuarioActual).Returns(new Usuario
            {
                Id = 1,
                Nombre = "Tester",
                Rol = new Rol { Nombre = "Usuario" }
            });

            Services.AddSingleton(mockTareaService.Object);
            Services.AddSingleton(mockAuth.Object);

            var comp = RenderComponent<TaskFlow.Pages.TaskList>();

            Assert.Contains("Tarea 1", comp.Markup);
            Assert.Contains("Tarea 2", comp.Markup);
        }


        [Fact]
        public void Puede_Marcar_Tarea_ComoCompletada()
        {
            var tarea = new Tarea { Id = 1, Titulo = "Completar tarea", Completada = false };

            var mockTareaService = new Mock<ITareaService>();
            mockTareaService.Setup(x => x.ObtenerTareasPorUsuarioAsync(It.IsAny<int>()))
                            .ReturnsAsync(new List<Tarea> { tarea });
            mockTareaService.Setup(x => x.ActualizarTareaAsync(It.IsAny<Tarea>()))
                            .Returns(Task.CompletedTask);

            var mockAuth = new Mock<IAuthStateService>();
            mockAuth.Setup(x => x.EstaLogueado).Returns(true);
            mockAuth.Setup(x => x.SesionCargada).Returns(true);
            mockAuth.Setup(x => x.UsuarioActual).Returns(new Usuario { Id = 1, Nombre = "Tester", Rol = new Rol { Nombre = "Usuario" } });

            Services.AddSingleton(mockTareaService.Object);
            Services.AddSingleton(mockAuth.Object);

            var comp = RenderComponent<TaskFlow.Pages.TaskList>();

            var checkbox = comp.Find("input[type=checkbox]");
            checkbox.Change(true);

            mockTareaService.Verify(x => x.ActualizarTareaAsync(It.Is<Tarea>(t => t.Completada)), Times.Once);
        }


        [Fact]
        public void Puede_Eliminar_Tarea()
        {
            var tarea = new Tarea { Id = 10, Titulo = "Eliminar tarea", Completada = false };

            var mockTareaService = new Mock<ITareaService>();
            mockTareaService.Setup(x => x.ObtenerTareasPorUsuarioAsync(It.IsAny<int>()))
                            .ReturnsAsync(new List<Tarea> { tarea });
            mockTareaService.Setup(x => x.EliminarTareaAsync(It.IsAny<int>()))
                            .Returns(Task.CompletedTask);

            var mockAuth = new Mock<IAuthStateService>();
            mockAuth.Setup(x => x.EstaLogueado).Returns(true);
            mockAuth.Setup(x => x.SesionCargada).Returns(true);
            mockAuth.Setup(x => x.UsuarioActual).Returns(new Usuario { Id = 1, Nombre = "Tester", Rol = new Rol { Nombre = "Usuario" } });

            Services.AddSingleton(mockTareaService.Object);
            Services.AddSingleton(mockAuth.Object);

            var comp = RenderComponent<TaskFlow.Pages.TaskList>();

            var botonEliminar = comp.Find("button.btn.btn-sm");
            botonEliminar.Click();

            mockTareaService.Verify(x => x.EliminarTareaAsync(tarea.Id), Times.Once);
        }


        [Fact]
        public void No_Crea_Tarea_Si_Titulo_Esta_Vacio()
        {
            var mockTareaService = new Mock<ITareaService>();
            mockTareaService.Setup(x => x.ObtenerTareasPorUsuarioAsync(It.IsAny<int>()))
                            .ReturnsAsync(new List<Tarea>());

            var mockAuth = new Mock<IAuthStateService>();
            mockAuth.Setup(x => x.EstaLogueado).Returns(true);
            mockAuth.Setup(x => x.SesionCargada).Returns(true);
            mockAuth.Setup(x => x.UsuarioActual).Returns(new Usuario { Id = 1, Nombre = "Tester", Rol = new Rol { Nombre = "Usuario" } });

            Services.AddSingleton(mockTareaService.Object);
            Services.AddSingleton(mockAuth.Object);

            var comp = RenderComponent<TaskFlow.Pages.TaskList>();

            var botonAgregar = comp.Find("button[type=submit]");
            botonAgregar.Click();

            mockTareaService.Verify(x => x.CrearTareaAsync(It.IsAny<Tarea>()), Times.Never);
        }

    }
}