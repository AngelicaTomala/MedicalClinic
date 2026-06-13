using MedicalClinic.AccesoDatos.Entidades;
using MedicalClinic.LogicaNegocio.Exceptions;
using MedicalClinic.LogicaNegocio.Interfaces;
using System.Globalization;


namespace MedicalClinic.ConsoleApp.Vistas
{
    public class PacienteConsoleView(IPacienteService _pacienteService) : MensajeConsoleView
    {
        // ====================================================
        // FORMULARIOS CRUD : Paciente
        // ====================================================
        public async Task RegistrarPaciente()
        {
            Console.Clear();
            Console.WriteLine(">>> REGISTRAR PACIENTE <<<");

            Console.Write("Nombre :");
            string nombre = Console.ReadLine() ?? "";

            Console.Write("Apellido :");
            string apellido = Console.ReadLine() ?? "";

            Console.Write("Número de Documento :");
            string numeroDocumento = Console.ReadLine() ?? "";

            Console.Write("Fecha de Nacimiento (AAAA-MM-DD) :");
            if (!DateTime.TryParseExact(Console.ReadLine(),
                       "yyyy-MM-dd",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime fechaNacimiento))
            {
                MostrarMensaje("El formato de fecha ingresado no es válido. Utilice el patrón AAAA-MM-DD.", ConsoleColor.Red);
                return;
            }

            Console.Write("Teléfono :");
            string telefono = Console.ReadLine() ?? "";

            var nuevoPaciente = new Paciente
            {
                Nombre = nombre,
                Apellido = apellido,
                DocumentoId = numeroDocumento,
                FechaNacimiento = fechaNacimiento,
                Telefono = telefono
            };

            try
            {
                await _pacienteService.RegistrarPacienteAsync(nuevoPaciente);
                MostrarMensaje($"Paciente: {nombre} {apellido} registrado con éxito.", ConsoleColor.Green);
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

        public async Task ListarPacientes()
        {
            Console.Clear();
            Console.WriteLine(">>> LISTAS DE PACIENTES REGISTRADOS <<<");

            var pacientes = await _pacienteService.ObtenerPacientesAsync();

            bool hayRegistros = false;

            foreach (var paciente in pacientes)
            {
                hayRegistros = true;
                Console.WriteLine($"- ID: {paciente.Id} | Nombre: {paciente.Nombre} | Apellido: {paciente.Apellido} | Número de Documento: {paciente.DocumentoId} | Teléfono: {paciente.Telefono} ");
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

        public async Task ObtenerPacientesPorNumeroDocumento()
        {
            Console.Clear();
            Console.WriteLine(">>> BUSCAR PACIENTE POR NUMERO DE DOCUMENTO <<<");

            Paciente? pacienteEncontrado = null;

            while (true)
            {
                Console.Write("Ingrese número de documento  a buscar: ");
                string numeroDocumento = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(numeroDocumento))
                {
                    MostrarMensaje("El número de documento no puede estar vacía.", ConsoleColor.Red);
                    continue;
                }

                try
                {
                    // Verificamos si la especialidad existe
                    pacienteEncontrado = await _pacienteService.ObtenerPacientePorNumeroDocumentoAsync(numeroDocumento);
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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[Registro Encontrado]");
            Console.ResetColor();
            Console.WriteLine($"----------------------------------------");
            Console.WriteLine($"ID:          {pacienteEncontrado.Id}");
            Console.WriteLine($"Nombre:      {pacienteEncontrado.Nombre}");
            Console.WriteLine($"Apellido:      {pacienteEncontrado.Apellido}");
            Console.WriteLine($"Número de Documento:      {pacienteEncontrado.DocumentoId}");
            Console.WriteLine($"Teléfono:      {pacienteEncontrado.Telefono}");
            Console.WriteLine($"----------------------------------------");

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        public async Task ActualizarPaciente()
        {
            Console.Clear();
            Console.WriteLine(">>> ACTUALIZAR INFORMACIÓN DE PACIENTE <<<");

            int id = await SolicitarYValidarPacienteIdAsync();

            Console.WriteLine("\n*(Ingrese los nuevos datos del paciente)*\n");

            Console.Write("Nuevo Documento Identidad: ");
            string documento = Console.ReadLine() ?? "";

            Console.Write("Nuevo Nombre: ");
            string nombre = Console.ReadLine() ?? "";

            Console.Write("Nuevo Apellido: ");
            string apellido = Console.ReadLine() ?? "";

            Console.Write("Nueva Fecha Nacimiento (AAAA-MM-DD): ");
            if (!DateTime.TryParseExact(Console.ReadLine(),
                   "yyyy-MM-dd",
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out DateTime fecha))
            {
                MostrarMensaje("Fecha inválida.", ConsoleColor.Red);
                return;
            }

            Console.Write("Nuevo Teléfono: ");
            string tel = Console.ReadLine() ?? "";

            var pacienteModificado = new Paciente
            {
                Id = id,
                DocumentoId = documento,
                Nombre = nombre,
                Apellido = apellido,
                FechaNacimiento = fecha,
                Telefono = tel
            };

            try
            {
                // Consumimos tu método ActualizarPacienteAsync
                await _pacienteService.ActualizarPacienteAsync(pacienteModificado);
                MostrarMensaje("Los datos del paciente han sido actualizados con éxito.", ConsoleColor.Green);
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

        public async Task EliminarPaciente()
        {
            Console.Clear();
            Console.WriteLine(">>> ELIMINAR PACIENTE DEL SISTEMA <<<");
            Console.Write("Ingrese el ID del paciente a borrar: ");

            int id = await SolicitarYValidarPacienteIdAsync();

            try
            {
                await _pacienteService.EliminarPacienteAsync(id);
                MostrarMensaje("El paciente ha sido eliminado del sistema.", ConsoleColor.Green);
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
    }
}
