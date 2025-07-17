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

        protected override void OnLocation(int y)
        {            
        }

        protected override LayeredScrollRegionBase StartScroll(SceneHandler h, Rectangle rectangle)
        {
            return h.StartScroll(new OpenSpaceScrollRegion(rectangle));
        }
    }
}
