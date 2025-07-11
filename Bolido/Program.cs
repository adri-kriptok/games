using Bolido.Entities;
using Bolido.Scenes;
using Kriptok;
using Kriptok.Core;
using Kriptok.IO;
using System;
using System.Threading;

namespace Bolido
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            Config.Load<BaseConfiguration>().Mute();
#endif

            Engine.Start(new InitScene(), s =>
            {
                s.Mode = WindowSizeEnum.M320x200;
                s.Title = "Bólido | Kriptok";
                //s.TimerInterval = 30;
            });

            return;

            // Array de ejemplo con valores para FRECUENCIA y CUENTA_RELOJ
            int[] sonido = { 50000, 300, 35000, 400, 20000, 500, 5000, 600,
                         4000, 65534, 3000, 65534, 2000, 65534, 1000, 65534, 0 };

            // Función para calcular la frecuencia efectiva ajustada
            int CalcularFrecuencia(int divisor)
            {
                const int FREQ_BASE = 1193182; // Frecuencia base del PIT
                const int FREQ_MIN = 37; // Frecuencia mínima de Console.Beep
                const int FREQ_MAX = 32767; // Frecuencia máxima de Console.Beep

                if (divisor <= 0)
                    return 0; // Divisor inválido

                // Calcular la frecuencia original
                int frecuencia = FREQ_BASE / divisor;

                // Ajustar al rango permitido
                if (frecuencia < FREQ_MIN)
                {
                    frecuencia = FREQ_MIN;
                }
                else if (frecuencia > FREQ_MAX)
                {
                    frecuencia = FREQ_MAX;
                }

                return frecuencia;
            }

            // Procesar el array de sonido
            for (int i = 0; i < sonido.Length; i+=2)
            {
                int valor = sonido[i];

                // Verificar fin de la secuencia
                if (valor == 0)
                {
                    Console.WriteLine("Fin de la secuencia");
                    break;
                }

                // Extraer la frecuencia y duración (suponiendo que los 8 bits más bajos son la duración y los 8 más altos la frecuencia)
                int frecuencia = valor >> 8;  // Desplazar los 8 bits más altos (frecuencia)
                int duracion = valor & 0xFF;  // Máscara para los 8 bits más bajos (duración)

                // Ajustar la frecuencia dentro del rango de Console.Beep
                frecuencia = CalcularFrecuencia(frecuencia);

                // Si la duración es 65534, esto puede representar una pausa (sin sonido)
                if (duracion == 65534)
                {
                    Console.WriteLine("Pausa breve");
                    Thread.Sleep(50); // Pausa breve simulada
                    continue;
                }

                // Reproducir sonido
                Console.WriteLine($"Reproduciendo: Frecuencia = {frecuencia} Hz, Duración = {duracion} ms");
                Console.Beep(frecuencia, duracion);
            }

        }
    }
}
