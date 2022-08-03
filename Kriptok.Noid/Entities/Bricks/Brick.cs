using Kriptok.Noid.Entities.Pills;
using Kriptok.Noid.Scenes;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Views.Sprites;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace Kriptok.Noid.Entities
{
    class Brick : EntityBase<ISpriteView>
    {
        private bool falling = false;

        /// <summary>
        /// Indica si debe agregar una píldora en el próximo frame.
        /// </summary>
        private bool addPill = false;

        public Brick(int type, int x, int y)
          : this(x, y, new SpriteView(typeof(Brick).Assembly, GetBrickName(type)))
        {
        }

        protected Brick(int x, int y, ISpriteView view) : base(view)
        {
            Location.X = x;
            Location.Y = y;
        }

        protected override void OnStart(ObjectStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Rectangle;
        }

        internal static string GetBrickName(int type)
        {
            return $"Assets.Images.Blocks.Block{type}.png";
        }

        protected override void OnFrame()
        {
            if (addPill)
            {
                Add(PillBase.Create(Location, Rand.Next(0, 9)));
                addPill = false;
            }

            if (falling)
            {
                if (Location.Y > 210)
                {
                    Die();
                }

                Angle.Z += 0.1f;
                View.Scale = new PointF(View.Scale.X * 0.99f, View.Scale.Y * 0.99f);
                Location.Y += 2;
            }
        }

        internal virtual RectangleF GetRect() => new RectangleF(Location.X - 8f, Location.Y - 4f, 16f, 8f);

        internal static EntityBase Create(int type, int x, int y)
        {
            if (type == 8)
            {
                return new BrickWall(x, y);
            }
            else if (type == 7)
            {
                return new BrickSolid(x, y);
            }
            else if (type == 6)
            {
                return new MovingBrick(x, y);
            }

            return new Brick(type, x, y);
        }

        internal void Drop()
        {
            Global.Score += 5;
            falling = true;
            Location.Z = -10;
            CheckLastBrick();
        }

        internal virtual void Hit()
        {            
            Sounds.TaikoDrum.Play(90, 127);

            Drop();

            if (Rand.Next(0, 100) < 20)
            {
                addPill = true;
            }
        }

        /// <summary>
        /// Indica si la bola rebota contra este ladrillo.
        /// </summary>
        internal virtual bool Bounces => !falling;

        internal virtual bool CanBeDestroyed() => Bounces;

        /// <summary>
        /// Indica si el bloque puede ser golpeado.
        /// </summary>        
        internal virtual bool CanBeHit() => !falling;

        internal virtual bool CanBeHitByLasers() => !falling;

        /// <summary>
        /// Chequea si es el último ladrillo.
        /// </summary>
        internal void CheckLastBrick()
        {
            var brickCount = Find.All<Brick>()                
                .Where(p => !p.Equals(this))
                .Where(p => p.CanBeHit()).Count();

            if (brickCount == 0)
            {
                Scene.SendMessage(LevelSceneMessages.CheckBricks);
            }
        }

        /// <summary>
        /// Mata el objeto si ya no está en juego.
        /// </summary>
        internal void KillIfFalling()
        {
            if (falling)
            {
                Die();
            }
        }
    }
}
