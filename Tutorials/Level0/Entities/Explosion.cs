﻿using Kriptok.Div;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using System.Drawing;

namespace Tutorials.Level0
{
    public class Explosion : SpriteAnimation
    {
        public Explosion(Vector3F location, PointF scale, float angle)
            : base(DivResources.Image("Tutorial.Explosion.png"), 5, 4)
        {
            Location = location;
            Scale = scale;
            Angle.Z = angle;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.Audio.GetWaveHandler(Assembly, "Level0.Assets.Sounds.Explosion1.wav").Play();
        }    
    }    
}
