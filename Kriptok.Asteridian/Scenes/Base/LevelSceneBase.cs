using Kriptok.Audio;
using Kriptok.Core;
using Kriptok.Drawing.Algebra;
using Kriptok.Regions.Scroll.Base;
using Kriptok.Regions.Scroll;
using Kriptok.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kriptok.Asteridian.Entities;
using Kriptok.Extensions;
using Kriptok.Asteridian.Regions;

namespace Kriptok.Asteridian.Scenes.Base
{
    /// <summary>
    /// Esta clase representa los comportamientos básicos de todos los niveles del juego.
    /// </summary>
    public abstract class LevelSceneBase : SceneBase
    {

        internal Player PlayerShip;
        private ScrollTarget target;
        protected float fSpeedInPixels = 0.5f;
        protected int iSpeedInPixels = 1;
        private LayeredScrollRegionBase scroll = null;

        public LevelSceneBase()
        {
        }

        protected abstract string GetLevelName();

        // protected abstract PlayMusicOptions GetMusicOptions();

        protected override void Run(SceneHandler h)
        {
            var levelRegion = h.ScreenRegion.Rectangle;
            levelRegion.Width = levelRegion.Width * 13 / 16;
            Init(levelRegion);

            scroll = StartScroll(h, levelRegion);

            // h.PlayMusic(GetMusicOptions());

            scroll.Ambience.SetLightSource(-0.33f, 0.66f, 1f);

            OnStartingLevel();

            PlayerShip = h.Add(scroll, new Player(this));

            this.target = new ScrollTarget(scroll.Rectangle, PlayerShip, ((IAsteridianScroll)scroll).GetLevelHeight());
            scroll.SetTarget(target);

            h.FadeOn();
            int i = 0;

#if DEBUG
            var start = DateTime.Now;
            h.ResetTimer();
#endif

            h.While(() => target.KeepMoving(), () =>
            {
                OnLocation(i);
                i += iSpeedInPixels;
            });

#if DEBUG
            var end = DateTime.Now - start;
#endif

            //while (Location.Y < maxY)
            //{
            //    Location.X = PlayerShip.UpdateY().Round();

            //    Frame();
            //}
        }

        protected abstract LayeredScrollRegionBase StartScroll(SceneHandler h, Rectangle rectangle);

        /// <summary>
        /// Inicializa variables importantes para el desarrollo del juego.
        /// </summary>        
        private static void Init(Rectangle region)
        {
            GlobalConsts.ShotBase_MinX = -50f;
            GlobalConsts.ShotBase_MinY = -50f;
            GlobalConsts.ShotBase_MaxX = region.Size.Width + 100f;
            GlobalConsts.ShotBase_MaxY = region.Size.Height + 50f;

            GlobalConsts.PlayerShip_MinX = 100; // 50 + 50 + 100 - 25 - 25- 5;
            GlobalConsts.PlayerShip_MaxX = 700; // (950f / 800f * Screen.Size.Width).Round() + 25 + 25 + 50;
            GlobalConsts.PlayerShip_MidX = (GlobalConsts.PlayerShip_MaxX - GlobalConsts.PlayerShip_MinX) / 2 + GlobalConsts.PlayerShip_MinX;

            //GlobalConsts.PlayerShip_MinModifierY = 50;
            //GlobalConsts.PlayerShip_MaxModifierY = (550f / 450f * Screen.Size.Height).Round();

            GlobalConsts.PlayerShip_MinModifierY = -(region.Size.Height / 2) + 35;
            GlobalConsts.PlayerShip_MaxModifierY = (region.Size.Height / 2) - 30;

            GlobalConsts.MouseMinX = (816 - region.Size.Width) / 2;
        }

        internal float GetLocationY() => target.GetLocation2D().Y;

        protected virtual void OnStartingLevel()
        {
        }

        protected abstract void OnLocation(int y);

        protected void SetViewProperties(RegionBase region, int totalWidth, int totalHeight)
        {


        }

        //internal Vector2F CalculateLocation(ObjectBase obj)
        //{
        //    return scroll.CalculateScreenCoords(obj.Location.X, obj.Location.Y);
        //}

        private class ScrollTarget : IScrollTarget
        {
            private readonly float maxY;
            private readonly float minY;

            private readonly Player playerShip;
            private Vector2F location;

            public ScrollTarget(Rectangle region, Player playerShip, int levelHeight)
            {
                this.playerShip = playerShip;

                minY = region.Size.Height / 2;
                maxY = levelHeight - region.Size.Height / 2;

                location.X = region.Size.Width / 2;
                location.Y = maxY;
            }

            public Vector2F GetLocation2D() => location;

            internal bool KeepMoving()
            {
                var keepMoving = location.Y > minY;

                if (keepMoving)
                {
                    location.Y -= 1f;
                    location.X = playerShip.UpdateY().Round();
                }

                return keepMoving;
            }
        }
    }
}
