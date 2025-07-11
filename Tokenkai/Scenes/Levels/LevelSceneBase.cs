using Kriptok.Core;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Queries;
using Kriptok.Helpers;
using Kriptok.Mapping.Grid;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Axonometric;
using Kriptok.Regions.Scroll.Axonometric.Base;
using Kriptok.Regions.Scroll.Base;
using Kriptok.Scenes;
using Kriptok.Views.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Tokenkai.Entities;

namespace Tokenkai.Scenes
{
    internal abstract class LevelSceneBase : SceneBase
    {
        private readonly int level;
        private GameCursor cursor;
        private TileMapGrid map;
        private Player player;

        public LevelSceneBase(int level)
        {
            this.level = level;
        }

        protected override void Run(SceneHandler h)
        {
            var scroll = h.StartAxonometric(new TokenkaiScroll(level));
            this.map = TileMapGrid.From(Assembly, $"Assets.Level{level}.005.png", TokenkaiConsts.TileSize);
            this.player = h.Add(scroll, new Player(map, GetStartLocation()));
            scroll.SetTarget(new ScrollCam(player));
            this.cursor = h.Add(new GameCursor(player));
        }

        protected abstract Point GetStartLocation();

        private class TokenkaiScroll : FixedAxonometricRegionLayered, IAxonometricScroll
        {
            private const float angleIncr = MathHelper.TwoPIF / 360f;
            private readonly GdipBrushScrollLayer secondLayer;
            private float angle = 0f;

            public TokenkaiScroll(int level) : this(TokenkaiConsts.PlayableArea, CreateLayer(level))
            {
                secondLayer = AddLayer(new GdipBrushScrollLayer(Assembly, $"Assets.Level{level}.004.png", true, true)
                {
                    Priority = -999999
                });
            }

            public TokenkaiScroll(Rectangle region, FixedGdipImageScrollLayer mainLayer) : base(region, mainLayer)
            {                
                mainLayer.MakeTransparent();
            }

            private static FixedGdipImageScrollLayer CreateLayer(int level)
            {
                return new FixedGdipImageScrollLayer(typeof(TokenkaiScroll).Assembly, $"Assets.Level{level}.003.png", false, false);
            }

            protected override void Render(ScrollRenderContextBase context, IEnumerable<IRenderizable> views)
            {
                base.Render(context, views);

                angle += angleIncr;
                secondLayer.Location.X = PolarVector.ProjectX(angle, 24f);
                secondLayer.Location.Y = PolarVector.ProjectY(angle, 12f);
            }
        }

        private class GameCursor : CursorBase
        {
            private readonly Player player;

            public GameCursor(Player player)
            {
                this.player = player;
            }

            protected override void OnFrame()
            {
                base.OnFrame();

                // player.PointToMouse();

                if (Mouse.Left)
                {
                    if (TokenkaiConsts.PlayableArea.Contains((int)Location.X, (int)Location.Y))
                    {
                        player.MoveToCursor();
                    }
                }
            }
        }
    }
}