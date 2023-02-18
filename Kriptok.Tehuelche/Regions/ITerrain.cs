using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Tehuelche.Regions
{
    internal interface ITerrain
    {
        float GetHeight(Vector2F location);

        BoundF2 GetPlayArea();
    }
}
