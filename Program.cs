using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<AuthService>();
builder.Services.AddRazorPages();
var connectionString = builder.Configuration["DefaultConnection"];
Console.WriteLine("🔍 Cadena de conexión: " + connectionString); // para depuración
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<TareaService>();
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(o => o.DetailedErrors = true);






var app = builder.Build();

// Middleware estándar
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
