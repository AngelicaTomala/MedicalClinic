using MedicalClinic.AccesoDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalClinic.LogicaNegocio.Interfaces
{
    public interface IEspecialidadService
    {
        Task CrearEspecialidadAsync(Especialidad especialidad);
        Task<IEnumerable<Especialidad>> ObtenerEspecialidadesAsync();
        Task<Especialidad> ObtenerEspecialidadPorIdAsync(int id);
        Task ActualizarEspecialidadAsync(Especialidad especialidad);
        Task EliminarEspecialidadAsync(int id);
    }
}
