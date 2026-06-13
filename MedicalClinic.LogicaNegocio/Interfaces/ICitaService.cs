using MedicalClinic.AccesoDatos.Entidades;
using MedicalClinic.AccesoDatos.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalClinic.LogicaNegocio.Interfaces
{
    public interface ICitaService
    {
        Task AgendarCitasAsync(Cita cita);
        Task<IEnumerable<Cita>> ObtenerCitasAsync();
        Task<bool> ExisteCitaAsync(int id);
        Task<IEnumerable<Cita>> ObtenerCitasPorMedicoAsync(int medicoId);
        Task<IEnumerable<Cita>> ObtenerCitasPorPacienteAsync(string numeroDocumento);
        Task ActualizarEstadoCitaAsync(int id, EstadoCita nuevoEstado, string nuevaNota);        
        Task ActualizarFechaCitaAsync(int id, DateTime nuevoFechaHora, string nuevaNota);
    }
}
