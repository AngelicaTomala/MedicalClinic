using MedicalClinic.AccesoDatos.Entidades;
using MedicalClinic.AccesoDatos.Enum; // Asegúrate de apuntar al namespace correcto de tu EstadoCita
using MedicalClinic.LogicaNegocio.Exceptions;
using MedicalClinic.LogicaNegocio.Interfaces;
using System.Globalization;

namespace MedicalClinic.ConsoleApp.Vistas
{
    public class CitaConsoleView(ICitaService _citaService,
        IPacienteService _pacienteService,
        IMedicoService _medicoService) : MensajeConsoleView
    {
        // ====================================================
        // FORMULARIOS CRUD : Cita Médica
        // ====================================================
        public async Task AgendarCita()
        {
            Console.Clear();
            Console.WriteLine(">>> AGENDAR CITA MÉDICA <<<");

            int pacienteId = await SolicitarYValidarPacienteIdAsync();
            int medicoId = await SolicitarYValidarMedicoIdAsync();

            Console.Write("Fecha y Hora (AAAA-MM-DD HH:MM): ");
            if (!DateTime.TryParseExact(Console.ReadLine(),
                       "yyyy-MM-dd HH:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime fechaHora))
            {
                MostrarMensaje("El formato de fecha y hora ingresado no es válido. Utilice el patrón AAAA-MM-DD HH:MM.", ConsoleColor.Red);
                return;
            }

            Console.Write("Notas iniciales (Opcional): ");
            string notas = Console.ReadLine() ?? "";

            var nuevaCita = new Cita
            {
                PacienteId = pacienteId,
                MedicoId = medicoId,
                FechaHora = fechaHora,
                Notas = string.IsNullOrWhiteSpace(notas) ? null : notas
            };

            try
            {
                await _citaService.AgendarCitasAsync(nuevaCita);
                MostrarMensaje("Cita médica agendada con éxito.", ConsoleColor.Green);
            }
            catch (MostrarMensaje ex)
            {
                MostrarMensaje($"Validación de Negocio: {ex.Message}", ConsoleColor.Yellow);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error interno del sistema: {ex.Message}", ConsoleColor.Red);
            }
        }

        public async Task ListarCitas()
        {
            Console.Clear();
            Console.WriteLine(">>> LISTA DE CITAS REGISTRADAS <<<");

            var citas = await _citaService.ObtenerCitasAsync();
            bool hayRegistros = false;

            foreach (var cita in citas)
            {
                hayRegistros = true;
                Console.WriteLine($"- ID Cita: {cita.Id} | Paciente: {cita.Paciente.Nombre} {cita.Paciente.Apellido} | Medico: {cita.Medico.Nombre} {cita.Medico.Apellido} | Especialidad: {cita.Medico.Especialidad.Nombre} | Fecha: {cita.FechaHora:dd/MM/yyyy HH:mm} | Estado: {cita.Estado} | Notas: {cita.Notas ?? "N/A"}");
            }

            if (!hayRegistros)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No se encontraron registros almacenados en la base de datos.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        public async Task ObtenerCitasPorMedico()
        {
            Console.Clear();
            Console.WriteLine(">>> CONSULTAR HISTORIAL DE CITAS POR MÉDICO <<<");

            int medicoId = await SolicitarYValidarMedicoIdAsync();

            try
            {
                var agenda = await _citaService.ObtenerCitasPorMedicoAsync(medicoId);

                Console.Clear();
                Console.WriteLine($">>> REPORTE DE AGENDA: MÉDICO ID {medicoId} <<<\n");

                //con esto estamos validando que el médico si existe pero que la lista llego vacía
                if (!agenda.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("El médico no registra citas agendadas.");
                    Console.ResetColor();
                }
                else
                {
                    foreach (var c in agenda)
                    {
                        Console.WriteLine($"--------------------------------------------------");
                        Console.WriteLine($"CITA ID:      {c.Id} | Estado: [{c.Estado}]");
                        Console.WriteLine($"Horario:      {c.FechaHora:dd/MM/yyyy HH:mm}");
                        Console.WriteLine($"Paciente:     {c.Paciente.Nombre} {c.Paciente.Apellido}");
                        Console.WriteLine($"Médico:       Dr(a). {c.Medico.Nombre} {c.Medico.Apellido} [{c.Medico.Especialidad.Nombre}]");
                        Console.WriteLine($"--------------------------------------------------");
                    }
                }

                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (MostrarMensaje ex)
            {
                MostrarMensaje($"Validación de Negocio: {ex.Message}", ConsoleColor.Yellow);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error interno del sistema: {ex.Message}", ConsoleColor.Red);
            }
        }

        public async Task ObtenerCitasPorPaciente()
        {
            Console.Clear();
            Console.WriteLine(">>> CONSULTAR HISTORIAL DE CITAS POR PACIENTE <<<");

            string numeroDocumento = "";

            while (true)
            {
                Console.Write("Ingrese número de documento del paciente a consultar las citas: ");
                numeroDocumento = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(numeroDocumento))
                {
                    MostrarMensaje("El número de documento no puede estar vacía.", ConsoleColor.Red);
                    continue;
                }

                try
                {
                    // Verificamos si la especialidad existe
                    await _pacienteService.ObtenerPacientePorNumeroDocumentoAsync(numeroDocumento);
                    break;
                }
                catch (MostrarMensaje ex)
                {
                    MostrarMensaje($"Validación de Negocio: {ex.Message}", ConsoleColor.Yellow);
                }
                catch (Exception ex)
                {
                    MostrarMensaje($"Error interno del sistema: {ex.Message}", ConsoleColor.Red);
                }
            }

            try
            {
                Console.Clear();
                Console.WriteLine($">>> HISTORIAL DE CITAS MÉDICAS <<<\n");

                var historial = await _citaService.ObtenerCitasPorPacienteAsync(numeroDocumento);
                bool tieneCitas = false;

                foreach (var c in historial)
                {
                    tieneCitas = true;
                    Console.WriteLine($"--------------------------------------------------");
                    Console.WriteLine($"Cita N°:   {c.Id} | Horario: {c.FechaHora:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Paciente:  {c.Paciente.Nombre} {c.Paciente.Apellido}");
                    Console.WriteLine($"Atendido:  Dr(a). {c.Medico.Nombre} {c.Medico.Apellido} [{c.Medico.Especialidad.Nombre}]");
                    Console.WriteLine($"Estado:    [{c.Estado}]");
                    Console.WriteLine($"Notas:     {c.Notas ?? "Ninguna."}");
                    Console.WriteLine($"--------------------------------------------------");
                }

                if (!tieneCitas)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("El paciente se encuentra registrado pero no posee historial de citas.");
                    Console.ResetColor();
                }

                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (MostrarMensaje ex)
            {
                MostrarMensaje($"Validación de Negocio: {ex.Message}", ConsoleColor.Yellow);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error interno del sistema: {ex.Message}", ConsoleColor.Red);
            }
        }

        public async Task ActualizarEstadoCita()
        {
            Console.Clear();
            Console.WriteLine(">>> CONTROL Y CIERRE DE CITA <<<");

            int id = await SolicitarYValidarCitaIdAsync();

            Console.WriteLine("\nSeleccione el nuevo estado clínico:");
            Console.WriteLine("1. Completada");
            Console.WriteLine("2. Cancelada");
            Console.Write("Selección: ");
            string op = Console.ReadLine() ?? "";

            if (op != "1" && op != "2")
            {
                MostrarMensaje("Opción de estado inválida.", ConsoleColor.Red);
                return;
            }

            EstadoCita nuevoEstado = op == "1" ? EstadoCita.Completada : EstadoCita.Cancelada;

            Console.Write("Escriba las conclusiones o anotaciones médicas de la cita: ");
            string nuevaNota = Console.ReadLine() ?? "";

            try
            {
                await _citaService.ActualizarEstadoCitaAsync(id, nuevoEstado, nuevaNota);
                MostrarMensaje($"El estado de la cita se ha modificado a [{nuevoEstado}] con éxito.", ConsoleColor.Green);
            }
            catch (MostrarMensaje ex)
            {
                MostrarMensaje($"Validación de Negocio: {ex.Message}", ConsoleColor.Yellow);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error interno del sistema: {ex.Message}", ConsoleColor.Red);
            }
        }

        public async Task ReprogramarFechaCita()
        {
            Console.Clear();
            Console.WriteLine(">>> REPROGRAMAR AGENDA CLÍNICA <<<");

            int id = await SolicitarYValidarCitaIdAsync();

            Console.Write("Ingrese la nueva Fecha y Hora (AAAA-MM-DD HH:MM): ");
            if (!DateTime.TryParseExact(Console.ReadLine(),
                       "yyyy-MM-dd HH:mm",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime nuevaFecha))
            {
                MostrarMensaje("El formato cronológico es incorrecto.", ConsoleColor.Red);
                return;
            }

            Console.Write("Ingrese el motivo de la reprogramación (Opcional): ");
            string nuevaNota = Console.ReadLine() ?? "";

            try
            {
                await _citaService.ActualizarFechaCitaAsync(id, nuevaFecha, nuevaNota);
                MostrarMensaje("La cita médica ha sido reagendada con éxito.", ConsoleColor.Green);
            }
            catch (MostrarMensaje ex)
            {
                MostrarMensaje($"Validación de Negocio: {ex.Message}", ConsoleColor.Yellow);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error interno del sistema: {ex.Message}", ConsoleColor.Red);
            }
        }

        private async Task<int> SolicitarYValidarPacienteIdAsync()
        {
            int pacienteId;
            while (true)
            {
                Console.Write("\nIngrese el id del paciente: ");
                if (!int.TryParse(Console.ReadLine(), out pacienteId))
                {
                    MostrarMensaje("El ID ingresado debe ser un número entero. Intente nuevamente.", ConsoleColor.Red);
                    continue;
                }

                try
                {
                    await _pacienteService.ExistePacienteAsync(pacienteId);
                    return pacienteId; // Corta el bucle y devuelve el ID válido
                }
                catch (MostrarMensaje ex)
                {
                    MostrarMensaje($"Validación de Negocio: {ex.Message}", ConsoleColor.Yellow);
                }
                catch (Exception ex)
                {
                    MostrarMensaje($"Error interno del sistema: {ex.Message}", ConsoleColor.Red);
                }
            }
        }

        private async Task<int> SolicitarYValidarMedicoIdAsync()
        {
            int especialidadId;
            while (true)
            {
                Console.Write("\nIngrese el Id del médico: ");
                if (!int.TryParse(Console.ReadLine(), out especialidadId))
                {
                    MostrarMensaje("El ID ingresado debe ser un número entero. Intente nuevamente.", ConsoleColor.Red);
                    continue;
                }

                try
                {
                    await _medicoService.ExisteMedicoAsync(especialidadId);
                    return especialidadId; // Corta el bucle y devuelve el ID válido
                }
                catch (MostrarMensaje ex)
                {
                    MostrarMensaje($"Validación de Negocio: {ex.Message}", ConsoleColor.Yellow);
                }
                catch (Exception ex)
                {
                    MostrarMensaje($"Error interno del sistema: {ex.Message}", ConsoleColor.Red);
                }
            }
        }

        private async Task<int> SolicitarYValidarCitaIdAsync()
        {
            int citaId;
            while (true)
            {
                Console.Write("\nIngrese el Id de la cita: ");
                if (!int.TryParse(Console.ReadLine(), out citaId))
                {
                    MostrarMensaje("El ID ingresado debe ser un número entero. Intente nuevamente.", ConsoleColor.Red);
                    continue;
                }

                try
                {
                    await _citaService.ExisteCitaAsync(citaId);
                    return citaId;
                }
                catch (MostrarMensaje ex)
                {
                    MostrarMensaje($"Validación de Negocio: {ex.Message}", ConsoleColor.Yellow);
                }
                catch (Exception ex)
                {
                    MostrarMensaje($"Error interno del sistema: {ex.Message}", ConsoleColor.Red);
                }
            }
        }
    }
}