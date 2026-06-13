using MedicalClinic.AccesoDatos.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalClinic.AccesoDatos.Configuraciones
{
    public class MedicoConfiguracion : IEntityTypeConfiguration<Medico>
    {
        public void Configure(EntityTypeBuilder<Medico> builder)
        {
            //Nombre de la tabla
            builder.ToTable("Medicos");

            //llave primaria
            builder.HasKey(x => x.Id);

            //Propiedades y restricciones
            builder.Property(p=>p.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Apellido)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.TarjetaProfesional)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasIndex(x => x.TarjetaProfesional)
                .IsUnique();

            //relacion de uno a muchos de especialidad a medico
            builder.HasOne(x => x.Especialidad)
                .WithMany(x => x.Medicos)
                .HasForeignKey(x => x.EspecialidadId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
