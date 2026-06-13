using MedicalClinic.AccesoDatos.Contexto;
using MedicalClinic.AccesoDatos.Entidades;
using MedicalClinic.LogicaNegocio.Exceptions;
using MedicalClinic.LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.LogicaNegocio.Services
{
    public class MedicoService(ClinicalContext _context) : IMedicoService
    {
        public async Task RegistrarMedicoAsync(Medico medico)
        {
            //Regla de Negocio: no permitir tarjeta profesional duplicado
            var existe = await _context.Medicos
                .AnyAsync(e => e.TarjetaProfesional.Trim().ToUpper() == medico.TarjetaProfesional.Trim().ToUpper());

            if (existe)
                throw new MostrarMensaje($"La tarjeta profesional con número {medico.TarjetaProfesional} ya se encuentra registrada.");

            await _context.Medicos.AddAsync(medico);

            await _context.SaveChangesAsync();

        }
        public async Task<IEnumerable<Medico>> ObtenerMedicosAsync()
        {
            var listaMedicos = await _context.Medicos
                .Include(m => m.Especialidad)
                .ToListAsync();
            return listaMedicos;
        }

        public async Task<bool> ExisteMedicoAsync(int id)
        {
            var existeMedico = await _context.Medicos.AnyAsync(c => c.Id == id);

            if (!existeMedico)
                throw new MostrarMensaje($"El médico no se encuentra registrado en el sistema.");

            return existeMedico;
        }

        public async Task<Medico> ObtenerMedicosPorTarjetaProfesionalAsync(string numeroTarjetaProfesional)
        {
            var medico = await _context.Medicos.FirstOrDefaultAsync(c => c.TarjetaProfesional.Trim().ToLower() == numeroTarjetaProfesional.Trim().ToLower()) ??
                throw new MostrarMensaje($"No existe médico para la tarjeta profesional ingresada : {numeroTarjetaProfesional}");

            return medico;
        }

        public async Task ActualizarMedicoAsync(Medico medico)
        {
            var medicoEntidad = await _context.Medicos.FindAsync(medico.Id) ??
                throw new MostrarMensaje($"El médico con el id {medico.Id} no existe");

            var tarjetaProfesionalDuplicado = await _context.Medicos.AnyAsync(c => c.TarjetaProfesional.Trim().ToUpper() == medico.TarjetaProfesional.Trim().ToUpper() && c.Id != medico.Id);

            if (tarjetaProfesionalDuplicado)
                throw new MostrarMensaje($"Ya existe un médico registrado con el número de tarjeta profesional ingresado.");

            medicoEntidad.Nombre = medico.Nombre;
            medicoEntidad.Apellido = medico.Apellido;
            medicoEntidad.TarjetaProfesional = medico.TarjetaProfesional;
            medicoEntidad.EspecialidadId = medico.EspecialidadId;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarMedicoAsync(int id)
        {
            var medicoEntidad = await _context.Medicos.Include(m => m.Citas).FirstOrDefaultAsync(m => m.Id == id) ??
                    throw new MostrarMensaje("El médico no existe");

            if (medicoEntidad.Citas.Any(c => c.Estado != AccesoDatos.Enum.EstadoCita.Cancelada))
                throw new MostrarMensaje("No se puede eliminar el médico porque tiene cita agendadas");

            _context.Medicos.Remove(medicoEntidad);
            await _context.SaveChangesAsync();
        }
    }
}
