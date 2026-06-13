using MedicalClinic.AccesoDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalClinic.LogicaNegocio.Interfaces
{
    public interface IMedicoService
    {
        Task RegistrarMedicoAsync(Medico medico);
        Task<IEnumerable<Medico>> ObtenerMedicosAsync();
        Task<bool> ExisteMedicoAsync(int id);
        Task<Medico> ObtenerMedicosPorTarjetaProfesionalAsync(string numeroTarjetaProfesional);
        Task ActualizarMedicoAsync(Medico medico);
        Task EliminarMedicoAsync(int id);
    }
}
