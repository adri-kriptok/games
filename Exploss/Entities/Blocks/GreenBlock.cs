using Exploss.Scenes;
using Kriptok.Audio;
using Kriptok.Div;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exploss.Entities.Blocks
{
    /// <summary>
    /// Este proceso lo que hace es poner el grafico verde en pantalla.
    /// </summary>
    internal class GreenBlock : EntityBase<SpriteView>
    {
        private ISingleCollisionQuery<PlayerCar> playerCollision;
        private ISoundHandler sound;

        public GreenBlock(float x, float y)
            : base(new SpriteView(typeof(GreenBlock).Assembly, "Assets.Images.Blocks.png", 2 * 43, 0, 43, 43)
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

            playerCollision = h.GetCollision2D<PlayerCar>();
            this.sound = h.Audio.GetSoundHandler(DivResources.Sound("Efectos.Varios.BUIU.WAV"));
        }

        protected override void OnFrame()
        {
            if (playerCollision.OnCollision())
            {
                sound.Play();
                Global.Score += 5;
                Global.BlocksBroken += 1;
                Die();
            }
        }
    }
}
