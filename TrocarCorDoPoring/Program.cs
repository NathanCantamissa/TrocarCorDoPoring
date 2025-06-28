using System.Diagnostics;
using TrocarCorDoPoring.Servicos;
using TrocarCorDoPoring.Servicos.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILightsOutSolver, LightsOutSolver>();
builder.Services.AddControllersWithViews();
var app = builder.Build();
#if !DEBUG
// Registrar o callback para abrir o navegador ao iniciar
var url = "http://localhost:5000";  // ajuste para a rota e porta corretas
app.Lifetime.ApplicationStarted.Register(() =>
{
    try
    {
        // No Windows, faz um "start" no cmd para abrir a URL padrão
        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
    }
    catch
    {
        // silencia falhas (por exemplo se não for Windows)
    }
});
#endif
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
