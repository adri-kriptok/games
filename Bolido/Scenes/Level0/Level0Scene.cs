using Bolido.Entities;
using Bolido.Scenes.Base;
using Kriptok.Drawing.Algebra;
using Kriptok.Regions.Scroll;
using Kriptok.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolido.Scenes.Level0
{
    internal class Level0Scene : LevelSceneBase
    {
        public Level0Scene() : base($"{typeof(Level0Scene).Namespace}.Map.bolidox")
        {            
        }


        protected override void Run(SceneHandler h, TileScrollFixedRegion scroll)
        {
            AppendFlag(h, scroll, 200f, 130f);
            AppendFlag(h, scroll, 200f, 130f);
            AppendFlag(h, scroll, 860f, 150f);
            AppendFlag(h, scroll, 860f, 150f);
            AppendFlag(h, scroll, 420f, 250f);
            AppendFlag(h, scroll, 420f, 250f);
            AppendFlag(h, scroll, 330f, 190f);
            AppendFlag(h, scroll, 330f, 190f);
            AppendFlag(h, scroll, 760f, 370f);
            AppendFlag(h, scroll, 760f, 370f);
            AppendFlag(h, scroll, 280f, 490f);
            AppendFlag(h, scroll, 280f, 490f);
            AppendFlag(h, scroll, 880f, 540f);
            AppendFlag(h, scroll, 880f, 540f);
            AppendFlag(h, scroll, 010f, 850f);
            AppendFlag(h, scroll, 010f, 850f);
            AppendFlag(h, scroll, 410f, 1040f);
            AppendFlag(h, scroll, 410f, 1040f);
            AppendFlag(h, scroll, 820f, 1290f);
            AppendFlag(h, scroll, 820f, 1290f);
        }

        private void AppendFlag(SceneHandler h, TileScrollFixedRegion scroll, float x, float y)
        {
            h.Add(scroll, new Flag()
            {
                Location = new Vector3F(x + 12, y + 24, 1f)
            });
        }
    }
}
