using Kriptok.Audio;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Extensions;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exploss.Entities.Bubbles
{
    internal abstract class BubbleBase : EntityBase<SpriteView>
    {
        private ISingleCollisionQuery<Shot> collShot;
        private ISingleCollisionQuery<Bomb> collBomb;

        /// <summary>
        /// Sonidos que reproduce la esfera grande.
        /// </summary>
        private ISoundHandler exploss4Sound, exploss9Sound;

        private BoundF2 bounds;

        private Vector2F inc;

        protected BubbleBase(float x, float y, string resourceName) 
            : base(new SpriteView(typeof(BubbleBase).Assembly, resourceName))
        {
            Location.X = x;
            Location.Y = y;
            Location.Z = -1f;

            inc.X = 8f * Math.Sign(Math.Cos(x));
            inc.Y = 8f * Math.Sign(Math.Sin(y));
            
            if (inc.X == 0f) inc.X = 8f;
            if (inc.Y == 0f) inc.Y = 8f;            
        }

        protected BubbleBase(float x, float y, string resourceName, float incX, float incY)
            : this(x, y, resourceName)
        {
            inc.X = incX;
            inc.Y = incY;            
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            bounds.MinX = 43f + View.Size.Width / 2;
            bounds.MaxX = h.RegionSize.Width - 43f - View.Size.Width / 2;

            bounds.MinY = 43f + View.Size.Width / 2;
            bounds.MaxY = 43f * 8f - View.Size.Width / 2;

            h.CollisionType = Collision2DTypeEnum.Ellipse;
          
            this.collShot = h.GetCollision2D<Shot>();
            this.collBomb = h.GetCollision2D<Bomb>();

            this.exploss4Sound = h.Audio.GetWaveHandler("Assets.Sounds.EXPLOSS4.WAV");
            this.exploss9Sound = h.Audio.GetWaveHandler("Assets.Sounds.EXPLOSS9.WAV");

            Location.X = Location.X.Clamp(bounds.MinX, bounds.MaxX);
            Location.Y = Location.Y.Clamp(bounds.MinY, bounds.MaxY);
        }

        protected override void OnFrame()
        {
            // Si colisiona la bola grande con un disparo genera dos bolas medianas
            if (collShot.OnCollision(out Shot shot))
            {
                shot.Die();

                exploss4Sound.Play();

                Global.Score += 5;
                CreateSmallerBalls(inc);

                Die();
                return;
            }

            // Si colisiona con una bomba se destruye la bola completamente.
            if (collBomb.OnCollision(out Bomb bomb))
            {
                bomb.Die();
                exploss9Sound.Play();
                Global.Score += 5;
                Die();
                return;
            }

            Location.X += inc.X;
            Location.Y += inc.Y;

            if (Location.X <= bounds.MinX || Location.X >= bounds.MaxX) 
            { 
                inc.X = -inc.X; 
            }
            if (Location.Y <= bounds.MinY || Location.Y >= bounds.MaxY) 
            { 
                inc.Y = -inc.Y; 
            }

            Location.X = Location.X.Clamp(bounds.MinX, bounds.MaxX);
            Location.Y = Location.Y.Clamp(bounds.MinY, bounds.MaxY);
        }

        internal abstract void CreateSmallerBalls(Vector2F inc);
    }
}
