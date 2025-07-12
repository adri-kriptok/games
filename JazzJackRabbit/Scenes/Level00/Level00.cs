using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.JazzJackRabbit.Maps;
using Kriptok.JazzJackRabbit.Scenes.Base;
using Kriptok.Mapping.Tiles;

namespace Kriptok.JazzJackRabbit.Scenes.Level00
{
    class Level00 : LevelSceneBase
    {
        internal override Vector2I GetPlayerInitialCoords() => new Vector2I(7, 185);        

        internal override TileMapInfo GetTileMap()
        {            
            return new TileMapInfo<DiamondusTileset>(Assembly, $"{GetType().Namespace}.Map.mapx");            
        }
    }
}
