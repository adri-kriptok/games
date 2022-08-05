using Kriptok.Drawing.Algebra;
using Kriptok.Helpers;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Asteroids.Entities
{
    class Asteroid : EntityBase
    {
        /// <summary>
        /// Velocidad de los asteroides
        /// </summary>
        private int speed;

        /// <summary>
        /// Tamaño de la pantalla.
        /// </summary>
        private Size regionSize;

        /// <summary>
        /// Velocidad de giro.
        /// </summary>
        private float angleIncrement;

        /// <summary>
        /// Direcci¢n de avance.
        /// </summary>
        private float angle;

        /// <summary>
        /// Nivel al que pertenece el asteroide.
        /// </summary>
        private readonly int level;

        private readonly int size;
        private ISingleCollisionQuery<Shot> shotCollision;
        private ISingleCollisionQuery<Ship> shipCollision;

        public Asteroid(int level, int size) : base(new AsteroidView(size))
        {
            this.level = level;
            ResetLocation();
            this.size = size;            
        }

        public Asteroid(int level, float x, float y, int size) : base(new AsteroidView(size))
        {
            this.level = level;
            Location.X = x;
            Location.Y = y;
            this.size = size;
        }

        internal void ResetLocation()
        {
            Location.X = -16;
            Location.Y = -16;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = Collision2DTypeEnum.Auto;            
            this.shotCollision = h.GetCollision2D<Shot>();
            this.shipCollision = h.GetCollision2D<Ship>();

            // Selecciona la direcci¢n del asteroide
            Angle.Z = angle = Rand.NextF(0f, MathHelper.TwoPIF);

            // Y la velocidad del giro
            angleIncrement = Rand.NextF(-MathHelper.PIF / 32, MathHelper.PIF / 32);

            // La velocidad depende de la fase del juego y del tama¤o del asteroide
            speed = size + level;

            regionSize = h.RegionSize;
        }

        protected override void OnFrame()
        {
            // Comprueba si se ha chocado con un disparo
            if (shotCollision.OnCollision(out Shot shot))
            {
                // Suma puntuación.
                Global.Score += 25 * size + (level - 1) * 25;  
                    
                shot.Die();                 // Elimina el disparo
                // sound(sonido_explosion, 30 * (6 - graph), 33 * graph);
                Audio.PlayWave(Assembly, "TUBO5.WAV");

                if (size < 5)
                {                      // Si el asteroide es muy grande
                    Add(new Asteroid(level, Location.X, Location.Y, size + 1));         // Crea dos m s peque¤os
                    Add(new Asteroid(level, Location.X, Location.Y, size + 1));
                }
                if (size == 3)
                {                      // Si es el asteroide m s grande
                    Add(new Asteroid(level, Location.X, Location.Y, size + 1));         // Crea uno m s (3 en total)
                }

                // Elimina el asteroide actual
                Die();

                // Si es el último asteroide...
                if (Find.All<Asteroid>().Length == 0)
                {
                    // ... le aviso a la actividad que pase de nivel.
                    Scene.SendMessage(PlaySceneMessages.Win);
                }

                return;
            }

            // Comprueba si se ha chocado con la nave                
            if (shipCollision.OnCollision(out Ship ship))
            {                        
                // Hace sonido de destrucción.
                Audio.PlayWave(Assembly, "TUBO5.WAV");                        

                // Elimina el proceso de la nave
                if (IsAlive())
                {
                    ship.Explode();
                }
            }

            // Mueve los asteroides            
            XAdvance2D(speed, angle);

            // Comprueba que si se ha salido de pantalla y actualiza la posici¢n en consecuencia
            Location = Helpers.Relocate(Location, regionSize);

            // Gira el asteroide            
            Angle.Z += angleIncrement;                 
        }

        class AsteroidView : PolygonView
        {
            public AsteroidView(int graph) : base(GetPolygon(graph), null, Strokes.Get(Color.Orange, 3f, LineJoin.Round))
            {
                Rounded = true;
            }

            private static PointF[] GetPolygon(int graph)
            {
                var list = new List<PointF>();

                int len = (6 - graph);
                var vertices = len * 5;

                var angle = 0f;
                var angleInc = MathHelper.TwoPIF / vertices;

                for (int i = 0; i < vertices; i++, angle += angleInc)
                {
                    var lenF = 10f * len - 2.5f + Rand.NextF(0, 7.5f);

                    list.Add(new PointF()
                    {
                        X = ((float)Math.Cos(angle)) * lenF,
                        Y = ((float)Math.Sin(angle)) * lenF,
                    });
                }

                var arr = list.ToArray();
                var avg = PointFHelper.GetRectangleF(arr);

                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = new PointF()
                    {
                        X = arr[i].X - avg.X,
                        Y = arr[i].Y - avg.Y
                    };
                }

                return arr;
            }
        }
    }
}
