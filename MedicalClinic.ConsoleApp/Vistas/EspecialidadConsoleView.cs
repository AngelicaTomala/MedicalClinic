using MedicalClinic.AccesoDatos.Entidades;
using MedicalClinic.LogicaNegocio.Exceptions;
using MedicalClinic.LogicaNegocio.Interfaces;

namespace MedicalClinic.ConsoleApp.Vistas
{
    public class EspecialidadConsoleView(IEspecialidadService _especialidadService) : MensajeConsoleView
    {
        // ====================================================
        // FORMULARIOS CRUD : ESPECIALIDAD
        // ====================================================
        public async Task CrearEspecialidad()
        {
            Console.Clear();
            Console.WriteLine(">>> REGISTRAR NUEVA ESPECIALIDAD <<<");

            Console.Write("Nombre de la Especialidad: ");
            string nombre = Console.ReadLine() ?? "";

            Console.Write("Descripción (Opcional): ");
            string descripcion = Console.ReadLine() ?? "";

            var nuevaEspecialidad = new Especialidad
            {
                Nombre = nombre,
                Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion
            };

            try
            {
                await _especialidadService.CrearEspecialidadAsync(nuevaEspecialidad);
                MostrarMensaje("Especialidad registrada con éxito.", ConsoleColor.Green);
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

        public async Task ListarEspecialidades()
        {
            Console.Clear();
            Console.WriteLine(">>> LISTA DE ESPECIALIDADES REGISTRADAS <<<\n");

            var lista = await _especialidadService.ObtenerEspecialidadesAsync();
            bool hayRegistros = false;

            foreach (var esp in lista)
            {
                hayRegistros = true;
                Console.WriteLine($"- ID: {esp.Id} | Nombre: {esp.Nombre} | Desc: {esp.Descripcion ?? "N/A"}");
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

        public async Task BuscarEspecialidadPorId()
        {
            Console.Clear();
            Console.WriteLine(">>> BUSCAR ESPECIALIDAD POR ID <<<");

            int id = await SolicitarYValidarEspecialidadIdAsync();

            try
            {
                var especialidadEncontrada = await _especialidadService.ObtenerEspecialidadPorIdAsync(id);

                // imprimimos la información en pantalla
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[Registro Encontrado]");
                Console.ResetColor();
                Console.WriteLine($"----------------------------------------");
                Console.WriteLine($"ID:          {especialidadEncontrada.Id}");
                Console.WriteLine($"Nombre:      {especialidadEncontrada.Nombre}");
                Console.WriteLine($"Descripción: {especialidadEncontrada.Descripcion ?? "Sin descripción registrada."}");
                Console.WriteLine($"----------------------------------------");

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

        public async Task ActualizarEspecialidad()
        {
            Console.Clear();
            Console.WriteLine(">>> ACTUALIZAR ESPECIALIDAD <<<");

            int especialidadID = await SolicitarYValidarEspecialidadIdAsync();

            Console.Write("Ingrese nuevo nombre: ");
            string NewNombre = Console.ReadLine() ?? "";

            Console.Write("Ingrese nueva descripcion: ");
            string NewDescripcion = Console.ReadLine() ?? "";

            var updateEspecialidad = new Especialidad
            {
                Id = especialidadID,
                Nombre = NewNombre,
                Descripcion = string.IsNullOrWhiteSpace(NewDescripcion) ? null : NewDescripcion
            };

            try
            {
                await _especialidadService.ActualizarEspecialidadAsync(updateEspecialidad);
                MostrarMensaje("Especialidad actualizada con éxito.", ConsoleColor.Green);
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

        public async Task EliminarEspecialidad()
        {
            Console.Clear();
            Console.WriteLine(">>> ELIMINAR ESPECIALIDAD <<<");

            int especialidadID = await SolicitarYValidarEspecialidadIdAsync();

            try
            {
                await _especialidadService.EliminarEspecialidadAsync(especialidadID);
                MostrarMensaje("Especialidad eliminada con éxito.", ConsoleColor.Green);
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

        private async Task<int> SolicitarYValidarEspecialidadIdAsync()
        {
            int especialidadId;
            while (true)
            {
                Console.Write("\nIngrese el Id de la Especialidad: ");
                if (!int.TryParse(Console.ReadLine(), out especialidadId))
                {
                    MostrarMensaje("El ID ingresado debe ser un número entero. Intente nuevamente.", ConsoleColor.Red);
                    continue;
                }

                try
                {
                    await _especialidadService.ObtenerEspecialidadPorIdAsync(especialidadId);
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
    }
}
