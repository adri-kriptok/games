using Kriptok.Drawing;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.JazzJackRabbit.Maps.Base;
using Kriptok.Mapping.Tiles;
using System.Drawing;
using System.Linq;

namespace Kriptok.JazzJackRabbit.Maps
{
    public class DiamondusTileset : JazzTilesetBase
    {
        public DiamondusTileset()
        {
            // Paleta original del recurso.
            Color[] basePalette = null;

            // Rotación de paletas para animaciones.
            PaletteSwap blueRotationPalette = null;
            PaletteSwap orangeRotationPaletteRight = null;
            
            BitmapHelper.UsingBitmap(Assembly, "Assets.Tilesets.Diamondus.png", bmp =>
            {
                // Guardo la paleta para realizar rotaciones después.
                basePalette = bmp.Palette.Entries;

                // Rotación de colores para las cascadas.
                blueRotationPalette = new PaletteSwap();
                blueRotationPalette.Add(basePalette[117], basePalette[116]);
                blueRotationPalette.Add(basePalette[118], basePalette[117]);
                blueRotationPalette.Add(basePalette[119], basePalette[118]);
                blueRotationPalette.Add(basePalette[120], basePalette[119]);
                blueRotationPalette.Add(basePalette[121], basePalette[120]);
                blueRotationPalette.Add(basePalette[122], basePalette[121]);
                blueRotationPalette.Add(basePalette[123], basePalette[122]);
                blueRotationPalette.Add(basePalette[116], basePalette[123]);

                orangeRotationPaletteRight = new PaletteSwap();
                orangeRotationPaletteRight.Add(basePalette[115], basePalette[114]);
                orangeRotationPaletteRight.Add(basePalette[114], basePalette[113]);
                orangeRotationPaletteRight.Add(basePalette[113], basePalette[112]);
                orangeRotationPaletteRight.Add(basePalette[112], basePalette[115]);
                
                // Hago transparente el bitmap.
                // Este proceso hace que deje de ser un BMP de 8bpp.
                bmp.MakeTransparent();

                using (var res = FastBitmap.CreateFrom(bmp))
                {
                    Layer0.AddRange(GetLayer0Tiles(res));
                    Layer1.AddRange(GetLayer1Tiles(res));

                    Layer1.SetEmptyTile(0);
                }
            });

            for (int i = 24; i < 40; i++)
            {
                Layer0.GetTile(i).Block();
            }

            Layer0.GetTile(45).Block();
            Layer0.GetTile(46).Block();
            Layer0.GetTile(47).Block();
            Layer0.GetTile(84).Block();

            Layer0.GetTile(70).Block();
            Layer0.GetTile(71).Block();
            Layer0.GetTile(78).Block();
            Layer0.GetTile(79).Block();

            Layer0.GetTile(81).AddFlag(TileFlagsEnum.BlockedFromTop | JazzTilesetCustomFlags.BlockedToTop);
            Layer0.GetTile(64).AddFlag(TileFlagsEnum.BlockedFromTop | JazzTilesetCustomFlags.BlockedToTop);

            Layer1.GetTile(56).Block();
            Layer1.GetTile(72).Block(); Layer1.GetTile(73).Block();
            Layer1.GetTile(74).Block(); Layer1.GetTile(76).Block();
            Layer1.GetTile(62).Block(); Layer1.GetTile(63).Block();
            Layer1.GetTile(70).Block(); Layer1.GetTile(71).Block();
            Layer1.GetTile(78).Block(); Layer1.GetTile(79).Block();

            Layer1.GetTile(80).Block(); Layer1.GetTile(81).Block();
            Layer1.GetTile(82).Block(); Layer1.GetTile(83).Block();
            Layer1.GetTile(85).Block(); Layer1.GetTile(86).Block();

            Layer1.GetTile(97).Block(); Layer1.GetTile(98).Block();
            Layer1.GetTile(99).Block(); Layer1.GetTile(100).Block();

            // Diagonales

            Layer0.GetTile(93).AddSlopeFlag(JazzTilesetCustomFlags.DiagonalUp);
            Layer0.GetTile(94).AddSlopeFlag(JazzTilesetCustomFlags.DiagonalDown);
            Layer0.GetTile(101).AddSlopeFlag(JazzTilesetCustomFlags.DiagonalUp);
            Layer0.GetTile(102).AddSlopeFlag(JazzTilesetCustomFlags.DiagonalDown);

            Layer1.GetTile(75).AddSlopeFlag(JazzTilesetCustomFlags.DiagonalUp);            
            Layer1.GetTile(77).AddSlopeFlag(JazzTilesetCustomFlags.DiagonalDown);
            Layer1.GetTile(84).AddSlopeFlag(JazzTilesetCustomFlags.DiagonalUp);
            Layer1.GetTile(87).AddSlopeFlag(JazzTilesetCustomFlags.DiagonalDown);
            Layer1.GetTile(96).AddSlopeFlag(JazzTilesetCustomFlags.DiagonalUp);
            Layer1.GetTile(101).AddSlopeFlag(JazzTilesetCustomFlags.DiagonalDown);

            Layer1.GetTile(115).AddFlag(TileFlagsEnum.OverlayLayer);
            Layer1.GetTile(123).AddFlag(TileFlagsEnum.OverlayLayer);

            Layer1.GetTile(16).AddFlag(JazzTilesetCustomFlags.BlockedToBottom);
            
            // Punta del pino.
            Layer1.GetTile(20).AddFlag(JazzTilesetCustomFlags.BlockedToBottom);
            Layer1.GetTile(27).AddFlag(JazzTilesetCustomFlags.DiagonalUp);
            Layer1.GetTile(29).AddFlag(JazzTilesetCustomFlags.DiagonalDown);

            Layer1.GetTile(57).AddFlag(TileFlagsEnum.BlockedFromTop | JazzTilesetCustomFlags.BorderBlocked);
            Layer1.GetTile(58).AddFlag(TileFlagsEnum.BlockedFromTop | JazzTilesetCustomFlags.BorderBlocked);
            Layer1.GetTile(59).AddFlag(TileFlagsEnum.BlockedFromTop | JazzTilesetCustomFlags.BorderBlocked);
            Layer1.GetTile(60).AddFlag(TileFlagsEnum.BlockedFromTop | JazzTilesetCustomFlags.BorderBlocked);

            Layer1.GetTile(92).AddFlag(TileFlagsEnum.BlockedFromTop);
            Layer1.GetTile(93).AddFlag(TileFlagsEnum.BlockedFromTop);
            Layer1.GetTile(94).AddFlag(TileFlagsEnum.BlockedFromTop);
            Layer1.GetTile(95).AddFlag(TileFlagsEnum.BlockedFromTop);

            Map2DTileInfo[] GetLayer0Tiles(FastBitmap res)
            {
                return base.GetBasicTiles(res, (x, y) => x < 8).Select((p, i) =>
                {
                    if (i.In(25, 26, 72, 80, 84, 74, 75, 81, 82, 83, 21))
                    {
                        return new PaletteRotationTile(p, blueRotationPalette);
                    }

                    if (i.In(90, 20, 22))
                    {
                        return new PaletteRotationTile(p, orangeRotationPaletteRight);
                    }

                    return p;
                }).ToArray();
            }

            Map2DTileInfo[] GetLayer1Tiles(FastBitmap res)
            {
                return base.GetBasicTiles(res, (x, y) => x >= 8).Select((p, i) =>
                {
                    if (i.In(112, 120, 113, 114, 115, 123))
                    {
                        return new PaletteRotationTile(p, blueRotationPalette);
                    }

                    if (i.In(1, 2, 9, 10, 12))
                    {
                        return new PaletteRotationTile(p, orangeRotationPaletteRight);
                    }

                    return p;
                }).ToArray();
            }
        }

        protected override void ConfigureMapEditor(MapEditorTilesetConfig config)
        {
            base.ConfigureMapEditor(config);

            config.PaletteTilesScale = 1f;
            config.PaletteTilesCount = 8;
        }

    }

    internal static class TileExtensions
    {
        public static void AddSlopeFlag(this Map2DTileInfo tile, TileFlagsEnum flag)
        {
            tile.AddFlag(flag);
            tile.AddFlag(TileFlagsEnum.BlockedFromBottom);

            if (flag == JazzTilesetCustomFlags.DiagonalDown)
            {
                tile.AddFlag(TileFlagsEnum.BlockedFromLeft);
            }

            if (flag == JazzTilesetCustomFlags.DiagonalUp)
            {
                tile.AddFlag(TileFlagsEnum.BlockedFromRight);
            }
        }
    }
}
