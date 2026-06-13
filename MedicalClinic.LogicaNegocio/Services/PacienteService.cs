using MedicalClinic.AccesoDatos.Contexto;
using MedicalClinic.AccesoDatos.Entidades;
using MedicalClinic.LogicaNegocio.Exceptions;
using MedicalClinic.LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.LogicaNegocio.Services
{
    public class PacienteService(ClinicalContext _context) : IPacienteService
    {
        public async Task RegistrarPacienteAsync(Paciente paciente)
        {
            if (paciente.FechaNacimiento > DateTime.Today)
                throw new MostrarMensaje("La fecha de nacimiento no puede ser una fecha futura.");

            //Regla de negocio: no permitir numero de documento duplicado
            var numeroDocumentoDuplicado = await _context.Pacientes.AnyAsync(c => c.DocumentoId.Trim().ToUpper() == paciente.DocumentoId.Trim().ToUpper());

            if (numeroDocumentoDuplicado)
                throw new MostrarMensaje($"El número de documento ingresado ya existe.");

            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();

        }
        public async Task<IEnumerable<Paciente>> ObtenerPacientesAsync()
        {
            var listadoPaciente = await _context.Pacientes.ToListAsync();
            return listadoPaciente;
        }

        public async Task<bool> ExistePacienteAsync(int id)
        {
            var existePaciente = await _context.Pacientes.AnyAsync(c => c.Id == id);

            if (!existePaciente)
                throw new MostrarMensaje($"El paciente no se encuentra registrado en el sistema.");

            return existePaciente;
        }

        public async Task<Paciente> ObtenerPacientePorNumeroDocumentoAsync(string numeroDocumento)
        {
            var pacienteEntidad = await _context.Pacientes.FirstOrDefaultAsync(c => c.DocumentoId.Trim().ToLower() == numeroDocumento.Trim().ToLower()) ??
                throw new MostrarMensaje($"No existe paciente con el id ingresado");

            return pacienteEntidad;

        }
        public async Task ActualizarPacienteAsync(Paciente paciente)
        {
            var pacienteEntidad = await _context.Pacientes.FindAsync(paciente.Id) ??
                throw new MostrarMensaje($"No existe el paciente");

            var numeroDocumentoDuplicado = await _context.Pacientes.AnyAsync(c => c.DocumentoId.Trim().ToUpper() == paciente.DocumentoId.Trim().ToUpper() && c.Id != paciente.Id);

            if (numeroDocumentoDuplicado)
                throw new MostrarMensaje($"El numero de documento ya existe");

            pacienteEntidad.Nombre = paciente.Nombre;
            pacienteEntidad.Apellido = paciente.Apellido;
            pacienteEntidad.DocumentoId = paciente.DocumentoId;
            pacienteEntidad.Telefono = paciente.Telefono;
            pacienteEntidad.FechaNacimiento = paciente.FechaNacimiento;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarPacienteAsync(int id)
        {
            var pacienteEntidad = await _context.Pacientes.Include(c => c.Citas).FirstOrDefaultAsync(c => c.Id == id) ??
                throw new MostrarMensaje($"El paciente no existe");

            if (pacienteEntidad.Citas.Any())
                throw new MostrarMensaje("No se puede eliminar el paciente porque tiene citas médicas asociadas.");

            _context.Pacientes.Remove(pacienteEntidad);
            await _context.SaveChangesAsync();
        }




    }
}
