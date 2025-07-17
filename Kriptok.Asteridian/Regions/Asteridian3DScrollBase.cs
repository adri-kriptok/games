using Kriptok.Drawing.Algebra;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Regions
{
    public interface IAsteridianScroll
    {
        int GetLevelHeight();
    }

    public abstract class Asteridian3DScrollBase : FixedScroll3DRegion, IAsteridianScroll
    {
        public Asteridian3DScrollBase(Rectangle region, ScrollLayerBase mainLayer) 
            : base(region, mainLayer)
        {
        }

        /// <inheritdoc/>
        public abstract int GetLevelHeight();
    }
}
