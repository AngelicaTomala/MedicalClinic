using MedicalClinic.AccesoDatos.Entidades;
using MedicalClinic.LogicaNegocio.Exceptions;
using MedicalClinic.LogicaNegocio.Interfaces;

namespace MedicalClinic.ConsoleApp.Vistas
{
    public class MedicoConsoleView(IMedicoService _medicoService, IEspecialidadService _especialidadService) : MensajeConsoleView
    {
        // ====================================================
        // FORMULARIOS CRUD : MEDICO
        // ====================================================
        public async Task CrearMedico()
        {
            Console.Clear();
            Console.WriteLine(">>> REGISTRAR UN NUEVO MEDICO <<<");

            Console.Write("Nombre : ");
            string nombre = Console.ReadLine() ?? "";

            Console.Write("Apellido : ");
            string apellido = Console.ReadLine() ?? "";

            Console.Write("Tarjeta Profesional: ");
            string tarjetaProfesional = Console.ReadLine() ?? "";

            await MostrarEspecialidadesDisponiblesAsync();
            int especialidadId = await SolicitarYValidarEspecialidadIdAsync();

            try
            {
                var nuevoMedico = new Medico
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    TarjetaProfesional = tarjetaProfesional,
                    EspecialidadId = especialidadId
                };

                await _medicoService.RegistrarMedicoAsync(nuevoMedico);
                MostrarMensaje($"Dr. {nombre} {apellido} registrado con éxito.", ConsoleColor.Green);
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

        public async Task ObtenerMedicosAsync()
        {
            Console.Clear();
            Console.WriteLine(">>> LISTADO DE MÉDICOS REGISTRADOS <<<\n");

            var lista = await _medicoService.ObtenerMedicosAsync();
            bool hayRegistros = false;

            foreach (var medico in lista)
            {
                hayRegistros = true;
                Console.WriteLine($"- ID: {medico.Id} | Nombre: {medico.Nombre} | Apellido: {medico.Apellido} | Tarjeta Profesional: {medico.TarjetaProfesional} | Especialidad: {medico.Especialidad.Nombre} ");
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

        public async Task ObtenerMedicosPorTarjetaProfesionalAsync()
        {
            Console.Clear();
            Console.WriteLine(">>> BUSCAR MEDICO POR TARJETA PROFESIONAL <<<");

            Medico? medicoEncontrado = null;

            while (true)
            {
                Console.Write("Ingrese tarjeta profesional del médico a buscar: ");
                string tarjetaProfesional = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(tarjetaProfesional))
                {
                    MostrarMensaje("La tarjeta profesional no puede estar vacía.", ConsoleColor.Red);
                    continue;
                }

                try
                {
                    // Verificamos si la especialidad existe
                    medicoEncontrado = await _medicoService.ObtenerMedicosPorTarjetaProfesionalAsync(tarjetaProfesional);
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
            Console.WriteLine($"ID:          {medicoEncontrado.Id}");
            Console.WriteLine($"Nombre:      {medicoEncontrado.Nombre}");
            Console.WriteLine($"Apellido:      {medicoEncontrado.Apellido}");
            Console.WriteLine($"Tarjeta Profesional:      {medicoEncontrado.TarjetaProfesional}");
            Console.WriteLine($"Especialidad:      {medicoEncontrado.Especialidad.Nombre}");
            Console.WriteLine($"----------------------------------------");

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        public async Task ActualizarMedicoAsync()
        {
            Console.Clear();
            Console.WriteLine(">>> ACTUALIZAR INFORMACIÓN DEL MÉDICO <<<");

            int medicoId = await SolicitarYValidarMedicoIdAsync();

            Console.Write("Ingrese nuevo nombre: ");
            string NewNombre = Console.ReadLine() ?? "";

            Console.Write("Ingrese nuevo apellido: ");
            string NewApellido = Console.ReadLine() ?? "";

            Console.Write("Ingrese nueva tarjeta profesional: ");
            string NewTarjetaProfesional = Console.ReadLine() ?? "";

            await MostrarEspecialidadesDisponiblesAsync();
            int especialidadId = await SolicitarYValidarEspecialidadIdAsync();

            var updateMedico = new Medico
            {
                Nombre = NewNombre,
                Apellido = NewApellido,
                TarjetaProfesional = NewTarjetaProfesional,
                EspecialidadId = especialidadId,
                Id = medicoId
            };

            try
            {
                await _medicoService.ActualizarMedicoAsync(updateMedico);
                MostrarMensaje("Especialidad actualizada con éxito.", ConsoleColor.Green);
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

        public async Task EliminarMedicoAsync()
        {
            Console.Clear();
            Console.WriteLine(">>> ELIMINAR MÉDICO <<<");

            int medicoId = await SolicitarYValidarMedicoIdAsync();

            try
            {
                await _medicoService.EliminarMedicoAsync(medicoId);
                MostrarMensaje("Médico eliminado con éxito.", ConsoleColor.Green);
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

        private async Task MostrarEspecialidadesDisponiblesAsync()
        {
            Console.WriteLine("\n[Especialidades Disponibles]:");
            var especialidades = await _especialidadService.ObtenerEspecialidadesAsync();
            foreach (var esp in especialidades)
            {
                Console.WriteLine($"  ID: {esp.Id} -> {esp.Nombre}");
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
    }
}
