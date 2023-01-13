using Kriptok.Core;
using Kriptok.Regions.Pseudo3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.AZ.Regions
{
    internal class Vector3DContext : Pseudo3DRenderContext
    {
        protected internal Vector3DContext(Graphics g, Camera camera, Pseudo3DRegionBase region) 
            : base(g, camera, region)
        {
        }
    }
}
