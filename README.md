# 🏥 MedicalClinic - Sistema de Consola

**MedicalClinic** es una aplicación de consola desarrollada para la gestión interna de una clínica médica. 
El sistema permite administrar de forma rápida y estructurada los datos de pacientes, médicos, especialidad, citas.


## 🎯 Características Principales

*   **Gestión de Especialidad**: Registro, consulta, actualización y baja.
*   **Gestión de Medicos**: Registro, consulta, actualización y baja.
*   **Gestión de Pacientes**: Registro, consulta, actualización y baja.
*   **Gestión de Citas**: Registro, consulta, cambiar estado (pendiente -> finalizado), reprograma cita..

## 🎮 Flujo del Menú Interactivo

Al arrancar, la aplicación despliega un menú numérico principal con validación de opciones:

=========================================
      SISTEMA MEDICAL CLINIC v1.0        
=========================================
1. Gestión de Especialidades
2. Gestión de Médicos
3. Gestión de Pacientes
4. Agendamiento de Citas Médicas
5. Salir
=========================================
Seleccione una opción: 

Al seleccionar una opción mostrará un submenú que incluye las operaciones CRUD correspondientes.

## 🛠️ Tecnologías Utilizadas

*   **Lenguaje**: C# / .NET 10
*   **Arquitectura**: Aplicación de Consola estructurada en capas (Presentación, Lógica de Negocio, Acceso a Datos).
*   **Persistencia**: Entity Framework Core.
