using Kriptok.Common;
using Kriptok.Entities.Base;
using Kriptok.Entities.Partitioned;
using Kriptok.Entities.Wld;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Mapping.Terrains;
using Kriptok.Regions.Partitioned.Wld;
using Kriptok.Regions.Pseudo3D.Partitioned.Terrain;
using Kriptok.Views.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Intruder.Entities.Enemies
{
    internal class Brachiosaurus : WldEntityBase<BrachiosaurusView>
    {
        private float inc = 0f;

        public Brachiosaurus(IPseudo3DTerrainRegion map) : base(map, new BrachiosaurusView())
        {            
            Radius = 2000;                      
        }

        protected override void OnStart(Part2DEntityStartHandler h)
        {
            base.OnStart(h);
        
            Angle.Z = (float)Math.Acos(Math.Cos(Location.X + Location.Y));
            View.ScaleX = 20f + MathHelper.CosF(Location.X) * 2f;
            View.ScaleY = 20f + MathHelper.SinF(Location.Y) * 2f;
            inc = MathHelper.CosF(Location.X) * 1000f;

            View.RotateTransform((MathHelper.CosF(Location.X) * 20).Round());
        }

        public override uint GetHeight() => 4000;

        protected override void OnFrame(float timeDelta)
        {
            inc += timeDelta + timeDelta * View.Graph;
            if (inc > 2000f)
            {
                //Angle.X += 0.05f;
                View.Rotate();
                inc %= 2000f;
            }

            if (inc < 100)
            {
            }
        }
    }

    class BrachiosaurusView : DirectionalSpriteView
    {
        private static readonly Resource resource = Resource.Get(typeof(BrachiosaurusView).Assembly, "Assets.Images.Dinosaurs.Brachiosaurus.png");

        private static readonly int[,] matrix = new int[2, 8]
        {
            { 0, 1, 2, 3, 4, 5, 6, 7  },
            { 8, -1, -2, -3, -4, -5, -6, -7}
        };

        public BrachiosaurusView() : base(resource, 4, 2, matrix)
        {
            //View.Center = new PointF(5f / 8f, 15f / 18f);
            View.Center = new PointF(0.5f, 15f / 18f);
        }
    }
}
