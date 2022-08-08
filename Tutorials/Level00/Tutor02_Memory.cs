using Kriptok;
using Kriptok.Core;
using Kriptok.Div;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.IO;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Texts;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tutorials.Level00
{
    static class Tutor02_Memory
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Start(new InitScene(), p =>
            {
                p.FullScreen = true;
                p.Mode = WindowSizeEnum.W800x450;
                p.Title = "Kriptok Sdk - Tutor - Tutor 01 - Save";
            });
        }
    }

    public class Coords
    {
        public float X;
        public float Y;
        public float AngleZ;
    }

    class InitScene : SceneBase
    {

        protected override void Run(SceneHandler h)
        {
            h.ScreenRegion.SetBackground(bg => bg.FillStretched(Assembly, "Level00.Assets.Images.Background.png"));

            h.Write(new SuperFont(Fonts.Arial12BoldItalic, Color.White), h.ScreenRegion.Size.Width / 2, 400,
                "Presione S para guardar la posición y L para cargar.");

            h.Add(new Ship());
        }
    }

    public class Ship : EntityBase<SpriteView>
    {
        private const float rotation = (float)(Math.PI / 16);
        public int Points;
        private Vector2F centerScreen;

        public Ship() : base(new SpriteView(DivResources.Image("Tutorial.Spaceship.png")))
        {
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.centerScreen = new Vector2F(h.RegionSize.Width / 2, h.RegionSize.Height / 2);

            Load();
        }

        protected override void OnFrame()
        {
            if (Input.Key(Keys.S))
            {
                Save();
            }

            if (Input.Key(Keys.L))
            {
                Load();
            }

            if (Input.Left())
            {
                Angle.Z -= rotation;
            }

            if (Input.Right())
            {
                Angle.Z += rotation;
            }

            if (Input.Up())
            {
                Advance2D(5);
            }
        }

        private void Load()
        {
            try
            {
                Memory<Coords>.Load();
                Location.X = Memory<Coords>.Current.X;
                Location.Y = Memory<Coords>.Current.Y;
                Angle.Z = Memory<Coords>.Current.AngleZ;
            }
            catch
            {
                Location.X = centerScreen.X;
                Location.Y = centerScreen.Y;
            }           
        }

        private void Save()
        {
            Memory<Coords>.Current.X = Location.X;
            Memory<Coords>.Current.Y = Location.Y;
            Memory<Coords>.Current.AngleZ = Angle.Z;
            Memory<Coords>.Save();
        }
    }
}
