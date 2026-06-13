using MedicalClinic.AccesoDatos.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalClinic.AccesoDatos.Configuraciones
{
    public class CitaConfiguracion : IEntityTypeConfiguration<Cita>
    {
        public void Configure(EntityTypeBuilder<Cita> builder)
        {
            //Nombre de la tabla
            builder.ToTable("Citas");

            //Llave primaria
            builder.HasKey(p => p.Id);

            //Propiedades y restricciones
            builder.Property(c => c.FechaHora)
            .HasColumnType("datetime2")
            .IsRequired();

            builder.Property(p => p.Estado)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(p => p.Notas)
                .IsRequired(false)
                .HasMaxLength(250);

            //relacion de 1 a muchos de Paciente -> citas
            builder.HasOne(p => p.Paciente)
                .WithMany(p=>p.Citas)
                .HasForeignKey(p => p.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            //relacion de 1 a muchos de Medico -> citas
            builder.HasOne(p => p.Medico)
                .WithMany(p => p.Citas)
                .HasForeignKey(p => p.MedicoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}