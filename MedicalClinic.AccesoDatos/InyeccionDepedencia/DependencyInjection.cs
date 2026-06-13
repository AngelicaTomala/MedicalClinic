using MedicalClinic.AccesoDatos.Contexto;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalClinic.AccesoDatos.InyeccionDepedencia
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccesoDatos(this IServiceCollection services)
        {
            // La capa de datos se registra a sí misma
            services.AddDbContext<ClinicalContext>();
            return services;
        }
    }
}
