using Exploss.Entities.Blocks;
using Exploss.Scenes;
using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Div;
using Kriptok.Views.Sprites;
using System.Drawing;

namespace Exploss.Entities
{
    /// <summary>
    /// Este proceso lo que hace es poner el grafico marron en pantalla.
    /// </summary>
    public class Obstacle : EntityBase<SpriteView>
    {
        private ISingleCollisionQuery<Shot> shotCollision;
        private ISoundHandler exploss6Sound;

        public Obstacle(float x, float y) 
            : base(new SpriteView(typeof(GreenBlock).Assembly, "Assets.Images.Blocks.png", 43, 0, 43, 43)
            {
                Center = new PointF(0f, 0f)
            })
        {
            Location.X = x;
            Location.Y = y;            
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Rectangle;

            shotCollision = h.GetCollision2D<Shot>();
            exploss6Sound = h.Audio.GetSoundHandler("Sounds.EXPLOSS6.WAV");
        }

        protected override void OnFrame()
        {
            if (shotCollision.OnCollision())
            {
                exploss6Sound.Play();
                Global.Score += 5;
                Global.BlocksBroken += 1;
                Die();
            }
        }
    }
}
