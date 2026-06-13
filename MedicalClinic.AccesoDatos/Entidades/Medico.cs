using System.Collections.ObjectModel;

namespace MedicalClinic.AccesoDatos.Entidades
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string TarjetaProfesional { get; set; } = string.Empty;
        public int EspecialidadId { get; set; }

        public Especialidad Especialidad { get; set; } = null!;
        public ICollection<Cita> Citas { get; set; } = null!;
    }
}
