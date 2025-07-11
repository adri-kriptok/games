using Kriptok.Drawing;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Games.Alien.Entities;
using Kriptok.Games.Alien.Entities.Enemies;
using Kriptok.Games.Alien.Regions;
using Kriptok.Regions.Context.Base;
using Kriptok.Scenes;
using Kriptok.Texts;
using Kriptok.Views.Base;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Kriptok.Games.Alien.Entities.Enemies.TankCannon;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace Kriptok.Games.Alien.Scenes
{
    internal class TestScene : SceneBase
    {        
        protected override void Run(SceneHandler h)
        {
            // Inicializa el movimiento de pantalla (scroll) y las variables de coordenadas del mismo
            var scroll = h.StartScroll(new ScrollX2Region());
            scroll.SetTarget(h.Add(scroll, new Test()));

        }

        private class Test : EntityBase<TankCannonView>
        {
            public Test() : base(new TankCannonView())
            {
                Location.X = 180;
                Location.Y = 160;
            }

            protected override void OnFrame()
            {
                if (Input.Left())
                {
                    Angle.Y -= 0.05f;
                    Trace.WriteLine(Angle.Y);
                }
                else if (Input.Right())
                {
                    Angle.Y += 0.05f;
                    Trace.WriteLine(Angle.Y);
                }
                
                // Add(new TankShot(this));                
            }
        }
    }
}