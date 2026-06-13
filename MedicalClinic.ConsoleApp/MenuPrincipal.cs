using MedicalClinic.AccesoDatos.Entidades;
using MedicalClinic.ConsoleApp.Vistas;
using MedicalClinic.LogicaNegocio.Exceptions;
using MedicalClinic.LogicaNegocio.Interfaces;

namespace MedicalClinic.ConsoleApp;

public class MenuPrincipal : MensajeConsoleView
{
    private readonly EspecialidadConsoleView _especialidadView;
    private readonly MedicoConsoleView _medicoView;
    private readonly PacienteConsoleView _pacienteView;
    private readonly CitaConsoleView _citaView;

    // Las dependencias se inyectan desde el Program.cs
    public MenuPrincipal(EspecialidadConsoleView especialidadView, MedicoConsoleView medicoView,
        PacienteConsoleView pacienteView, CitaConsoleView citaView)
    {
        _especialidadView = especialidadView;
        _medicoView = medicoView;
        _pacienteView = pacienteView;
        _citaView = citaView;
    }

    public async Task MostrarMenuAsync()
    {
        bool salir = false;

        while (!salir)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.WriteLine("        SISTEMA DE GESTIÓN MÉDICA       ");
            Console.WriteLine("========================================");
            Console.ResetColor();
            Console.WriteLine("1. Módulo de Especialidades");
            Console.WriteLine("2. Módulo de Medicos");
            Console.WriteLine("3. Módulo de Pacientes");
            Console.WriteLine("4. Módulo de Citas");
            Console.WriteLine("5. Salir");
            Console.WriteLine("----------------------------------------");
            Console.Write("Seleccione una opción: ");

            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    await MostrarSubmenuEspecialidadAsync();
                    break;
                case "2":
                    await MostrarSubmenuMedicoAsync();
                    break;
                case "3":
                    await MostrarSubmenuPacienteAsync();
                    break;
                case "4":
                    await MostrarSubmenuCitaAsync();
                    break;
                case "5":
                    salir = true;
                    Console.WriteLine("\n¡Gracias por utilizar el sistema!");
                    break;
                default:
                    MostrarMensaje("Opción no válida. Intente de nuevo.", ConsoleColor.Red);
                    break;
            }
        }
    }

    // ==========================================
    // SUBMENÚ: ESPECIALIDADES
    // ==========================================
    public async Task MostrarSubmenuEspecialidadAsync()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.WriteLine("        MÓDULO DE ESPECIALIDADES        ");
            Console.WriteLine("========================================");
            Console.ResetColor();
            Console.WriteLine("1. Registrar una nueva Especialidad");
            Console.WriteLine("2. Listar todas las Especialidades");
            Console.WriteLine("3. Buscar Especialidad por Id");
            Console.WriteLine("4. Modificar Especialidad");
            Console.WriteLine("5. Eliminar Especialidad");
            Console.WriteLine("6. Volver al menú principal");
            Console.WriteLine("----------------------------------------");
            Console.Write("Seleccione una opción: ");

            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    await _especialidadView.CrearEspecialidad();
                    break;
                case "2":
                    await _especialidadView.ListarEspecialidades();
                    break;
                case "3":
                    await _especialidadView.BuscarEspecialidadPorId();
                    break;
                case "4":
                    await _especialidadView.ActualizarEspecialidad();
                    break;
                case "5":
                    await _especialidadView.EliminarEspecialidad();
                    break;
                case "6":
                    volver = true;                    
                    break;
                default:
                    MostrarMensaje("Opción no válida. Intente de nuevo.", ConsoleColor.Red);
                    break;
            }
        }
    }

    // ==========================================
    // SUBMENÚ: MEDICO
    // ==========================================
    public async Task MostrarSubmenuMedicoAsync()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.WriteLine("            MÓDULO DE MÉDICOS           ");
            Console.WriteLine("========================================");
            Console.ResetColor();
            Console.WriteLine("1. Registrar un nuevo Médico");
            Console.WriteLine("2. Listar todos los Médicos");
            Console.WriteLine("3. Buscar Medico por Tarjeta profesional");
            Console.WriteLine("4. Modificar Médico");
            Console.WriteLine("5. Eliminar Médico");
            Console.WriteLine("6. Volver al menú principal");
            Console.WriteLine("----------------------------------------");
            Console.Write("Seleccione una opción: ");

            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    await _medicoView.CrearMedico();
                    break;
                case "2":
                    await _medicoView.ObtenerMedicosAsync();
                    break;
                case "3":
                    await _medicoView.ObtenerMedicosPorTarjetaProfesionalAsync();
                    break;
                case "4":
                    await _medicoView.ActualizarMedicoAsync();
                    break;
                case "5":
                    await _medicoView.EliminarMedicoAsync();
                    break;
                case "6":
                    volver = true;
                    break;
                default:
                    MostrarMensaje("Opción no válida. Intente de nuevo.", ConsoleColor.Red);
                    break;
            }
        }
    }

    // ==========================================
    // SUBMENÚ: PACIENTE
    // ==========================================
    public async Task MostrarSubmenuPacienteAsync()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.WriteLine("           MÓDULO DE PACIENTES          ");
            Console.WriteLine("========================================");
            Console.ResetColor();
            Console.WriteLine("1. Registrar un nuevo Paciente");
            Console.WriteLine("2. Listar todos los Pacientes");
            Console.WriteLine("3. Buscar Paciente por Número de Documento");
            Console.WriteLine("4. Modificar Paciente");
            Console.WriteLine("5. Eliminar Paciente");
            Console.WriteLine("6. Volver al menú principal");
            Console.WriteLine("----------------------------------------");
            Console.Write("Seleccione una opción: ");

            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    await _pacienteView.RegistrarPaciente();
                    break;
                case "2":
                    await _pacienteView.ListarPacientes();
                    break;
                case "3":
                    await _pacienteView.ObtenerPacientesPorNumeroDocumento();
                    break;
                case "4":
                    await _pacienteView.ActualizarPaciente();
                    break;
                case "5":
                    await _pacienteView.EliminarPaciente();
                    break;
                case "6":
                    volver = true;
                    break;
                default:
                    MostrarMensaje("Opción no válida. Intente de nuevo.", ConsoleColor.Red);
                    break;
            }
        }
    }

    // ==========================================
    // SUBMENÚ: CITAS
    // ==========================================
    public async Task MostrarSubmenuCitaAsync()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.WriteLine("       MÓDULO DE CITAS MÉDICAS          ");
            Console.WriteLine("========================================");
            Console.ResetColor();
            Console.WriteLine("1. Agendar Cita Médica");
            Console.WriteLine("2. Listar todas las Citas Médicas");
            Console.WriteLine("3. Buscar Citas por Médico");
            Console.WriteLine("4. Buscar Citas por Paciente");
            Console.WriteLine("5. Cierre de Cita Médica");
            Console.WriteLine("6. Reprogramar Cita Médica");
            Console.WriteLine("7. Volver al menú principal");
            Console.WriteLine("----------------------------------------");
            Console.Write("Seleccione una opción: ");

            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    await _citaView.AgendarCita();
                    break;
                case "2":
                    await _citaView.ListarCitas();
                    break;
                case "3":
                    await _citaView.ObtenerCitasPorMedico();
                    break;
                case "4":
                    await _citaView.ObtenerCitasPorPaciente();
                    break;
                case "5":
                    await _citaView.ActualizarEstadoCita();
                    break;
                case "6":
                    await _citaView.ReprogramarFechaCita();
                    break;
                case "7":
                    volver = true;
                    break;
                default:
                    MostrarMensaje("Opción no válida. Intente de nuevo.", ConsoleColor.Red);
                    break;
            }
        }
    }
}

