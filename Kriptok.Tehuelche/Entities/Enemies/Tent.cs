using Kriptok.Div;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Regions.Scroll;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Tehuelche.Regions;
using Kriptok.Tehuelche.Scenes.Base;
using Kriptok.Tehuelche.Views;
using Kriptok.Views.Shapes;
using System;
using static Kriptok.Tehuelche.Enemies.Tent;

namespace Kriptok.Tehuelche.Enemies
{
    internal class Tent : EnemyBase<TentView>
    {
        public Tent(LevelBuilder builder, int x, int y, float angle) 
            : base(builder, new TentView(), 10f)
        {
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);

            Location.X = (float)(x + cos * 25f);
            Location.Y = (float)(y + sin * 25f);
            Angle.Z = angle + MathHelper.PIF;            
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision3DViewOBB();
        }

        protected override void OnFrame()
        {
            base.OnFrame();
        }

        internal override void OnDying()
        {
            base.OnDying();
            Audio.PlayWave(DivResources.Sound("Guerra.EXPLOS00.WAV"));
        }

        internal override Vector3F GetAimAngle()
        {
            throw new NotImplementedException();
        }

        public class TentView : VoxelSpaceShapeViewBase
        {
            public TentView() : base(typeof(TentView).Assembly, "Assets.Models.Tent.mqo")
            {
                Scale = new Vector3F(0.1f);
            }
        }
    }    
}
