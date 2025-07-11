using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolido.Entities
{
    internal class Flag : EntityBase<IndexedSpriteView>
    {
        private int graphCounter = 0;
        private bool bGraph;

        public Flag() : base(new IndexedSpriteView(typeof(Flag).Assembly, "Assets.Flag.png", 2, 1))
        {
        }

        protected override void OnFrame()
        {
            if ((graphCounter += 1) == 50)
            {
                bGraph = !bGraph;
                graphCounter = 0;
            }

            View.Graph = bGraph ? 0 : 1;
        }
    }
}
