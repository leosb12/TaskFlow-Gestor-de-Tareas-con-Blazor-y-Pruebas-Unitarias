using Xunit;
using TaskFlow.Services;
using TaskFlow.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using TaskFlow.Data;

namespace TaskFlow.Tests
{
    public class UsuarioServiceTests
    {
        private UsuarioService CrearUsuarioServiceConContexto(out ApplicationDbContext context)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(options);

            // Datos iniciales
            context.Roles.Add(new Rol { Id = 1, Nombre = "Admin" });
            context.Usuarios.Add(new Usuario { Id = 1, Nombre = "Prueba", Correo = "prueba@mail.com", RolId = 1 });
            context.SaveChanges();

            return new UsuarioService(context);
        }

        [Fact]
        public async Task ObtenerTodosAsync_DebeRetornarUsuarios()
        {
            var service = CrearUsuarioServiceConContexto(out var context);
            var resultado = await service.ObtenerTodosAsync();
            Assert.NotEmpty(resultado);
            Assert.Equal("Prueba", resultado[0].Nombre);
        }

        [Fact]
        public async Task ActualizarAsync_DebeActualizarNombre()
        {
            var service = CrearUsuarioServiceConContexto(out var context);
            var usuario = await context.Usuarios.FirstAsync();
            usuario.Nombre = "Modificado";

            await service.ActualizarAsync(usuario);

            var actualizado = await context.Usuarios.FindAsync(usuario.Id);
            Assert.Equal("Modificado", actualizado?.Nombre);
        }

        [Fact]
        public async Task ObtenerTodosConRolAsync_DebeIncluirRol()
        {
            var service = CrearUsuarioServiceConContexto(out var context);
            var resultado = await service.ObtenerTodosConRolAsync();
            Assert.NotNull(resultado[0].Rol);
            Assert.Equal("Admin", resultado[0].Rol?.Nombre);
        }

        [Fact]
        public async Task ObtenerRolesAsync_DebeRetornarRoles()
        {
            var service = CrearUsuarioServiceConContexto(out var context);
            var roles = await service.ObtenerRolesAsync();
            Assert.Single(roles);
            Assert.Equal("Admin", roles[0].Nombre);
        }

        [Fact]
        public async Task EliminarAsync_DebeEliminarUsuario()
        {
            var service = CrearUsuarioServiceConContexto(out var context);
            await service.EliminarAsync(1);
            var usuario = await context.Usuarios.FindAsync(1);
            Assert.Null(usuario);
        }

        [Fact]
        public async Task EliminarAsync_NoDebeFallarSiUsuarioNoExiste()
        {
            var service = CrearUsuarioServiceConContexto(out var context);
            var ex = await Record.ExceptionAsync(() => service.EliminarAsync(999));
            Assert.Null(ex); 
        }
    }
}
