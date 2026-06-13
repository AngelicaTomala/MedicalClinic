using System.Collections.ObjectModel;

namespace MedicalClinic.AccesoDatos.Entidades
{
    public class Especialidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; } = string.Empty;

        public ICollection<Medico> Medicos { get; set; } = null!;
    }
}
