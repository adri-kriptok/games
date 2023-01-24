using Kriptok.Div;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Extensions;
using Kriptok.Tehuelche.Entities.Enemies;
using System;
using System.Diagnostics;

namespace Kriptok.Tehuelche.Entities.Player
{
    internal class Explosion : SpriteAnimation
    {
        public Explosion(Vector3F location, float scale) 
            : base(DivResources.Image("Tutorial.Explosion.png"), 5, 4)
        {
            Location = location;            
            Angle.X = Rand.NextF();
            View.ScaleX = scale * 0.1f;
            View.ScaleY = scale * 0.1f;
        } 
    }
}