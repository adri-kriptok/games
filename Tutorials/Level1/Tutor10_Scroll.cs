using Kriptok;
using Kriptok.Common;
using Kriptok.Core;
using Kriptok.Div;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Debug;
using Kriptok.Helpers;
using Kriptok.IO;
using Kriptok.Modes.Scroll;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tutorials.Level1
{
    static class Tutor10_Scroll
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Start(new InitScene(), s =>
            {
                s.FullScreen = true;
                s.Mode = WindowSizeEnum.W680x384;
                s.Title = "Tutor - 1 - 0 - Scroll | Kriptok";
            });
        }

        class InitScene : SceneBase
        {
            protected override void Run(SceneHandler h)
            {
    #if DEBUG
                Config.Get<BaseConfiguration>().Mute();
    #endif

                var starsBitmap = new FastBitmap(1024, 1024);

                for (int i = 0; i < 200; i++)
                {
                    var x = Rand.Next(1, 255) * 4;
                    var y = Rand.Next(1, 255) * 4;
                    starsBitmap.SetPixel(x, y, Color.White);
                    starsBitmap.SetPixel(x + 1, y, Color.LightCyan);
                    starsBitmap.SetPixel(x, y + 1, Color.LightCyan);
                    starsBitmap.SetPixel(x - 1, y, Color.LightCyan);
                    starsBitmap.SetPixel(x, y - 1, Color.LightCyan);
                    starsBitmap.SetPixel(x + 1, y + 1, Color.Blue);
                    starsBitmap.SetPixel(x - 1, y + 1, Color.Blue);
                    starsBitmap.SetPixel(x + 1, y - 1, Color.Blue);
                    starsBitmap.SetPixel(x - 1, y - 1, Color.Blue);
                }

                var mode = h.StartScroll(new Scroll(h.ScreenRegion.Rectangle, starsBitmap));

                mode.SetTarget(h.Add(mode, new Ship(mode)));

    #if DEBUG
                h.Add(mode, new ScrollCustomizer(mode));
    #endif
            }

            class Scroll : ScrollMode
            {
                public Scroll(Rectangle region, FastBitmap starsBitmap) : base(region, new GdipBrushScrollLayer(starsBitmap, true, true)
                {
                    Priority = -1000,
                    Antialias = true
                })
                {
                    AddLayer(new GdipBrushScrollLayer(starsBitmap, true, true)
                    {
                        // Lo pone del mismo tamaño, pero se mueve a la mitad de la velocidad.                
                        ReScale = new Vector2F(0.5f, 0.5f),
                        Antialias = true,
                        Priority = -10000
                    });

                    AddLayer(new GdipBrushScrollLayer(starsBitmap, true, true)
                    {
                        // Se ve el doble de grande y avanza al doble de velocidad.
                        //sp.Size = new Vector3F(2f, 2f, 1);
                        ReScale = new Vector2F(1.5f, 1.5f),
                        Antialias = true,
                        Priority = 10000
                    });
                }
            }

            class Ship : EntityBase<SpriteView>
            {
                private const float rotation = MathHelper.PIF / 48;
                private readonly ScrollMode scroll;
                private int shootCounter;

                public Ship(ScrollMode scroll) : base(new SpriteView(DivResources.Image("Tutorial.Spaceship.png")))
                {
                    this.scroll = scroll;
                }

                protected override void OnFrame()
                {
                    if (Input.Left()) Angle.Z -= rotation;
                    if (Input.Right()) Angle.Z += rotation;

                    if (Input.L1()) scroll.Rotation += rotation;
                    if (Input.R1()) scroll.Rotation -= rotation;

                    Advance2D(2f);
                    if (Input.Up()) Advance2D(2f);

                    if (Input.Key(Keys.Space))
                    {
                        if (shootCounter <= 0)
                        {
                            Audio.PlaySound(GetType().Assembly, "Assets.Sounds.Shot1.wav");
                            shootCounter = 5;
                        }

                        Add(new Shot(this, Location.X, Location.Y, Angle.Z));

                        if (shootCounter >= 0)
                        {
                            shootCounter -= 1;
                        }
                    }
                }
            }

            class Shot : EntityBase<SpriteView>
            {
                private readonly Ship owner;

                public Shot(Ship ship, float x, float y, float angle)
                    : base(new SpriteView(DivResources.Image("Tutorial.Shot.png")))
                {
                    this.owner = ship;

                    Location.X = x;
                    Location.Y = y;

                    Angle.Z = angle;
                }

                protected override void OnStart(EntityStartHandler h)
                {
                    base.OnStart(h);

                    Advance2D(Rand.Next(45, 50));
                }

                protected override void OnFrame()
                {
                    Advance2D(20);

                    if (MathHelper.GetDistance(Location.X, Location.Y, owner.Location.X, owner.Location.Y) > 400)
                    {
                        Die();
                    }
                }
            }
        }
    }
}
