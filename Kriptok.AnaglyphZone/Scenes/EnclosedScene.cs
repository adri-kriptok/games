using Kriptok.Core;
using Kriptok.Scenes;
using Kriptok.AZ.Entities;
using Kriptok.AZ.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.AZ.Scenes
{
    internal partial class EnclosedScene : SceneBase
    {
        protected override void Run(SceneHandler h)
        {
            var playArea = h.ScreenRegion.Rectangle;
            
            var region = h.StartPseudo3D(new Vector3DRegion(playArea));
            region.Ambience.SetLightSource(1f, 0f, 0f);
            region.Ambience.AbsoluteLight = true;

            var camTarget = h.Add(region, new CamTarget());
            var player = h.Add(region, new PlayerShip(camTarget));

            region.SetCamera(new Cam(camTarget, player));


            var startGrid = 0;
            var repeatPattern = startGrid + 6;
            // -----------------------------------------------
            h.Add(region, new Grid(camTarget, startGrid));

            h.Add(region, new Column(camTarget, startGrid + 0, -6, repeatPattern));
            h.Add(region, new Column(camTarget, startGrid + 0, 2, repeatPattern));

            h.Add(region, new Column(camTarget, startGrid + 4, -3, repeatPattern));
            h.Add(region, new Column(camTarget, startGrid + 4, 1, repeatPattern));

            h.Add(region, new Column(camTarget, startGrid + 8, -6, repeatPattern));
            h.Add(region, new Column(camTarget, startGrid + 8, 2, repeatPattern));

            h.Add(region, new Beam(camTarget,  startGrid + 16, 4, 1, repeatPattern));
            h.Add(region, new Beam(camTarget, startGrid + 16, 1, 1, repeatPattern));

            //h.Add(region, new Beam(camTarget, 12, 4.5f, 2));
            //h.Add(region, new Beam(camTarget, 12, 0.5f, 2));
        }

        private class CamTarget : CamTargetBase
        {
        }
    }
}
