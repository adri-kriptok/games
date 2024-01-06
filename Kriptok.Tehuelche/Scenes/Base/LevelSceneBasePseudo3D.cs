using Kriptok.Audio;
using Kriptok.Common;
using Kriptok.Extensions;
using Kriptok.Mapping.VoxelSpace;
using Kriptok.Mapping.Terrains;
using Kriptok.Scenes;
using Kriptok.Tehuelche.Entities;
using Kriptok.Tehuelche.Entities.Hud;
using Kriptok.Tehuelche.Regions;
using System.Drawing;

namespace Kriptok.Tehuelche.Scenes.Base
{
    internal enum PlayerMessagesEnum
    {
        //Hit = 0
    }

    internal abstract class LevelSceneBasePseudo3D : SceneBase
    {
        private const float mapScale = 1f;

        protected sealed override void Run(SceneHandler h)
        {
            // Uso toda la pantalla, aunque haya una parte oculta por el HUD.
            var rect = h.ScreenRegion.Rectangle;

            // Cargo el terreno.
            var texture = GetTexture();
            var terrain = VoxelTerrain.Create(texture, GetTerrain(), mapScale);

            // Creo el entorno de juego.
            var region = h.StartPseudo3D(new TehuelcheMapRegionPseudo3D(rect, terrain, GetBackground())
            {
                //TextureScale = 0.5f
                TextureScale = mapScale
            });
            region.Ambience.SetLightSource(1f, 4f, 2f);

            var player = h.Add(region, new PlayerHelicopterPseudo3D(region, 810f, 1560f));
            region.SetCamera(new PlayerCam(player));            


            var minimap = h.StartScroll(new MinimapScrollRegion(new Rectangle(68, 132, 80, 54), texture));
            
            minimap.SetTarget(h.Add(minimap, new MinimapPlayer(player, region.TextureScale)));

            h.Add(new Hud());

            Run(new LevelBuilder(player, h, region, minimap));

            h.PlayMidiNote(MidiInstrumentEnum.Helicopter, 0, 51, 111);
        }

        protected abstract void Run(LevelBuilder builder);

        protected abstract ByteTerrainData GetTerrain();

        protected abstract Resource GetTexture();

        protected abstract Resource GetBackground();

        protected override void OnMessage(SceneHandler h, object message)
        {
            base.OnMessage(h, message);

            if (message is PlayerMessagesEnum msg)
            {
                //if (msg == PlayerMessagesEnum.Hit)
                //{
                //    h.FadeFrom(Color.Red.SetAlpha(128), 4);
                //}
            }
        }
    }
}