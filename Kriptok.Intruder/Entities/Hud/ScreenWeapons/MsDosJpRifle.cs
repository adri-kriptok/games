using Kriptok.Audio;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Intruder.Entities.Hud;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Entities.ScreenWeapons
{
    /// <summary>
    /// Arma que se ve en la pantalla
    /// </summary>
    internal class MsDosJpRifle : EntityBase<IndexedSpriteView>
    {
        private const int shootState = 10;
        private const float swingMax = 5f;

        /// <summary>
        /// Multiplicadores para manejar la animación del arma.
        /// </summary>
        private const float deltaMultiplier = 25f, deltaDivisor = 1f / deltaMultiplier;

        /// <summary>
        /// Jugador que sostiene el arma.
        /// </summary>
        private readonly Player player;

        private Vector2F baseLocation;

        /// <summary>
        /// Contador para cambiar de estado el arma.
        /// </summary>
        private float shooting;

        /// <summary>
        /// Munición que tiene el arma.
        /// </summary>
        internal int Ammo;

        /// <summary>
        /// Indica si está listo para reproducir el sonido.
        /// </summary>
        private bool readyToPlaySound = true;

        /// <summary>
        /// Indica que tiene que reproducir el sonido el siguiente frame.
        /// </summary>
        private bool soundNextFrame = false;

        /// <summary>
        /// Alturas default de renderización del arma.
        /// </summary>
        private float yDefault, yLower = 0f;

        /// <summary>
        /// Indica si el arma se está moviendo y no puede disparar.
        /// </summary>
        private bool movingWeapon = false;

        public MsDosJpRifle(Player player)
            : base(new IndexedSpriteView(typeof(MsDosJpRifle).Assembly, "Assets.Images.Weapons.MsDosJpRifle.png", 3, 1)
            {
                Center = new PointF(1f, 1f),
                ScaleX = 1.75f,
                ScaleY = 1.75f
            })
        {
            this.player = player;
#if DEBUG || SHOWFPS
            Ammo = 999;
#else
            Ammo = 400;
#endif
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            Location.X = h.RegionSize.Width + swingMax * 2;
            Location.Y = h.RegionSize.Height + swingMax - IntruderConsts.HudHeight;
            Location.Z = Hud.Hud.WeaponZ;

            baseLocation = Location.XY();
            yDefault = baseLocation.Y;
            yLower = yDefault + 90;
        }

        protected override void OnFrame()
        {
            var weaponUp = player.WeaponUp();

            // Ubicación vertical de dónde debería renderizarse la imagen.
            var targetY = weaponUp ? yDefault : yLower;
            
            var diff = baseLocation.Y - targetY;
            if (diff.Abs().Round() > 0)
            {
                baseLocation.Y -= diff * 0.5f;
                movingWeapon = true;
            }
            else
            {
                baseLocation.Y = targetY;
                movingWeapon = false;
            }

            if (!movingWeapon)
            {
                if (weaponUp)
                {
                    Swing();

                    if (readyToPlaySound && soundNextFrame)
                    {
                        Audio.PlayMidiNote(MidiInstrumentEnum.Gunshot,
                            IntruderConsts.GunMidiChannel, (byte)Rand.Next(53, 56), 127);
                        readyToPlaySound = false;
                    }

                    soundNextFrame = false;

                    var state = (shooting -= Sys.TimeDelta) * deltaDivisor;
                    if (state <= 0f)
                    {
                        View.Graph = 0;
                        shooting = 0f;
                        readyToPlaySound = true;
                    }
                    else if (state >= shootState)
                    {
                        soundNextFrame = true;
                        // if (state == shootState)
                        // {
                        // }
                        View.Graph = 1;
                    }
                    else if (state < 4f)
                    {
                        View.Graph = 0;
                    }
                    else
                    {
                        View.Graph = 2;
                    }
                }
            }
            else
            {
                Location.Y = baseLocation.Y;
            }
        }

        public bool Shoot()
        {            
            if (Ammo <= 0)
            {
                return false;
            }

            if (shooting == 0)
            {
                shooting = (shootState + 3) * deltaMultiplier;
                Ammo -= 1;
                return true;
            }

            return false;
        }

        internal virtual void Swing()
        {
            Location.X = baseLocation.X + Player.SwingCos * swingMax;
            Location.Y = baseLocation.Y + swingMax - Math.Abs(Player.SwingCos) * swingMax;
            Angle.Z = Player.SwingCos * 0.05f;
        }
    }
}
