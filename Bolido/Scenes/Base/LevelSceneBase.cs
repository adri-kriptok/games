using Bolido.Entities;
using Bolido.Mapping;
using Kriptok.Audio;
using Kriptok.Mapping.Grid;
using Kriptok.Mapping.Tiles;
using Kriptok.Mapping.Tiles.Editor;
using Kriptok.Regions.Scroll;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolido.Scenes.Base
{
    internal abstract class LevelSceneBase : SceneBase
    {
        private readonly string fileName;
        private TileMap tileMap;

        public LevelSceneBase(string fileName)
        {
            this.fileName = fileName;
        }

        sealed protected override void Run(SceneHandler h)
        {
            var map = new TileMapInfo<Tileset>(TileMapX.Load(Assembly, fileName));
            var scroll = h.StartScroll(new TileScrollFixedRegion(h.ScreenRegion.Rectangle, map));

            // Guardo el mapa generado para utlizar después.
            this.tileMap = scroll.BaseLayerMap;

            scroll.TileAnimatedSpeed = 0.1f;

            scroll.AddLayer(new GdipBrushScrollLayer(Assembly, "Assets.Space.png", true, true)
            {
                ScaleX = 2f,
                ScaleY = 2f,
                ReScaleX = 0.5f,
                ReScaleY = 0.5f,
                Priority = -99999
            });

            scroll.SetTarget(h.Add(scroll, new Player(this)));

            Run(h, scroll);
        }

        protected abstract void Run(SceneHandler h, TileScrollFixedRegion scroll);

        internal TileFlagsEnum GetTileFlegs(float x, float y) => tileMap.SampleTileFlags(x, y);        
    }
}
