using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Scenes;
using Kriptok.AZ.Entities;
using Kriptok.AZ.Entities.Enemies;
using Kriptok.AZ.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Kriptok.Vector3D.Extensions;
using Kriptok.IO;
using System.Collections.Specialized;
using Kriptok.Vector3D.Scenes;

namespace Kriptok.AZ.Scenes
{
    public enum GameMessages
    {
        Died = 0
    }

    internal class StarsScene : SceneBase
    {
        private CamTarget2 camTarget;
        private StarsRegion region;

        protected override void Run(SceneHandler h)
        {
            
            var playArea = h.ScreenRegion.Rectangle;
            //playArea.Y = 12;
            //playArea.Height -= 12;

            region = h.StartPseudo3D(new StarsRegion(playArea));
            region.Ambience.SetLightSource(1f, 0f, 0f);

            camTarget = h.Add(region, new CamTarget2());
            var player = h.Add(region, new PlayerShip(camTarget));

            region.SetCamera(new Cam(camTarget, player));

            // -----------------------------------------------
            // h.Add(region, new Stars(camTarget));

            // h.Add(region, new AsteroidsMessage(camTarget));

            h.WriteScoreboard();

            h.FadeOn();
        }

        protected override void OnMessage(SceneHandler h, object message)
        {
            base.OnMessage(h, message);

            if (message is GameMessages e)
            {
                if (e == GameMessages.Died)
                {
                    h.Wait(2000);
                    h.Add(region, new GameOver(camTarget));
                    h.Wait(2000);
                    h.FadeOff();
                    Global.Record = MaxScore.CheckAndSave(Global.Score);
                    h.Set(new TitleScene());
                }
            }
        }

        private class CamTarget2 : CamTargetBase
        {
            private int counter = 0;
            private float arcadeIncreaser = 1f;

            protected override void OnFrame()
            {
                base.OnFrame();

                counter++;

                if (counter >= 22)
                {
                    arcadeIncreaser += 0.001f;
                    Add(new Asteroid(this, arcadeIncreaser));
                    counter = 0;
                }
            }
        }        
    }
}
