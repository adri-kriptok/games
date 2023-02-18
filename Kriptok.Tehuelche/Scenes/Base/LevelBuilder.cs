using Kriptok.Core;
using Kriptok.Regions.Scroll;
using Kriptok.Scenes;
using Kriptok.Tehuelche.Entities;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Tehuelche.Entities.Hud;
using Kriptok.Tehuelche.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Tehuelche.Scenes.Base
{
    internal class LevelBuilder
    {
        private readonly SceneHandler h;
        public LevelBuilder(PlayerHelicopterBase player,
            SceneHandler handler, ITerrain terrain, ScrollRegion minimap)
        {
            this.h = handler;
            Player = player;
            Terrain = terrain;
            Minimap = minimap;
        }

        public ITerrain Terrain { get; }

        public ScrollRegion Minimap { get; }

        public PlayerHelicopterBase Player { get; }

        internal void Add(MinimapEnemy enemy) => h.Add(Minimap, enemy);

        internal void Add(EnemyBase enemy) => h.Add((RegionBase)Terrain, enemy);
    }
}
