using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Common
{
    public class Explosion : EntityBase<IndexedSpriteView>
    {
        /// <summary>
        /// Contador de la animacion
        /// </summary>
        private int graphIndex;

        /// <summary>
        /// Primera grafico de la animacion
        /// </summary>
        private readonly int graph0;

        public Explosion(int type, int size, float x, float y)
            : base(new IndexedSpriteView(typeof(Explosion).Assembly, "Assets.Images.Explosions.png", 6, 3))
        {
            View.ScaleX = size * 0.01f;
            View.ScaleY = View.ScaleX;
            Location.X = x + Rand.Next(0, 14) - 7;
            Location.Y = y + Rand.Next(0, 14) - 7;    // Posicionalo de forma aleatoria

            // Inicializa variables
            graphIndex = 0;

            Location.Z = -1;
            graph0 = type * 6;

            // Selecciona el grafico inicial
            View.Graph = graph0;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.Audio.GetWaveHandler("Assets.Sounds.Explosion.wav").Play();
        }

        protected override void OnFrame()
        {
            Location.X -= 6;
            graphIndex++;

            if (graphIndex >= 6)
            {
                Die();
                return;
            }
            else if (graphIndex > 0)
            {
                View.Graph++;
            }
        }
    }
}
