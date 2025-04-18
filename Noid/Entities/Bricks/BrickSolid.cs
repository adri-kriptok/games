﻿using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System.Drawing;

namespace Noid.Entities
{
    class BrickSolid : Brick
    {
        private readonly IndexedSpriteView view;
        private bool flashing;
        private bool change;
        private ISoundHandler metalSound;

        public BrickSolid(int x, int y)
            : base(x, y, new IndexedSpriteView(typeof(BrickWall).Assembly, GetBrickName(7), 1, 3))
        {
            this.view = (IndexedSpriteView)View;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            metalSound = h.Audio.GetWaveHandler(Sounds.MetalSound);
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
            metalSound.Play();
            flashing = true;
        }

        internal override bool CanBeDestroyed() => false;

        internal void Change()
        {
            this.change = true;
        }

        internal override bool CanBeHit() => false;

        internal override bool CanBeHitByLasers() => true;
    }
}
