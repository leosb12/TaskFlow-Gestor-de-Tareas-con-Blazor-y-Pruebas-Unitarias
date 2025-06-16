using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TaskFlow.Data;
using TaskFlow.Models;
using TaskFlow.Services;
using Xunit;

namespace TaskFlow.Tests
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDB_Auth") 
                .Options;

            _contextFactory = new PooledDbContextFactory<ApplicationDbContext>(options);
            _authService = new AuthService(_contextFactory);

            
            using var context = _contextFactory.CreateDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Fact]
        public async Task RegisterAsync_CreatesUser()
        {
            var result = await _authService.RegistrarAsync(new RegisterModel
            {
                Nombre = "Test",
                Correo = "test@test.com",
                Contrasena = "Test123"
            });

            using var context = _contextFactory.CreateDbContext();
            var user = await context.Usuarios.FirstOrDefaultAsync(u => u.Correo == "test@test.com");

            Assert.NotNull(user);
            Assert.True(result);
        }

        [Fact]
        public async Task RegisterAsync_DuplicateEmail_ReturnsFalse()
        {
            using var context = _contextFactory.CreateDbContext();

            context.Usuarios.Add(new Usuario
            {
                Nombre = "Existente",
                Correo = "repetido@test.com",
                Contrasena = "123",
                RolId = 1
            });
            await context.SaveChangesAsync();

            var result = await _authService.RegistrarAsync(new RegisterModel
            {
                Nombre = "Nuevo",
                Correo = "repetido@test.com",
                Contrasena = "456"
            });

            Assert.False(result);
        }

        [Fact]
        public async Task LoginAsync_CorrectCredentials_ReturnsUser()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Usuarios.Add(new Usuario
                {
                    Nombre = "Login",
                    Correo = "login@test.com",
                    Contrasena = "pass123",
                    RolId = 1
                });
                await context.SaveChangesAsync();
            }

            var user = await _authService.LoginAsync("login@test.com", "pass123");

            Assert.NotNull(user);
            Assert.Equal("Login", user.Nombre);
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_ReturnsNull()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Usuarios.Add(new Usuario
                {
                    Nombre = "Login",
                    Correo = "login@test.com",
                    Contrasena = "correcta",
                    RolId = 1
                });
                await context.SaveChangesAsync();
            }

            var user = await _authService.LoginAsync("login@test.com", "incorrecta");

            Assert.Null(user);
        }

        [Fact]
        public async Task LoginAsync_NonExistentUser_ReturnsNull()
        {
            var user = await _authService.LoginAsync("noexiste@test.com", "pass");
            Assert.Null(user);
        }
    }
}
