using MedicalClinic.AccesoDatos.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalClinic.AccesoDatos.Entidades
{
    public class Cita
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public DateTime FechaHora { get; set; }
        public EstadoCita Estado { get; set; }
        public string? Notas { get; set; }

        public Medico Medico { get; set; } = null!;
        public Paciente Paciente { get; set; } = null!;

    }
}
