using MedicalClinic.ConsoleApp.Vistas;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalClinic.ConsoleApp.InyeccionDependencia;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationViews(this IServiceCollection services)
    {
        // 1. La misma capa de presentación registra sus componentes visuales de forma interna
        services.AddScoped<EspecialidadConsoleView>();
        services.AddScoped<MedicoConsoleView>();
        services.AddScoped<PacienteConsoleView>();
        services.AddScoped<CitaConsoleView>();

        // 2. Registra el menú principal
        services.AddScoped<MenuPrincipal>();

        return services;
    }
}
