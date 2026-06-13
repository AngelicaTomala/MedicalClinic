namespace MedicalClinic.LogicaNegocio.Exceptions
{
    /// <summary>
    /// Presenta los errores que ocurren exclusivamente durante la ejecución de las reglas del negocio.
    /// Permite diferenciar fallos de validación de usuario (Ej: especialidad duplicada) de errores técnicos del servidor.
    /// </summary>
    public class MostrarMensaje : Exception
    {
        public MostrarMensaje(string message) : base(message)
        {

        }
    }
}
