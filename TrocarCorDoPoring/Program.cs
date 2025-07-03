using TrocarCorDoPoring.Servicos;
using TrocarCorDoPoring.Servicos.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILightsOutSolver, LightsOutSolver>();
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
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
});
#endif

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
