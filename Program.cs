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
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthStateService>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
