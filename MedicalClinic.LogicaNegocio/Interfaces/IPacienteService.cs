using MedicalClinic.AccesoDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalClinic.LogicaNegocio.Interfaces
{
    public interface IPacienteService
    {
        Task RegistrarPacienteAsync(Paciente paciente);
        Task<IEnumerable<Paciente>> ObtenerPacientesAsync();
        Task<bool> ExistePacienteAsync(int id);
        Task<Paciente> ObtenerPacientePorNumeroDocumentoAsync(string numeroDocumento);
        Task ActualizarPacienteAsync(Paciente paciente);
        Task EliminarPacienteAsync(int id);
    }
}
