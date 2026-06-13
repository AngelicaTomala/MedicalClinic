using MedicalClinic.AccesoDatos.Contexto;
using MedicalClinic.AccesoDatos.Entidades;
using MedicalClinic.AccesoDatos.Enum;
using MedicalClinic.LogicaNegocio.Exceptions;
using MedicalClinic.LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.LogicaNegocio.Services
{
    public class CitaService(ClinicalContext _context) : ICitaService
    {
        public async Task AgendarCitasAsync(Cita cita)
        {
            // Validación de existencia de entidades y cruce de horarios antes de insertar
            await ValidarExistenciaYHorarioAsync(cita.MedicoId, cita.PacienteId, cita.FechaHora, cita.Id);

            //forzamos que el registro inicie con estado pendiente
            cita.Estado = EstadoCita.Pendiente;

            await _context.Citas.AddAsync(cita);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Cita>> ObtenerCitasAsync()
        {
            var listadoCitas = await _context.Citas
                .Include(m => m.Paciente)
                .Include(m => m.Medico)
                .ThenInclude(m => m.Especialidad)
                .ToListAsync();
            return listadoCitas;
        }

        public async Task<bool> ExisteCitaAsync(int id)
        {
            var existeCita = await _context.Citas.AnyAsync(c => c.Id == id);

            if (!existeCita)
                throw new MostrarMensaje($"La cita no está registrado en el sistema.");

            return existeCita;
        }

        public async Task<IEnumerable<Cita>> ObtenerCitasPorMedicoAsync(int medicoId)
        {
            //Regla de negocio 1: Verificamos en la tabla de Médicos si el ID es real
            var existeMedico = await _context.Medicos.AnyAsync(m => m.Id == medicoId);
            if (!existeMedico)
                throw new MostrarMensaje($"El médico con el ID {medicoId} no está registrado en el sistema.");

            // Regla de negocio 2: Si el médico existe, procedemos de forma segura a traer su agenda
            var medicoEntidad = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ThenInclude(c => c.Especialidad)
                .Where(c => c.MedicoId == medicoId)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();

            return medicoEntidad;

        }
        public async Task<IEnumerable<Cita>> ObtenerCitasPorPacienteAsync(string numeroDocumento)
        {
            //Regla de negocio 1: Verificamos en la tabla paciente si el Id es real
            var pacienteEntidad = await _context.Pacientes.FirstOrDefaultAsync(c => c.DocumentoId == numeroDocumento) ??
                throw new MostrarMensaje($"El paciente con número de documento {numeroDocumento} no está registrado en el sistema.");

            // Regla de negocio 2: Si el paciente existe, procedemos de forma segura a traer su citas agendadas
            var paciente = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ThenInclude(c => c.Especialidad)
                .Where(c => c.PacienteId == pacienteEntidad.Id)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();

            return paciente;
        }

        public async Task ActualizarEstadoCitaAsync(int id, EstadoCita nuevoEstado, string nuevaNota)
        {
            var citaEntidad = await _context.Citas.FindAsync(id) ??
                throw new MostrarMensaje("La cita no existe");

            citaEntidad.Estado = nuevoEstado;
            citaEntidad.Notas = nuevaNota;

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarFechaCitaAsync(int id, DateTime nuevoFechaHora, string nuevaNota)
        {
            var citaEntidad = await _context.Citas.FindAsync(id) ??
                throw new MostrarMensaje("La cita no existe");

            if (citaEntidad.Estado != EstadoCita.Pendiente)
                throw new MostrarMensaje("Solo se pueden reprogramar citas que se encuentren Pendientes");

            await ValidarExistenciaYHorarioAsync(citaEntidad.MedicoId, citaEntidad.PacienteId, nuevoFechaHora, id);

            citaEntidad.FechaHora = nuevoFechaHora;
            await _context.SaveChangesAsync();
        }

        private async Task ValidarExistenciaYHorarioAsync(int medicoId, int pacienteId, DateTime fechaHora, int citaIdActual)
        {
            // Regla de negocio: Bloquear registros pasados
            if (fechaHora < DateTime.Now)
                throw new MostrarMensaje("No se pueden programar ni trasladar citas a fechas u horas pasadas.");

            //Regla de negocio: Validar que el paciente exista
            var pacExiste = await _context.Pacientes.AnyAsync(p => p.Id == pacienteId);
            if (!pacExiste)
                throw new MostrarMensaje("El paciente asignado no se encuentra registrado.");

            //Regla de negocio: Validar que el medico exista
            var medExiste = await _context.Medicos.AnyAsync(m => m.Id == medicoId);
            if (!medExiste)
                throw new MostrarMensaje("El médico asignado no se encuentra registrado.");

            // Regla de negocio : Evitar el cruce de horarios para el mismo médico.
            // Establecemos un rango de protección estándar de 30 minutos por cita médica.
            DateTime inicioRango = fechaHora.AddMinutes(-29);
            DateTime finRango = fechaHora.AddMinutes(29);

            // Hallamos la primera coincidencia
            var medicoOcupado = await _context.Citas.AnyAsync(c =>
                c.MedicoId == medicoId &&
                c.Id != citaIdActual && // Excluye la misma cita en caso de estar reprogramándola
                c.Estado != EstadoCita.Cancelada &&
                c.FechaHora >= inicioRango &&
                c.FechaHora <= finRango);

            if (medicoOcupado)
                throw new MostrarMensaje("El médico ya tiene cita agendada en un rango horario cercano.");
        }
    }
}
