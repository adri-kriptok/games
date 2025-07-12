using Kriptok.Common;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Kriptok.JazzJackRabbit.Entities;
using Kriptok.JazzJackRabbit.Maps;
using Kriptok.Mapping.Tiles;
using Kriptok.Regions.Scroll;
using Kriptok.Scenes;
using System;
using System.Drawing;

namespace Kriptok.JazzJackRabbit.Scenes.Base
{
    abstract class LevelSceneBase : SceneBase
    {
        private TileMap tiledMap;
        private int mapWidth;
        private int mapHeight;

        /// private TiledMapGrid mapGrid;

        protected override void Run(SceneHandler h)
        {
            var playRectangle = new Rectangle(0, 0, h.ScreenRegion.Rectangle.Width, h.ScreenRegion.Rectangle.Height - 32);

            // Inicio el scroll.
            var scroll = h.StartScroll(new TileScrollFixedRegion(playRectangle, GetTileMap()));

            scroll.TileAnimatedSpeed = 0.5f;

            // Cargo el mapa.
            this.tiledMap = scroll.BaseLayerMap;
            this.mapWidth = tiledMap.FullSize.Width;
            this.mapHeight = tiledMap.FullSize.Height;
            // // Genero la grilla de "durezas"
            // this.mapGrid = TiledMapGrid.From(tiledMap);

            scroll.AddLayer(new GdipBrushScrollLayer(Assembly, "Assets.Back.Blue.png", true, true)
            {
                Scale = new Vector2F(2f, 2f),
                ReScale = new Vector2F(0.5f, 0.5f),
                Priority = int.MinValue
            });

            var location = GetPlayerInitialCoords();
            var player = h.Add(scroll, new Jazz(this)
            {
                Location = new Vector3F(
                    location.X * JazzConsts.TileSize + JazzConsts.HalfTileSize,
                    location.Y * JazzConsts.TileSize + JazzConsts.HalfTileSize, 0)
            });

            scroll.SetTarget(player);

            h.Add(new Hud(player, playRectangle.Height));
        }

        internal abstract Vector2I GetPlayerInitialCoords();

        internal abstract TileMapInfo GetTileMap();

        internal int GetLowerPlatformY(float x, float y0)
        {
            // {
            //     // Resto 1 para contemplar que se cruce un tile que está bloqueado hacia abajo.
            //     // Recordar cuando se salta a la copa del árbol finito de Diamondus.
            //     var y_12 = y0 - 0.01f;
            // 
            //     var t = tiledMap.GetTileFlags(x, y_12);
            //     if (t.HasFlag(JazzTilesetCustomFlags.BlockedToBottom))
            //     {
            //         return ((int)(y_12 * JazzConsts.InvTileSize) + 1) * JazzConsts.TileSize;
            //     }
            //     else if (t.HasFlag(JazzTilesetCustomFlags.DiagonalDown))
            //     {
            //         return ((int)(y_12 * JazzConsts.InvTileSize)) * JazzConsts.TileSize + ((int)x) % JazzConsts.TileSize;
            //     }
            //     else if (t.HasFlag(JazzTilesetCustomFlags.DiagonalUp))
            //     {
            //         return (((int)(y_12 * JazzConsts.InvTileSize)) + 1) * JazzConsts.TileSize - ((int)x) % JazzConsts.TileSize;
            //     }
            // }
            {
                 var y = Math.Max(y0 - 0.01f, 0f);

                for (; y < mapHeight; y += JazzConsts.TileSize)
                {
                    var t = tiledMap.SampleTileFlags(x, y);
                    
                    if (t.HasFlag(JazzTilesetCustomFlags.BlockedToBottom))
                    {
                        return ((int)(y * JazzConsts.InvTileSize) + 1) * JazzConsts.TileSize;
                    }
                    else if (t.HasFlag(TileFlagsEnum.BlockedFromTop))
                    {
                        var lower = ((int)(y * JazzConsts.InvTileSize)) * JazzConsts.TileSize;

                        if (t.HasFlag(JazzTilesetCustomFlags.BorderBlocked))
                        {
                            // Esto es para contemplar que por ahí sólo la parte de arriba
                            // del casillero deba ser tomada en cuenta.
                            if (lower > y0 ||
                                Math.Abs(lower - y0) < 1f /* Margen de error. */)
                            {
                                // Si sale por acá está por encima de la plataforma,
                                // o con un margen de error de 1 pixel.
                                return lower;
                            }
                            else
                            {
                                // Si pasa por acá, la plataforma está un poco por encima
                                // de los pies del personaje, así que la ignoro.
                                break;
                            }
                        }
                        return lower;
                    }
                    else if (t.HasFlag(JazzTilesetCustomFlags.DiagonalDown))
                    {
                        return ((int)(y * JazzConsts.InvTileSize)) * JazzConsts.TileSize + ((int)x) % JazzConsts.TileSize;
                    }
                    else if (t.HasFlag(JazzTilesetCustomFlags.DiagonalUp))
                    {
                        return (((int)(y * JazzConsts.InvTileSize)) + 1) * JazzConsts.TileSize - ((int)x) % JazzConsts.TileSize;
                    }
                }

                // Si no encuentro una plataforma, caigo hasta el infinito.
                return int.MaxValue;
            }
        }

        internal int GetUpperPlatformY(float x, float y0)
        {
             {
                // El primer chequeo es en el tile en el que estoy (pero en este caso, sólo chequeo si
                // el tile está blqueado para ir para arriba.
                var t = tiledMap.SampleTileFlags(x, y0-0.1f);
                
                if (t.HasFlag(JazzTilesetCustomFlags.BlockedToTop))
                {
                    return ((int)(y0 * JazzConsts.InvTileSize)) * JazzConsts.TileSize;
                }
            }
            {
                // Ahora hago los otros chequeos.
                var y = Math.Max(0f, y0 - JazzConsts.TileSize);

                for (; y >= 0f; y -= JazzConsts.TileSize)
                {
                    var t = tiledMap.SampleTileFlags(x, y);

                    if (t.HasFlag(TileFlagsEnum.BlockedFromBottom))
                    {
                        return ((int)(y * JazzConsts.InvTileSize) + 1) * JazzConsts.TileSize;
                    }
                    //else if (t.HasFlag(JazzTilesetCustomFlags.BlockedToTop))
                    //{
                    //    return ((int)(y * JazzConsts.InvTileSize)) * JazzConsts.TileSize + 1;
                    //}
                    // else if (t.HasFlag(JazzTilesetCustomFlags.DiagonalDown))
                    // {
                    //     return y + 0.5f * JazzConsts.TileSize;
                    // }
                    // else if (t.HasFlag(JazzTilesetCustomFlags.DiagonalUp))
                    // {
                    //     return y + 0.5f * JazzConsts.TileSize;
                    // }
                }

                // Si no encuentro una plataforma, caigo hasta el infinito.
                return int.MinValue;
            }
        }

        internal bool IsValid(TileFlagsEnum wallFlag, float x, float y)
        {
            var t = tiledMap.SampleTileFlags(x, y);

            if (t.HasFlag(wallFlag))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Indica si la coordenada se encuentra fuera de los límites del mapa.
        /// </summary>        
        internal bool OutOfBoundsX(float x) => (x < 0f || x >= mapWidth);

        internal TileFlagsEnum SampleTileFlags(float x, float y) => tiledMap.SampleTileFlags(x, y);
    }
}
