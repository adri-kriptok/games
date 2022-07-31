using Kriptok.Noid.Entities.Pills;
using Kriptok.Objects.Base;
using Kriptok.Objects.Collisions;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Noid.Entities
{
    class Brick : ObjectBase<ISpriteView>
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

        /// <summary>
        /// Indica si el bloque puede ser golpeado (por un laser).
        /// </summary>        
        internal virtual bool CanBeHit() => true;

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

        internal static ObjectBase Create(int type, int x, int y)
        {
            if (type == 8)
            {
                return new BrickWall(x, y);
            }
            else if (type == 7)
            {
                return new BrickSolid(x, y);
            }

            return new Brick(type, x, y);
        }

        internal void Drop()
        {
            falling = true;
            Location.Z = 10;
        }

        internal virtual void Hit()
        {
            Drop();

            if (Rand.Next(0, 100) < 20)
            {
#if DEBUG
                Trace.WriteLine("pill");
#endif
                addPill = true;
            }
        }

        /// <summary>
        /// Indica si la bola rebota contra este ladrillo.
        /// </summary>
        internal virtual bool Bounces => !falling;

        internal virtual bool CanBeDestroyed() => Bounces;
    }

    class BrickWall : Brick
    {
        private readonly IndexedSpriteView view;

        public BrickWall(int x, int y) 
            : base(x, y, new IndexedSpriteView(typeof(BrickWall).Assembly, GetBrickName(8), 1, 3))
        {
            this.view = (IndexedSpriteView)View;
        }

        internal override void Hit()
        {
            if (view.Graph == 2)
            {
                Die();
            }
            else
            {
                view.Graph++;
            }
        }

        internal override bool CanBeDestroyed() => base.CanBeDestroyed() && view.Graph == 0;
    }

    class BrickSolid : Brick
    {
        private readonly IndexedSpriteView view;
        private bool flashing;
        private bool change;

        public BrickSolid(int x, int y)
            : base(x, y, new IndexedSpriteView(typeof(BrickWall).Assembly, GetBrickName(7), 1, 3))
        {
            this.view = (IndexedSpriteView)View;
        }

        /// <summary>
        /// Siempre rebota este tipo de ladrillos.
        /// </summary>
        internal override bool Bounces => true;

        internal override RectangleF GetRect() => new RectangleF(Location.X - 7.75f, Location.Y - 3.75f, 15.5f, 7.5f);

        protected override void OnFrame()
        {
            base.OnFrame();

            if (change)
            {
                Add(new Brick(5, (int)Location.X, (int)Location.Y));
                Die();
                return;
            }
            else if (flashing)
            {
                if (view.Graph == 2)
                {
                    flashing = false;
                    view.Graph--;
                }
                else
                {
                    view.Graph++;
                }
            }
            else
            {
                if (view.Graph > 0)
                {
                    view.Graph--;
                }                
            }
        }

        internal override void Hit()
        {
            flashing = true;
        }

        internal override bool CanBeDestroyed() => false;

        internal void Change()
        {
            this.change = true;
        }

        internal override bool CanBeHit() => false;
    }
}
