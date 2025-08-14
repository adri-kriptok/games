using Kriptok.Asteridian.Entities.Enemies;
using Kriptok.Asteridian.Regions;
using Kriptok.Asteridian.Scenes.Base;
using Kriptok.Extensions;
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

        protected override void LoadLevelEvents(LevelEventContext context)
        {
            // int amount = 15;
            int amount = 1500;

            context.Wait(3000);

            var rnd = new Random(2);
            for (int i = 0; i < amount; i++)
            {
                var x = ((rnd.NextFloat() * 2f) - 1f) * context.ScreenWidth * 0.5f;

                // context.Enqueue(2500 - i * 150, new Asteroid(x, ((float)Math.Abs(Math.Cos(i))) * 0.0625f));
                context.Enqueue(250, new AsciiInvader(x));
            }
        }
    }
}
