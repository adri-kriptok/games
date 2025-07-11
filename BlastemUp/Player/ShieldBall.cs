using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Helpers;
using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Views.Sprites;
using System;

namespace Kriptok.Games.BlastemUp.Player
{
    public class ShieldBall : ProcessBase<IndexedSpriteView>
    {
        private const float quarterPi = (float)Math.PI * 0.25f;
        private const float quarterQuarterPi = quarterPi * 0.25f * Consts.SpeedModifier;

        /// <summary>
        /// Contador de uso general.
        /// </summary>
        private int timer = 0;

        /// <summary>
        /// Angulo de giro.
        /// </summary>
        private float locationAngle;

        /// <summary>
        /// Índice de la bola.
        /// </summary>
        private readonly int index;

        /// <summary>
        /// Máximo tiempo que se queda la bola del escudo en pantalla.
        /// </summary>
        private readonly int maxTimer;

        public ShieldBall(int index) 
            : base(new IndexedSpriteView(typeof(ShieldBall).Assembly, "Assets.Images.ShieldBall.png", 2, 1))
        {
            this.index = index;
            this.maxTimer = (int)((72 + index * 4) / Consts.SpeedModifier);
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Rectangle;
        }

        protected override void OnBegin()
        {
            // Va cambiando el angulo segun numero de bola 
            locationAngle = -index * quarterPi;
            
            // Coge las coordenadas a partir de la nave y en forma circular
            Location.X = Global.PlayerShip.Location.X + PolarVector.ProjectX(locationAngle, 70);
            Location.Y = Global.PlayerShip.Location.Y + PolarVector.ProjectY(locationAngle, 50);
            Location.Z = 2;

            Repeat(() =>
            {
                locationAngle += quarterQuarterPi;
                timer++;
                
                // Continua realizando la animacion                
                View.Graph = View.Graph == 0 ? 1 : 0;

                Location.X = Global.PlayerShip.Location.X + PolarVector.ProjectX(locationAngle, 70);
                Location.Y = Global.PlayerShip.Location.Y + PolarVector.ProjectY(locationAngle, 50);
                Frame();
            }, () => timer > maxTimer);

            Repeat(() =>
            {
                // El escudo se acaba, se quitan las bolas
                Location.X -= 30f * Consts.SpeedModifier;                  
                Frame();
            }, () => Location.X < 0f);

            if (index == 7)
            {
                // La nave ahora es vulnerable.
                PlayerShip.PlayerShipStatus = PlayerShipStatusEnum.Normal;
            }
        }

        public override Vector3F GetRenderLocation()
        {
            if (timer > maxTimer)
            {
                return base.GetRenderLocation();
            }
            else
            {
                return new Vector3F(
                    Global.PlayerShip.Location.X + PolarVector.ProjectX(locationAngle, 70),
                    Global.PlayerShip.Location.Y + PolarVector.ProjectY(locationAngle, 50),
                    Location.Z);
            }
        }
    }
}
