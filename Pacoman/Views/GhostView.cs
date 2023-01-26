using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacoman.Views
{
    public class GhostView : IndexedSpriteView
    {
        public GhostView(int model) : base(typeof(GhostView).Assembly, $"Assets.Images.Ghost{model}.png", 2, 2)
        {
            base.Add(typeof(GhostView).Assembly, $"Assets.Images.GhostScared.png", 0, 0, 0, 0, 2, 1);
        }
    }
}
