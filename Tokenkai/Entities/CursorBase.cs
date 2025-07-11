using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenkai.Entities
{
    internal abstract class CursorBase : EntityBase<SpriteView>
    {
        public CursorBase() : base(new SpriteView(typeof(CursorBase).Assembly, "Assets.Menu.Mouse.png")
        {
            Center = new PointF(1 / 15f, 1 / 15f)
        })
        {
            Location.Z = -9999f;
        }

        protected override void OnFrame()
        {
            Location.X = Mouse.X;
            Location.Y = Mouse.Y;

            // if(selectedOption.HasValue)
            // {
            //     Trace.WriteLine(selectedOption.Value);
            // }
        }
    }
}
