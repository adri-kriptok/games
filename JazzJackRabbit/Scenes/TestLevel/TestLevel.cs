using Kriptok.Drawing;
using Kriptok.JazzJackRabbit.Maps;
using Kriptok.JazzJackRabbit.Scenes.Base;
using Kriptok.Mapping.Tiles;

namespace Kriptok.JazzJackRabbit.Scenes.TestLevel
{
    class TestLevel : LevelSceneBase
    {
        internal override Vector2I GetPlayerInitialCoords() => new Vector2I(8, 5);

        internal override TileMapInfo GetTileMap()
        {            
            return new TileMapInfo<DiamondusTileset>(Assembly, $"{GetType().Namespace}.Map.mapx");            
        }
    }
}
