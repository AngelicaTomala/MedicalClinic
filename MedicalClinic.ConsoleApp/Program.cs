using MedicalClinic.ConsoleApp.InyeccionDependencia;
using MedicalClinic.LogicaNegocio.InyeccionDepedencia;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalClinic.ConsoleApp;

class Program
{
    static async Task Main(string[] args)
    {
        //inicializa el contenedor de las dependencias
        var serviceProvider = ConfigurarServicios();

        //resuelve en cascada las dependencias
        var menu = serviceProvider.GetRequiredService<MenuPrincipal>();
        await menu.MostrarMenuAsync();
    }

    /// <summary>
    /// centraliza el registro de todas dependencias de la aplicación
    /// </summary>
    /// <returns></returns>
    private static IServiceProvider ConfigurarServicios()
    {
        var services = new ServiceCollection();

        // añadimos los servicios a través de la capa lógica de negocio
        services.AddService();

        // Registramos los componentes visuales y la UI de la consola
        services.AddPresentationViews();

        return services.BuildServiceProvider();
    }
}
