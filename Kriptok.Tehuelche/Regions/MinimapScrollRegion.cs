using Kriptok.Common;
using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Kriptok.Regions.Context;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Base;
using Kriptok.Views.Base;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Tehuelche.Regions
{
    internal class MinimapScrollRegion : ScrollRegion
    {
        public MinimapScrollRegion(Rectangle rect, Resource texture) : base(rect, new GdipBrushScrollLayer(texture, false, false)
        {
            ScaleX = 0.0625f,
            ScaleY = 0.0625f
        })
        {
        }
    }
}
