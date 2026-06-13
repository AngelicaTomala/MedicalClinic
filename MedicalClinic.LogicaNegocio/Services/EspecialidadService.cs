using MedicalClinic.AccesoDatos.Contexto;
using MedicalClinic.AccesoDatos.Entidades;
using MedicalClinic.LogicaNegocio.Exceptions;
using MedicalClinic.LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.LogicaNegocio.Services
{
    public class EspecialidadService(ClinicalContext _context) : IEspecialidadService
    {
        public async Task CrearEspecialidadAsync(Especialidad especialidad)
        {
            // Regla de negocio: No permitir nombres duplicados
            var existe = await _context.Especialidades
                .AnyAsync(e => e.Nombre.Trim().ToUpper() == especialidad.Nombre.Trim().ToUpper());

            if (existe)
                throw new MostrarMensaje($"La especialidad '{especialidad.Nombre}' ya se encuentra registrada.");

            await _context.Especialidades.AddAsync(especialidad);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Especialidad>> ObtenerEspecialidadesAsync()
        {
            var ListaEspecialidades = await _context.Especialidades.ToListAsync();

            return ListaEspecialidades;
        }

        public async Task<Especialidad> ObtenerEspecialidadPorIdAsync(int id)
        {
            var especialidad = await _context.Especialidades.FindAsync(id) ??
                throw new MostrarMensaje($"La especialidad con el id {id} no existe.");

            return especialidad;
        }

        public async Task ActualizarEspecialidadAsync(Especialidad especialidad)
        {
            //realiza la búsqueda de un registro especifico
            var especialidadEntidad = await _context.Especialidades.FindAsync(especialidad.Id) ??
                throw new MostrarMensaje($"La especialidad con el id {especialidad.Id} no existe.");

            var duplicado = await _context.Especialidades.AnyAsync(e => e.Nombre.Trim().ToUpper() == especialidad.Nombre.Trim().ToUpper() && e.Id != especialidad.Id);
            if (duplicado)
                throw new MostrarMensaje($"Ya existe otra especialidad con el nombre '{especialidad.Nombre}'.");

            especialidadEntidad.Nombre = especialidad.Nombre;
            especialidadEntidad.Descripcion = especialidad.Descripcion;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarEspecialidadAsync(int id)
        {
            //busca el primer registro que coincida con el id e incluye a la tabla medico
            var especialidadEntidad = await _context.Especialidades.Include(e => e.Medicos).FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new MostrarMensaje("La especialidad no existe.");

            //Evalúa si alguna de las especialidades esta atado a un medico
            if (especialidadEntidad.Medicos.Any())
                throw new MostrarMensaje("No se puede eliminar la especialidad porque tiene médicos asociados.");

            _context.Especialidades.Remove(especialidadEntidad);
            await _context.SaveChangesAsync();
        }
    }
}
