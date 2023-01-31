using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exploss.Entities
{
    internal class BombPickup : EntityBase<SpriteView>
    {
        private int counter = 100;
        private ISingleCollisionQuery<PlayerCar> collision;
        private ISoundHandler sound;

        public BombPickup(int x, int y)
            : base(new SpriteView(typeof(Bombs).Assembly, "Assets.Images.BombPickup.png"))
        {
            Location.X = x;
            Location.Y = y;
            Location.Z = -1f;
        }

        protected override void OnStart(EntityStartHandler h)
        {         
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;
            this.collision = h.GetCollision2D<PlayerCar>();
            this.sound = h.Audio.GetSoundHandler(Assembly, "Assets.Sounds.BombPickup.wav");
        }

        protected override void OnFrame()
        {
            if (counter-- < 0)
            {
                Die();
                return;
            }

            // Comprobamos si es cogido por el coche con la funcion COLLISION
            if (collision.OnCollision())
            {
                sound.Play();
                
                Global.CurrentBombs = Math.Min(Global.CurrentBombs + 4, Consts.MaxBombs);
                Die();
                return;
            }
        }
    }
}
