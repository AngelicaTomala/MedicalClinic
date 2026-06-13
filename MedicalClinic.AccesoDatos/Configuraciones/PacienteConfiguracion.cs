using MedicalClinic.AccesoDatos.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalClinic.AccesoDatos.Configuraciones
{
    public class PacienteConfiguracion : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder)
        {
            // Nombre de la tabla
            builder.ToTable("Pacientes");

            // Llave primaria
            builder.HasKey(p => p.Id);

            // Propiedades y restricciones
            builder.Property(p => p.DocumentoId)
                .HasMaxLength(13)
                .IsRequired();

            // Crear un índice único para el documento
            builder.HasIndex(p => p.DocumentoId)
                .IsUnique();

            builder.Property(p => p.Nombre)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Apellido)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.FechaNacimiento)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(p => p.Telefono)
                .HasMaxLength(15)
                .IsRequired(false); // Opcional
        }
    }
}
