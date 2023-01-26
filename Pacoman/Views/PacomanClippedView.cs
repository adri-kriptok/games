using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Collisions;
using Kriptok.Views;
using Kriptok.Views.Base;
using Kriptok.Views.Gdip;
using Kriptok.Views.Gdip.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacoman.Views
{
    public class PacomanClippedView : ClippedIndexedSpriteView
    {
        public PacomanClippedView(IndexedSpriteView view) 
            : base(view, new Rectangle(105, 0, 431, 480))
        {
        }
    }
}
