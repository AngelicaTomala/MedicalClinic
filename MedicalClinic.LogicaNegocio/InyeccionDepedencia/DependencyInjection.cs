using MedicalClinic.AccesoDatos.InyeccionDepedencia;
using MedicalClinic.LogicaNegocio.Interfaces;
using MedicalClinic.LogicaNegocio.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalClinic.LogicaNegocio.InyeccionDepedencia
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            // 1. Registra los servicios propios de esta capa
            services.AddScoped<IEspecialidadService, EspecialidadService>();
            services.AddScoped<IMedicoService, MedicoService>();
            services.AddScoped<IPacienteService, PacienteService>();
            services.AddScoped<ICitaService, CitaService>();

            // 2. Llama al registro de la capa inferior (AccesoDatos)
            services.AddAccesoDatos();

            return services;
        }
    }
}
