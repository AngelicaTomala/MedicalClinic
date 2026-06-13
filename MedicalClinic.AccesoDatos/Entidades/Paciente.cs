using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MedicalClinic.AccesoDatos.Entidades
{
    public class Paciente
    {
        public int Id { get; set; }
        public string DocumentoId { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; } = string.Empty;

        public ICollection<Cita> Citas { get; set; } = null!;
    }
}
