using Kriptok.Scenes;
using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.IO;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kriptok.Audio;
using Kriptok.Views.Shapes;

namespace Billiard.Scenes
{
    class TitleScene : SceneBase
    {
        private static readonly SuperFont titleFont = SuperFont.Build(f =>
        {
            f.Font = new Font("Arial", 50, FontStyle.Bold);
            f.Border = Strokes.Get(Color.FromArgb(100, 108, 104));
            f.SetColor(Color.FromArgb(180, 168, 104), Color.Orange);
            f.SetShadow(3, 3, Color.FromArgb(36, 24, 12));
        });

        private static readonly SuperFont menuFont = SuperFont.Build(f =>
        {
            f.Font = new Font("Bahnschrift", 26, FontStyle.Bold);
            f.SetColor(Color.FromArgb(232, 216, 164), Color.FromArgb(100, 108, 104));
            f.SetShadow(1, 1, Color.FromArgb(120, 116, 68));
        });

        protected override void Run(SceneHandler h)
        {
            h.FadeOff(255);
#if !DEBUG
            h.PlayMusic(Assembly, "Assets.TheEntertainer.mid");
#endif
            h.ScreenRegion.SetBackground(Assembly, "Assets.TitleScreen.png");

            // Imprime mensajes
            h.Write(titleFont, 320, 4, "TOTAL BILLIARD").CenterTop();

            h.Write(menuFont, 320, 480, "Daniel Navarro  DIV Games Studio").CenterBottom();

            var stick = h.Add(new MenuStick(new ITextEntity[3]
            {
                h.Write(menuFont, 400, 320, "Empezar").LeftTop(),
                h.Write(menuFont, 400, 354, Global.WinningScoreString).LeftTop(),
                h.Write(menuFont, 400, 388, "Salir al DOS").LeftTop()
            }));

            h.FadeOn();

            h.WaitWhile(() => stick.IsAlive());

            if (stick.Option == -3)
            {
                h.Set(new GameScene());
            }
            else
            {
                // Borra todo y apaga la pantalla
                h.FadeOff();
                h.Exit();
            }
        }

        private class MenuStick : Process2Base<SpriteView>
        {
            /// <summary>
            /// Opción seleccionada.
            /// </summary>
            internal int Option = 0;

            /// <summary>
            /// Opciones del menú.
            /// </summary>
            private readonly ITextEntity[] options;

            private float angle;

            /// <summary>
            /// X incremento.
            /// </summary>
            private float incr_x;

            /// <summary>
            /// Movimiento del taco.
            /// </summary>
            private float ang2 = 0;
            private ISoundHandler menuChangeSound;
            private ISoundHandler menuOKSound;

            public MenuStick(ITextEntity[] options) 
                : base(new SpriteView(typeof(MenuStick).Assembly, "Assets.TitleStick.png")
            {
                Center = new PointF(0f, 0.75f)
            })
            {
                this.options = options;
            }

            protected override void OnStart(ProcessStartHandler h)
            {
                base.OnStart(h);

                this.menuChangeSound = h.Audio.GetMenuChangeSound();
                this.menuOKSound = h.Audio.GetMenuOKSound();
            }

            protected override void OnBegin(ProcessHandler h)
            {
                // Renicia las variables del cursor
                Location.Y = 342f;
                var incr_y = 0f;
                var ang1 = 0f;
 
                h.Repeat(() =>
                {
                    Location.X = 17 + PolarVector.ProjectX(angle, 16f);
                    if ((angle += MathHelper.PIF / 8) > MathHelper.PIF)
                    {
                        angle -= 2f * MathHelper.PIF;
                    }

                    // Comprueba si se ha elegido una opcion
                    if (Option != 0 && angle < MathHelper.PIF / 15 && angle > -MathHelper.PIF / 15)
                    {
                        incr_x = 400;

                        // Repite hasta que desaparezca el texto
                        h.Repeat(() =>
                        {
                            var text = options[Option - 1];
                            text.LocationX = incr_x += 16;
                            text.LocationY = incr_y;

                            Frame();
                        }, () => incr_x > 640);  // Cambia texto

                        // Se ha elegido la opcion de cambiar tipo de juego
                        if (Option == 2)
                        {
                            Global.CurrentScoreOption = ++Global.CurrentScoreOption % 3;
                            options[1].Message = Global.WinningScoreString;
                            h.Repeat(() =>
                            {
                                var text = options[1];
                                text.LocationX = incr_x -= 16;
                                text.LocationY = incr_y;
                                
                                Frame();
                            }, () => incr_x == 400);
                            Option = 0;
                            incr_y = 0;
                        }
                        else
                        {
                            Option -= 4;
                        }
                    }

                    // Cuando el palo este sobre una opcion
                    if (incr_y == 0)
                    {
                        if (Input.Key(Keys.Enter) ||
                            Mouse.Left ||
                            Input.Key(Keys.Space))    // Selecciona esa opcion
                        {
                            menuOKSound.Play();
                            incr_y = Location.Y - 22f;
                            Option = (Location.Y.Round() - 342) / 34 + 1;
                        }
                        else
                        {
                            // Cambia a otra opcion
                            if (Input.Down() && Location.Y < 410f)
                            {
                                menuChangeSound.Play();
                                incr_y = Location.Y + 17;
                                ang1 = MathHelper.PIF / 2;
                                ang2 = -MathHelper.PIF / 8;
                            }

                            // Cambia a otra opcion
                            if (Input.Up()  && Location.Y > 342f)
                            {
                                menuChangeSound.Play();
                                incr_y = Location.Y - 17;
                                ang1 = -MathHelper.PIF / 2;
                                ang2 = MathHelper.PIF / 8;
                            }
                        }
                    }

                    // Detiene el incremento vertical.
                    if (ang2 != 0f)
                    {
                        ang1 += ang2;
                        Location.Y = incr_y - PolarVector.ProjectY(ang1, 17f);
                        if (ang2 < 0)
                        {
                            if (ang1 < -MathHelper.PIF / 2)
                            {
                                Location.Y = incr_y + 17;
                                incr_y = 0;
                                ang2 = 0;
                            }
                        }
                        else
                        {
                            if (ang1 > MathHelper.PIF / 2)
                            {
                                Location.Y = incr_y - 17;
                                incr_y = 0;
                                ang2 = 0;
                            }
                        }
                    }
                    // comprueba la pulsacion de la tecla escape
                    if (Input.Key(Keys.Escape))
                    {
                        Option = -1;
                    }
                    Frame();
                }, () => Option < 0);
            }
        }
    }
}
