using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Snake.Processes
{
    static class Global
    {
        public static int SnakeLength = 8;        // Longitud de la cola del gusano
        public static int Apples = 0;             // Número de manzanas en pantalla
        public static int Score = 0, Record = 0;  // Variables para puntuación y record
    }
}
