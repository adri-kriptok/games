using Kriptok.Views.Sprites;

namespace Galax.Entities
{
    class Enemy2 : EnemyBase<IndexedSpriteView>
    {
        /// <summary>
        /// Configuracion de la animacion.
        /// </summary>
        private readonly int[] animacion = new int[] { 0, 1, 2, 3, 2, 1 };

        /// <summary>
        /// Numero de animacion.
        /// </summary>
        private int graph = 0;

        public Enemy2(int index)         
            : base(index, new IndexedSpriteView(typeof(Enemy3).Assembly, "Assets.Images.Enemy2.png", 2, 2))
        {
            View.Graph = 0;
        }

        protected override int EnemyType => 2;

        protected override void OnFrame()
        {
            // Animacion de enemigos tipo 2
            graph++;
            if (graph >= animacion.Length)
            {
                graph = 0;
            }
            View.Graph = animacion[graph];

            base.OnFrame();
        }

        protected override void PlaySound()
        {           
        }

        protected override void Shoot(int rnd)
        {            
        }
    }
}
