using Kriptok.Drawing;
using Kriptok.Extensions;
using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Pacoman.Entities
{
    public class Eyes : ProcessBase<IndexedSpriteView>
    {
        /// <summary>
        /// Mapa de búsqueda de caminos.
        /// </summary>
        private static readonly FastBitmap8 pathFind = new FastBitmap8(typeof(Ghost).Assembly, "Assets.Images.BoardPathFind.png");

        /// <summary>
        /// Numero de grafico.
        /// </summary>
        private int imagen;

        private readonly int modelo;
        private readonly FastBitmap8 hardnesses;
        private readonly int level;

        public Eyes(FastBitmap8 hardnesses, int x, int y, int modelo, int level)
            :base(new IndexedSpriteView(typeof(Eyes).Assembly, "Assets.Images.GhostEyes.png", 2, 2))
        {
            this.level = level;
            this.Location.X = x;
            this.Location.Y = y;
            this.modelo = modelo;

            this.hardnesses = hardnesses;
        }

        public int X { get { return Location.X.Floor(); } set { Location.X = value; } }

        public int Y { get { return Location.Y.Floor(); } set { Location.Y = value; } }

        protected override void OnBegin()
        {
            Loop(() =>
            {
                // Comprueba los lados de la pantalla
                if (X < 105) X = 105;
                if (X > 554) X = 554;

                // Selecciona la direccion y el grafico de acuerdo al color del mapa de caminos
                switch (pathFind.Sample((ushort)((X - 105) / 2), (ushort)((Y - 1) / 2)))
                {
                    case 14: X -= 2; View.Graph = 0; break;
                    case 10: X += 2; View.Graph = 1; break;
                    case 12: Y += 2; View.Graph = 2; break;
                    case 9:  Y -= 2; View.Graph = 3; break;
                    // Si es el color 11 es que ha llegado a casa, quita los ojos y pone un fantasma
                    case 11: imagen = 0;
                    {
                        Add(new Ghost(hardnesses, X, Y, modelo, level)); 
                        Die(); 
                        break;
                    }
                }

                // Solo muestra los graficos cada cuatro imagens
                if ((imagen & 3) == 0)
                {
                    Frame();
                }

                imagen++;
            });
        }
    }
}
