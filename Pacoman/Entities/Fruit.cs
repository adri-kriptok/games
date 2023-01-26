using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Sprites;

namespace Pacoman.Entities
{
    public class Fruit : ProcessBase<IndexedSpriteView>
    {
        /// <summary>
        /// Tabla con la puntuacion de las frutas.
        /// </summary>
        public static int[] Scores = new int[] { 0, 100, 300, 500, 500, 700, 700, 1000, 1000, 2000, 2000 };

        /// <summary>
        /// Tabla con los graficos de la puntuacion de la fruta.
        /// </summary>
        public static int[] ScoreGraphics = new int[] { 0, 0, 1, 2, 2, 3, 3, 4, 4, 5, 5 };

        /// <summary>
        /// Tabla con los graficos de la fruta.
        /// </summary>
        public static int[] Graphics = new int[] { 0, 0, 1, 2, 2, 3, 3, 4, 4, 5, 5 };

        /// <summary>
        /// Contador de tiempo.
        /// </summary>
#if DEBUG        
        private int timer = 10000;
#else
        private int timer = 100;
#endif
        private ISingleCollisionQuery<Paco> pacoCollision;
        private ISoundHandler fruitSound;
        private readonly int level;

        public Fruit(int level) : base(new IndexedSpriteView(typeof(Fruit).Assembly, "Assets.Images.Fruits.png", 3, 2))
        {
            this.level = level;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;
            this.pacoCollision = h.GetCollision2D<Paco>();

            this.fruitSound = h.Audio.GetWaveHandler("Assets.Sounds.EatFruit.wav");
        }

        protected override void OnBegin()
        {
            // Asigna las coordenadas y el grafico.
            Location.X = 320;                  
            Location.Y = 270;
            View.Graph = Graphics[level];
            
            // Selecciona el grafico.
            Location.Z = 10;                   

            // Repite hasta que tiempofruto sea 0.
            While(() => timer > 0, () =>
            {
                // Decrementa tiempo de fruta                                    
                timer--;      

                // Fruta comida
                if (pacoCollision.OnCollision())
                {
                    // Hace el sonido.                          
                    fruitSound.Play();

                    // Suma puntuacion.
                    Global.Score += Scores[level];

                    // Pon grafico con puntos.                    
                    base.Add(new EatScore(Location, 1, ScoreGraphics[level]));

                    // Elimina la fruta.
                    Die();
                }
                Frame();
            });
        }
    }
}
