using Kriptok.Adventure.Entities.Monsters;
using Kriptok.Adventure.Entities.Player;
using Kriptok.Adventure.Extensions;
using Kriptok.Common;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Partitioned;
using Kriptok.Extensions;
using Kriptok.Mapping.Entities;
using Kriptok.Mapping.Grid;
using Kriptok.Mapping.Tiles;
using Kriptok.Mapping.Tiles.Base;
using Kriptok.Mapping.Tiles.Editor;
using Kriptok.Regions;
using Kriptok.Regions.Scroll;
using Kriptok.Scenes;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Kriptok.Adventure.Scenes.Base
{
    public abstract class ScrollMapSceneBase<TTileset> : MapSceneBase
        where TTileset : TilesetBase, new()
    {
        private readonly Resource map;

        protected ScrollMapSceneBase(Resource map)
        {
            this.map = map;
        }

        protected sealed override void Run(SceneHandler h)
        {
            var scroll = h.StartScroll(new TileScrollRegion<TTileset>(h.ScreenRegion.Rectangle, map));

            var lina = h.Add(scroll, new Lina2D(scroll)
            {
                Location = new Vector3F(8 * 5 + 8-1, 16 * 5 + 8, 0f)
            });

            scroll.SetTarget(lina);

            var handler = new ScrollMapHandler(h, scroll, lina);

            foreach(var itemX in scroll.EntitiesX.Item2)
            {
                scroll.EntitiesX.Item1.AddEntity(scroll, itemX, c => c.SetHandler(handler));                
            }

            Run(handler);
        }

        protected abstract void Run(ScrollMapHandler handler);

        internal class TileScrollRegion<T> : TileScrollRegion
            where T : TilesetBase, new()
        {
            public TileScrollRegion(Rectangle region, Resource resource)
                : base(region, resource, new T())
            {
            }
        }
    }

    internal class TileScrollRegion : RpgTiledScrollRegion
    {
        private const float initialTileSpeedAnimation = 0.006125f / Consts.SpeedMultiplier;

        public Tuple<EntitySet, TileMapEntityX[]> EntitiesX { get; }

        public TileScrollRegion(Rectangle region, Resource resource, TilesetBase tileSet)
            : this(region, GetMapInfo(resource, tileSet))
        {            
        }

        public TileScrollRegion(Rectangle region, TileMapInfo tileMapInfo)
            : base(region, tileMapInfo)
        {
            TileAnimatedSpeed = initialTileSpeedAnimation;            
            EntitiesX = new Tuple<EntitySet, TileMapEntityX[]>(tileMapInfo.Tileset.Entities, tileMapInfo.Entities);
        }

        private static TileMapInfo GetMapInfo(Resource resource, TilesetBase tileSet)
        {
            var tileMap = TileMapX.Load(resource);
            var mapInfo = new TileMapInfo(tileMap, tileSet);
            return mapInfo;
        }
    }

    public class ScrollMapHandler : ILocationValidatorProvider<ITileScrollEntity>
    {
        private readonly SceneHandler h;
        private readonly TileScrollRegion scroll;

        internal ScrollMapHandler(SceneHandler h, TileScrollRegion scroll, Lina2D lina)
        {
            this.h = h;
            this.scroll = scroll;
            this.Player = lina;
        }

        public readonly Lina2D Player;

        internal T Add<T>(T entity)
            where T : EntityBase
        {
            return h.Add(scroll, entity);
        }

        public ILocationValidator GetLocationValidator(ITileScrollEntity param)
        {
            return ((ILocationValidatorProvider<ITileScrollEntity>)scroll).GetLocationValidator(param);
        }
    }

}
