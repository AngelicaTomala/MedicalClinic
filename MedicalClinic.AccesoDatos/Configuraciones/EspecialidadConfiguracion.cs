using MedicalClinic.AccesoDatos.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalClinic.AccesoDatos.Configuraciones
{
    public class EspecialidadConfiguracion : IEntityTypeConfiguration<Especialidad>
    {
        public void Configure(EntityTypeBuilder<Especialidad> builder)
        {
            //Nombre de la tabla
            builder.ToTable("Especialidades");

            //llave primaria
            builder.HasKey(p => p.Id);

            //propiedades y restricciones
            builder.Property(p=>p.Nombre)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(p => p.Descripcion)
                .IsRequired(false)
                .HasMaxLength(300);
        }
    }
}
