using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.Alien.Entities.Enemies
{
    internal class Bunker : ProcessBase<IndexedSpriteView>
    {
        private int energia = 3000;
        private ISoundHandler shootSound;
        private IQuery<bool?> outScreen;
        private ISingleCollisionQuery<RobotShot> shotColl;
        private ISingleCollisionQuery<Explosion3> expColl;
        private int disparo;
        private readonly int graph;

        public Bunker(int graph, int x, int y)
            : base(new IndexedSpriteView(typeof(Bunker).Assembly, $"Assets.Images.Bunker{graph}.png", 2, 1))
        {
            this.graph = graph;
            View.Graph = 0;
            Location.X = x;
            Location.Y = y;
            Location.Z = 9f;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = Collision2DTypeEnum.Auto;

            shootSound = h.Audio.GetWaveHandler(Assembly, "Assets.Sounds.ESCO_AT1.WAV");

            outScreen = h.GetOutOfScreenQuery();
            shotColl = h.GetCollision2D<RobotShot>();
            expColl = h.GetCollision2D<Explosion3>();
        }

        protected override void OnBegin()
        {
            // Espera ha estar en pantalla
            While(() => outScreen.Result.GetValueOrDefault(true), () => Frame());

            While(() => (energia > 0), () =>
            {
                // Comprueba si le ha tocado el disparo del robot
                if (shotColl.OnCollision(out RobotShot playerShot))
                {
                    Add(new Impact(playerShot.Location));
                    playerShot.Die();
                    energia -= 175;
                }

                if (expColl.OnCollision(out Explosion3 explosion))
                {
                    energia -= 100;
                }

                // Comprueba si esta fuera de pantalla
                if (!outScreen.Result.GetValueOrDefault(true))
                {
                    if (disparo-- <= 0)
                    {
                        disparo = 15;

                        if (graph == 0)
                        {
                            Add(new BunkerShot(Location.X + 38, Location.Y + 26, graph, 8, 8));
                        }
                        else
                        {
                            Add(new BunkerShot(Location.X - 32, Location.Y + 28, graph, -8, 8));
                        }

                        shootSound.Play();
                    }
                }
                Frame();
            });

            // El bunker es destruido y...
            View.Graph = 1;

            Add(new Explosion2(Location, 1f));
            
            Global.puntuacion += 200;

            // Todo el rato esta echando humo
            Loop(() =>
            {
                if (!outScreen.Result.GetValueOrDefault(true))
                {
                    Add(new Smoke2(Location.X + Rand.Next(-4, 4), Location.Y + Rand.Next(-4, 4)));
                }
                Frame();
            });
        }

        private class BunkerShot : EntityBase<SpriteView>
        {
            private readonly float speedX;
            private readonly float speedY;
            private IQuery<bool?> outOfRegion;
            private ISingleCollisionQuery<Robot> collRobot;
            private ISingleCollisionQuery<RobotLegs> collRobotLegs;

            public BunkerShot(float x, float y, int graph, float speedX, float speedY)
                : base(new SpriteView(typeof(BunkerShot).Assembly, $"Assets.Images.BunkerShot{graph}.png"))
            {
                Location.X = x;
                Location.Y = y;

                this.speedX = speedX;
                this.speedY = speedY;
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);

                h.CollisionType = Collision2DTypeEnum.Auto;

                this.outOfRegion = h.GetOutOfScreenQuery();
                this.collRobot = h.GetCollision2D<Robot>();
                this.collRobotLegs = h.GetCollision2D<RobotLegs>();
            }

            protected override void OnFrame()
            {
                if (outOfRegion.Result.GetValueOrDefault(false))
                {
                    Die();
                }
                else if (collRobot.OnCollision() || collRobotLegs.OnCollision())
                {
                    Add(new Impact(Location));
                    Global.energia_robot -= 1000;
                    Die();
                }
                else
                {
                    Location.X += speedX;
                    Location.Y += speedY;
                }
            }
        }
    }
}
