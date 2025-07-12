using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.JazzJackRabbit
{
    class JazzConsts
    {
        public const int TileSize = 32;
        public const int HalfTileSize = TileSize / 2;

        public const float InvTileSize = 1f / TileSize;

        // public const float Timer = 0.1875f;
        public const float Timer = 0.2f;

        /// <summary>
        /// Modificador que se agrega al calcular la altura de la "plataforma de abajo" cuando el personaje
        /// está cayendo.
        /// </summary>
        public const int HeightModifierWhileFalling = 1;
    }
}
