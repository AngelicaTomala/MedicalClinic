using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalClinic.ConsoleApp.Vistas
{
    public abstract class MensajeConsoleView
    {
        protected void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"\n{mensaje}");
            Console.ResetColor();
            //Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
            //Console.ReadKey();
        }
    }
}
