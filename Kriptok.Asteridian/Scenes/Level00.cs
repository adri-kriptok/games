using Kriptok.Asteridian.Entities.Enemies;
using Kriptok.Asteridian.Regions;
using Kriptok.Asteridian.Scenes.Base;
using Kriptok.Regions.Scroll.Base;
using Kriptok.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Scenes
{
    class Level00 : LevelSceneBase
    {
        protected override string GetLevelName()
        {
            throw new NotImplementedException();
        }

        protected override LayeredScrollRegionBase StartScroll(SceneHandler h, Rectangle rectangle)
        {
            return h.StartScroll(new OpenSpaceScrollRegion(this, rectangle));
        }

        protected override void GetEventList(LevelEventList list)
        {
            for (int i = 0; i < 300; i++)
            {
                int x = ((i * 1366)) % 321 - 153;
                list.Enqueue(500, new Asteroid(x, ((float)Math.Abs(Math.Cos(i))) * 0.0625f));
            }
        }
    }
}
