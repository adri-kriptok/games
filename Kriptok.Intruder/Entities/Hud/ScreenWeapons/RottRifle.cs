//using Kriptok.Audio;
//using Kriptok.Drawing.Algebra;
//using Kriptok.Entities.Base;
//using Kriptok.Views.Sprites;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Kriptok.Intruder.Entities.ScreenWeapons
//{
//    /// <summary>
//    /// Arma que se ve en la pantalla
//    /// </summary>
//    internal class RottRifle : EntityBase<IndexedSpriteView>
//    {
//        private const int shootState = 10;
//        private const float swingMax = 5f;

//        private readonly Player player;
//        private int shooting;
//        private Vector2F baseLocation;

//        public RottRifle(Player player)
//            : base(new IndexedSpriteView(typeof(MsDosJpRifle).Assembly, "Assets.Images.Weapons.RottRifle.png", 3, 1)
//            {
//                Center = new PointF(0.5f, 1f),
//                // ScaleX = 1.5f,
//                // ScaleY = 1.5f
//            })
//        {
//            this.player = player;
//        }

//        protected override void OnStart(EntityStartHandler h)
//        {
//            base.OnStart(h);

//            Location.X = h.RegionSize.Width / 2;// + swingMax;
//            Location.Y = h.RegionSize.Height + 20;
//            Location.Z = Hud.Hud.WeaponZ;

//            baseLocation = Location.XY();
//        }

//        protected override void OnFrame()
//        {
//            Swing();

//            var state = shooting--;
//            if (state <= 0)
//            {
//                View.Graph = 0;
//                shooting = 0;
//            }
//            else if (state >= shootState)
//            {
//                Audio.PlayMidiNote(MidiInstrumentEnum.Gunshot, 
//                    IntruderConsts.GunMidiChannel, (byte)Rand.Next(53, 56), 127);
//                View.Graph = 1;
//            }            
//            else if (state < 4)
//            {
//                View.Graph = 0;
//            }
//            else
//            {
//                View.Graph = 2;
//            }
//        }

//        public bool Shoot()
//        {
//            if (shooting == 0)
//            {
//                shooting = shootState+3;
//                return true;
//            }
//            return false;
//        }

//        internal virtual void Swing()
//        {
//            Location.X = baseLocation.X + Player.SwingCos * swingMax;
//            Location.Y = baseLocation.Y + swingMax - Math.Abs(Player.SwingCos) * swingMax;
//        }
//    }
//}
