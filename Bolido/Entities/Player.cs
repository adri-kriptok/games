using Bolido.Mapping;
using Bolido.Scenes.Base;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Mapping.Tiles;
using Kriptok.Views.Sprites;
using System;
using System.Drawing;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace Bolido.Entities
{
    public enum PlayerDirectionEnum
    {
        None = 0,
        Right = 1,
        Down = 2,
        Left = 3,
        Up = 4
    }

    class Player : EntityBase<IndexedSpriteView>
    {
        private static readonly int[] sound_motor = { 65000, 65534, 1, 65534, 18000, 65534, -1, -12 };
        private static readonly int[] sound_bandera = { 50000, 30000, 35000, 40000, 20000, 50000, 5000, 60000, 4000, 65534, 3000, 65534, 2000, 65534, 1000, 65534, 0 };
        private static readonly int[] sound_charco = { 10000, 20000, 8000, 20000, 15000, 20000, 5000, 20000, 65000, 65534, 1, 65534, 18000, 65534, -1, -12 };
        private static int[] dirSound = sound_motor;

        private readonly LevelSceneBase level;
        private PlayerDirectionEnum pressedKey = PlayerDirectionEnum.None;
        private PlayerDirectionEnum direction = PlayerDirectionEnum.Right;

        private float avance = 8f;
        private float turbo = 16f;
        private int wheels = 0;

        private float eje_x = 200f;
        private float eje_y = 128f;

        public Player(LevelSceneBase level) : base(new IndexedSpriteView(typeof(Player).Assembly, "Assets.Player.png", 2, 4))
        {
            this.level = level;
            this.direction = PlayerDirectionEnum.Right;
        }

        protected override void OnFrame()
        {
            if (Input.UpPressed())
            {
                pressedKey = PlayerDirectionEnum.Up;
            }
            else if (Input.DownPressed())
            {
                pressedKey = PlayerDirectionEnum.Down;
            }
            else if (Input.LeftPressed())
            {
                pressedKey = PlayerDirectionEnum.Left;
            }
            else if (Input.RightPressed())
            {
                pressedKey = PlayerDirectionEnum.Right;
            }

            var closestFlag = base.FindCloseEntities2D<Flag>().OrderBy(p => p.Distance).FirstOrDefault();

            if (closestFlag.Entity != null)
            {
                var flag = closestFlag.Entity;
                if ((flag.Location.X > eje_x - 48f) && (flag.Location.X < eje_x + 24f) &&
                    (flag.Location.Y > eje_y - 72f) && (flag.Location.Y < eje_y))
                {
                    flag.Die();
                }
            }


            var snd = sound_bandera[0];
            PlayConsoleBeep(snd >> 8, 1);

            do
            {
                switch (pressedKey)
                {
                    case PlayerDirectionEnum.Left:
                        {
                            direction = PlayerDirectionEnum.Left;

                            var newX = eje_x - 24f - avance;
                            var v1 = level.GetTileFlegs(newX, eje_y - 24f);
                            var v2 = level.GetTileFlegs(newX, eje_y);
                            var v3 = level.GetTileFlegs(newX, eje_y + 23f);

                            if ((v1 != TileFlagsEnum.Blocked && v2 == TileFlagsEnum.Blocked) ||
                                (v2 != TileFlagsEnum.Blocked && v3 == TileFlagsEnum.Blocked))
                            {
                                pressedKey = PlayerDirectionEnum.Up;
                                direction = PlayerDirectionEnum.None;
                            }
                            else if ((v3 != TileFlagsEnum.Blocked && v2 == TileFlagsEnum.Blocked)
                                  || (v2 != TileFlagsEnum.Blocked && v1 == TileFlagsEnum.Blocked))
                            {
                                pressedKey = PlayerDirectionEnum.Down;
                                direction = PlayerDirectionEnum.None;
                            }
                            else if (v1 != TileFlagsEnum.Blocked
                                  && v2 != TileFlagsEnum.Blocked
                                  && v3 != TileFlagsEnum.Blocked)
                            {
                                eje_x -= avance;
                                avance = turbo;

                                CheckWater(v1, v2, v3);
                            }
                            break;
                        }
                    case PlayerDirectionEnum.Right:
                        {
                            direction = PlayerDirectionEnum.Right;

                            var newX = eje_x + 23f + avance;
                            var v1 = level.GetTileFlegs(newX, eje_y - 24f);
                            var v2 = level.GetTileFlegs(newX, eje_y);
                            var v3 = level.GetTileFlegs(newX, eje_y + 23f);

                            if ((v1 != TileFlagsEnum.Blocked && v2 == TileFlagsEnum.Blocked) ||
                                (v2 != TileFlagsEnum.Blocked && v3 == TileFlagsEnum.Blocked))
                            {                                
                                pressedKey = PlayerDirectionEnum.Up;
                                direction = PlayerDirectionEnum.None;
                            }
                            else if ((v3 != TileFlagsEnum.Blocked && v2 == TileFlagsEnum.Blocked)
                                  || (v2 != TileFlagsEnum.Blocked && v1 == TileFlagsEnum.Blocked))
                            {
                                pressedKey = PlayerDirectionEnum.Down;
                                direction = PlayerDirectionEnum.None;
                            }
                            else if (v1 != TileFlagsEnum.Blocked
                                  && v2 != TileFlagsEnum.Blocked
                                  && v3 != TileFlagsEnum.Blocked)
                            {
                                eje_x += avance;
                                avance = turbo;

                                CheckWater(v1, v2, v3);
                            }
                            break;
                        }
                    case PlayerDirectionEnum.Up:
                        {
                            direction = PlayerDirectionEnum.Up;

                            var newY = eje_y - 24f - avance;
                            var v1 = level.GetTileFlegs(eje_x - 24f, newY);
                            var v2 = level.GetTileFlegs(eje_x, newY);
                            var v3 = level.GetTileFlegs(eje_x + 23f, newY);

                            if ((v1 != TileFlagsEnum.Blocked && v2 == TileFlagsEnum.Blocked)
                             || (v2 != TileFlagsEnum.Blocked && v3 == TileFlagsEnum.Blocked))
                            {
                                pressedKey = PlayerDirectionEnum.Left;
                                direction = PlayerDirectionEnum.None;
                            }
                            else if ((v3 != TileFlagsEnum.Blocked && v2 == TileFlagsEnum.Blocked)
                                || (v2 != TileFlagsEnum.Blocked && v1 == TileFlagsEnum.Blocked))
                            {
                                pressedKey = PlayerDirectionEnum.Right;
                                direction = PlayerDirectionEnum.None;
                            }
                            else if (v1 != TileFlagsEnum.Blocked
                                  && v2 != TileFlagsEnum.Blocked
                                  && v3 != TileFlagsEnum.Blocked)
                            {
                                eje_y -= avance;
                                avance = turbo;

                                CheckWater(v1, v2, v3);
                            }
                            break;
                        }
                    case PlayerDirectionEnum.Down:
                        {
                            direction = PlayerDirectionEnum.Down;

                            var newY = eje_y + 23f + avance;
                            var v1 = level.GetTileFlegs(eje_x - 24, newY);
                            var v2 = level.GetTileFlegs(eje_x, newY);
                            var v3 = level.GetTileFlegs(eje_x + 23f, newY);

                            if ((v1 != TileFlagsEnum.Blocked && v2 == TileFlagsEnum.Blocked) ||
                                (v2 != TileFlagsEnum.Blocked && v3 == TileFlagsEnum.Blocked))
                            {
                                pressedKey = PlayerDirectionEnum.Left; 
                                direction = PlayerDirectionEnum.None;
                            }
                            else if ((v3 != TileFlagsEnum.Blocked && v2 == TileFlagsEnum.Blocked)
                                  || (v2 != TileFlagsEnum.Blocked && v1 == TileFlagsEnum.Blocked))
                            {
                                pressedKey = PlayerDirectionEnum.Right;
                                direction = PlayerDirectionEnum.None;
                            }
                            else if (v1 != TileFlagsEnum.Blocked
                                  && v2 != TileFlagsEnum.Blocked
                                  && v3 != TileFlagsEnum.Blocked)
                            {
                                eje_y += avance;
                                avance = turbo;

                                CheckWater(v1, v2, v3);
                            }
                            break;
                        }
                    default:
                        {
                            pressedKey = direction;
                            direction = PlayerDirectionEnum.None;
                            turbo = 8;
                            break;
                        }
                }
            } while (direction == PlayerDirectionEnum.None);

            var localWheels = (wheels = (wheels + 1) & 0xF) >> 3;

            switch (direction)
            {
                case PlayerDirectionEnum.Right:
                    View.Graph = 0 + localWheels;
                    break;
                case PlayerDirectionEnum.Left:
                    View.Graph = 2 + localWheels;
                    break;
                case PlayerDirectionEnum.Down:
                    View.Graph = 4 + localWheels;
                    break;
                case PlayerDirectionEnum.Up:
                    View.Graph = 6 + localWheels;
                    break;
            }

            Location.X = eje_x;
            Location.Y = eje_y;
        }

        private void CheckWater(TileFlagsEnum v1, TileFlagsEnum v2, TileFlagsEnum v3)
        {
            if (v1 == Tileset.Water || v2 == Tileset.Water || v3 == Tileset.Water)
            {
                // *dir_sound = sound_charco; 
                avance = 1f;
            }
        }


        internal static void PlayConsoleBeep(int frequency, int duration)
        {
#if DEBUG
            //ExceptionHelper.ValidateRange(frequency, 37, 32767);
#endif
            Action beep = () => Console.Beep(frequency, duration);
            beep.BeginInvoke(null, null);
        }
    }
}
