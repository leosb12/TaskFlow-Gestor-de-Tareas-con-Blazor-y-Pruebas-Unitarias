using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;

namespace TaskFlow.Services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<AuthService>();
            services.AddScoped<ITareaService, TareaService>();
            services.AddScoped<IAuthStateService, AuthStateService>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<ProtectedLocalStorage>();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(o => o.DetailedErrors = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }

}
