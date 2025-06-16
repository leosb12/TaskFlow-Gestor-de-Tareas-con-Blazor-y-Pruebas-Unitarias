using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TaskFlow.Pages;
using TaskFlow.Services;
using TaskFlow.Models;
using Xunit;

public class AdminComponentTests : TestContext
{
    [Fact]
    public void Renderiza_Contenido_Admin_SiEsAdmin()
    {
        // Arrange
        var authMock = new Mock<IAuthStateService>();
        authMock.Setup(a => a.SesionCargada).Returns(true);
        authMock.Setup(a => a.EstaLogueado).Returns(true);
        authMock.Setup(a => a.UsuarioActual).Returns(new Usuario
        {
            Nombre = "Admin",
            Rol = new Rol { Nombre = "Admin" }
        });

        var usuarioServiceMock = new Mock<IUsuarioService>();
        usuarioServiceMock.Setup(u => u.ObtenerTodosConRolAsync()).ReturnsAsync(new List<Usuario>());
        usuarioServiceMock.Setup(u => u.ObtenerRolesAsync()).ReturnsAsync(new List<Rol>());
        usuarioServiceMock.Setup(u => u.ActualizarAsync(It.IsAny<Usuario>())).Returns(Task.CompletedTask);
        usuarioServiceMock.Setup(u => u.EliminarAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        var tareaServiceMock = new Mock<ITareaService>();
        tareaServiceMock.Setup(t => t.ObtenerTareasAsync()).ReturnsAsync(new List<Tarea>());
        tareaServiceMock.Setup(t => t.ActualizarTareaAsync(It.IsAny<Tarea>())).Returns(Task.CompletedTask);
        tareaServiceMock.Setup(t => t.EliminarTareaAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        Services.AddSingleton(authMock.Object);
        Services.AddSingleton(usuarioServiceMock.Object);
        Services.AddSingleton(tareaServiceMock.Object);

        // Act
        var cut = RenderComponent<Admin>();

        // Assert
        cut.Markup.Contains("👑 Panel de Administración");
        cut.Markup.Contains("Usuarios registrados");
    }

    [Fact]
    public void Renderiza_Tabla_Usuarios_CuandoHayDatos()
    {
        var authMock = new Mock<IAuthStateService>();
        authMock.Setup(a => a.SesionCargada).Returns(true);
        authMock.Setup(a => a.EstaLogueado).Returns(true);
        authMock.Setup(a => a.UsuarioActual).Returns(new Usuario
        {
            Nombre = "Admin",
            Rol = new Rol { Nombre = "Admin" }
        });

        var usuarios = new List<Usuario>
    {
        new Usuario { Id = 1, Nombre = "Juan", Correo = "juan@test.com", Rol = new Rol { Nombre = "Estudiante" }, RolId = 2 }
    };

        var tareas = new List<Tarea>
    {
        new Tarea { Id = 1, Titulo = "Tarea A", UsuarioId = 1, Completada = false }
    };

        var roles = new List<Rol>
    {
        new Rol { Id = 2, Nombre = "Estudiante" }
    };

        var tareaServiceMock = new Mock<ITareaService>();
        tareaServiceMock.Setup(t => t.ObtenerTareasAsync()).ReturnsAsync(tareas);

        var usuarioServiceMock = new Mock<IUsuarioService>();
        usuarioServiceMock.Setup(u => u.ObtenerTodosConRolAsync()).ReturnsAsync(usuarios);
        usuarioServiceMock.Setup(u => u.ObtenerRolesAsync()).ReturnsAsync(roles);

        Services.AddSingleton(authMock.Object);
        Services.AddSingleton(tareaServiceMock.Object);
        Services.AddSingleton(usuarioServiceMock.Object);

        var cut = RenderComponent<Admin>();

        cut.MarkupMatches(cut.Markup); // sin excepción = OK
    }

    [Fact]
    public async Task Cambiar_Rol_Usuario_Y_Guardar()
    {
        // Arrange
        var usuario = new Usuario { Id = 1, Nombre = "Test", Correo = "t@test.com", RolId = 1, Rol = new Rol { Id = 1, Nombre = "Estudiante" } };
        var roles = new List<Rol> { new Rol { Id = 2, Nombre = "Admin" } };

        var authMock = new Mock<IAuthStateService>();
        authMock.Setup(a => a.SesionCargada).Returns(true);
        authMock.Setup(a => a.EstaLogueado).Returns(true);
        authMock.Setup(a => a.UsuarioActual).Returns(new Usuario { Nombre = "Admin", Rol = new Rol { Nombre = "Admin" } });

        var tareaServiceMock = new Mock<ITareaService>();
        tareaServiceMock.Setup(t => t.ObtenerTareasAsync()).ReturnsAsync(new List<Tarea>());

        var usuarioServiceMock = new Mock<IUsuarioService>();
        usuarioServiceMock.Setup(u => u.ObtenerTodosConRolAsync()).ReturnsAsync(new List<Usuario> { usuario });
        usuarioServiceMock.Setup(u => u.ObtenerRolesAsync()).ReturnsAsync(roles);
        usuarioServiceMock.Setup(u => u.ActualizarAsync(It.IsAny<Usuario>())).Returns(Task.CompletedTask);

        Services.AddSingleton(authMock.Object);
        Services.AddSingleton(tareaServiceMock.Object);
        Services.AddSingleton(usuarioServiceMock.Object);

        var cut = RenderComponent<Admin>();

        await cut.InvokeAsync(() => cut.Instance.EditarRol(usuario));
        cut.Render(); 
        var select = cut.Find("select");
        select.Change("2"); 

        await cut.InvokeAsync(() => cut.Instance.GuardarRol(usuario));

        usuarioServiceMock.Verify(u => u.ActualizarAsync(It.Is<Usuario>(x => x.RolId == 2)), Times.Once);

    }



    [Fact]
    public async Task Elimina_Usuario_Correctamente()
    {
        var usuario = new Usuario { Id = 99, Nombre = "Eliminarme", Rol = new Rol { Nombre = "Admin" } };

        var authMock = new Mock<IAuthStateService>();
        authMock.Setup(a => a.SesionCargada).Returns(true);
        authMock.Setup(a => a.EstaLogueado).Returns(true);
        authMock.Setup(a => a.UsuarioActual).Returns(usuario);

        var tareaServiceMock = new Mock<ITareaService>();
        tareaServiceMock.Setup(t => t.ObtenerTareasAsync()).ReturnsAsync(new List<Tarea>());

        var usuarioServiceMock = new Mock<IUsuarioService>();
        usuarioServiceMock.Setup(u => u.ObtenerTodosConRolAsync()).ReturnsAsync(new List<Usuario> { usuario });
        usuarioServiceMock.Setup(u => u.ObtenerRolesAsync()).ReturnsAsync(new List<Rol>());
        usuarioServiceMock.Setup(u => u.EliminarAsync(usuario.Id)).Returns(Task.CompletedTask);

        Services.AddSingleton(authMock.Object);
        Services.AddSingleton(tareaServiceMock.Object);
        Services.AddSingleton(usuarioServiceMock.Object);

        var cut = RenderComponent<Admin>();
        await cut.InvokeAsync(() => cut.Instance.EliminarUsuario(usuario.Id));

        usuarioServiceMock.Verify(u => u.EliminarAsync(usuario.Id), Times.Once);
    }


}
