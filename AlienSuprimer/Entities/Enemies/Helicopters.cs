using Kriptok.Audio;
using Kriptok.Common;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Games.Alien.Regions;
using Kriptok.Helpers;
using Kriptok.Regions.Context.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Games.Alien.Entities.Enemies
{
    internal class HelicopterBase : ProcessBase<DirectionalSpriteView>
    {
        private const float piQuarters = MathHelper.PIF / 4;
        private const float movementAngle = piQuarters / 32;
        private static readonly int[,] matrix = new int[16, 1]
        {
            { 08 }, { 09 }, { 10 }, { 11 },
            { 12 }, { 13 }, { 14 }, { 15 },
            { 00 }, { 01 }, { 02 }, { 03 },
            { 04 }, { 05 }, { 06 }, { 07 }
        };
        private readonly float screenX;
        private ISingleCollisionQuery<RobotShot> shotColl;
        private AlienScrollTarget cam;
        private int energy = 1500;
        private float initialX;
        private int directionY;
        private int disparo;
        private float angulo0 = 0f;

        public HelicopterBase(float screenX) : base(new DirectionalSpriteView(typeof(HelicopterBase).Assembly, "Helicopter.png", 4, 4, matrix))
        {
            Location.Z = -148f;
            this.screenX = screenX;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = Collision2DTypeEnum.Auto;
            this.shotColl = h.GetCollision2D<RobotShot>();

            this.cam = ((ScrollX2Region)h.Region).Cam;

            Location = new Vector3F(cam.GetLocation2D().Minus(screenX, h.RegionSize.Height * 0.5f + 20), Location.Z);
            this.initialX = Location.X;

            directionY = 1;
            disparo = Rand.Next(0, Global.HelicopterShootFrequency * 2 - 1);
        }

        protected override void OnBegin()
        {
            Add(new HelicopterHelix(this, 0, 1));
            Add(new HelicopterHelix(this, piQuarters, -1));

            While(() => energy > 0, () =>
            {
                if (directionY > 0 && Location.Y > cam.Location.Y + 220)
                {
                    directionY = -1;
                }
                else if(directionY < 0 && Location.Y < cam.Location.Y + 20)
                {
                    directionY = 1;
                }
                
                // Comprueba si le ha tocado el disparo del robot.
                if (shotColl.OnCollision(out RobotShot playerShot))
                {
                    Add(new Impact(playerShot.Location));
                    playerShot.Die();
                    energy -= 175;

                    if (energy <= 0)
                    {
                        Add(new Explosion1(Location));
                        Global.puntuacion += 125;
                        
                        Die();

                        return;
                    }
                }

                Location.Y += directionY;
                Location.X = initialX + PolarVector.ProjectX(angulo0, 80f);
                angulo0 += movementAngle;

                var player = Find.First<Robot>();
                if (player != null)
                {
                    Angle.Y = GetAngle2D(player);
                }

                if (disparo == 0)
                {
                    Add(new HelicopterShot(Location));
                    disparo = Global.HelicopterShootFrequency * Rand.Next(1, 3);
                }
                else
                {
                    disparo--;
                }

                Frame();
            });            
        }
    }

    internal class HelicopterHelix : EntityBase<SpriteView>
    {
        private const float piEights = MathHelper.PIF / 8;
        private readonly HelicopterBase owner;
        private readonly float direction;

        public HelicopterHelix(HelicopterBase owner, float angle, int direction) 
            : base(new HelicopterHelixView(owner.View, typeof(HelicopterHelix).Assembly, "HelicopterHelix.png"))
        {
            View.Alpha = 0.5f;
            this.owner = owner;
            this.AngleZ = angle;
            this.direction = direction;
        }        

        protected override void OnFrame()
        {
            if (!owner.IsAlive())
            {
                Die();
                return;
            }

            Location = owner.Location;
            Angle.Z += direction * piEights;
        }

        public override Vector3F GetRenderLocation() => new Vector3F(owner.Location.XY(), Location.Z);        

        private class HelicopterHelixView : SpriteView
        {
            private readonly DirectionalSpriteView view;
            
            public HelicopterHelixView(DirectionalSpriteView view, Assembly assembly, string resourceName)
                : base(assembly, resourceName)
            {
                this.view = view;                
            }            

            public override float GetPriority(IProjector context) => view.GetPriority(context) + 1;
        }
    }

    internal class Helicopter1 : HelicopterBase
    {
        public Helicopter1(float screenX) : base(screenX)
        {
        }
    }

    internal class Helicopter2 : HelicopterBase
    {
        public Helicopter2(float screenX) : base(screenX)
        {
        }
    }

    internal class Helicopter3 : HelicopterBase
    {
        public Helicopter3(float screenX) : base(screenX)
        {
        }
    }

    internal class HelicopterShot : EntityBase<SpriteView>
    {
        private IQuery<bool?> outOfScreenQuery;
        private ISingleCollisionQuery<Robot> collisionRobot;
        private ISingleCollisionQuery<RobotLegs> collisionRobotLegs;        
        private float speed;

        public HelicopterShot(Vector3F location) : base(new SpriteView(typeof(HelicopterShot).Assembly, "Assets.Images.HelicopterShot.png"))
        {
            Location= location;
            speed = 4f;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            h.Audio.GetWaveHandler(Assembly, "Sounds.EXPLOSIO.WAV").Play();
            Angle.Z = GetAngle2D(Global.MainRobot);
            h.CollisionType = Collision2DTypeEnum.Auto;

            outOfScreenQuery = h.GetOutOfScreenQuery();

            collisionRobot = h.GetCollision2D<Robot>();
            collisionRobotLegs = h.GetCollision2D<RobotLegs>();

        }

        protected override void OnFrame()
        {
            if (outOfScreenQuery.Result.GetValueOrDefault(false))
            {
                Die();
            }
            else if (collisionRobot.OnCollision() || collisionRobotLegs.OnCollision())
            {
                Add(new Impact(Location));
                Global.energia_robot -= 1000;
                Die();
            }
            else
            {
                Add(new TrailSmoke(Location));

                Advance2D(speed);
                if (speed < 8f)        // Incrementa la velocidad del misil
                {
                    speed += 0.5f;
                }
            }
        }
    }
}
