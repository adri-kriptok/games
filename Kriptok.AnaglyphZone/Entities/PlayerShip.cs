using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.AZ.Entities.Enemies;
using Kriptok.AZ.Scenes;
using Kriptok.AZ.Views;
using Kriptok.Views.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kriptok.Audio;

namespace Kriptok.AZ.Entities
{
    internal class PlayerShip : EntityBase<WireframeMeshView>
    {
        private readonly CamTargetBase camTarget;

        private Vector2F intertia;
        internal Vector2F RelativeLocation;

        private bool readyToShoot = false;
        private int iAngle = 0;

        private ISingleCollisionQuery<EnemyBase> enemyCollision;
        private int soundCounter = 0;

        /// <summary>
        /// Sonido a ejecutar cuando dispara.
        /// </summary>
        private ISoundHandler shootSound;

        //public PlayerShip(CamTarget camTarget) : base(new WireframeMeshView(typeof(Meshes).Assembly, "Assets.Models.SpaceShip.mqo"))
        public PlayerShip(CamTargetBase camTarget) : base(new PlayerShipView())
        {
            this.camTarget = camTarget;            
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision3DViewOBB();

            enemyCollision = h.GetCollision3D<EnemyBase>();
            shootSound = h.Audio.GetSoundHandler("Assets.Sounds.Shot1.wav");
        }

        protected override void OnFrame()
        {
            // Angle.Z += 0.01f;

            if (Input.Up())
            {
                intertia = intertia.Plus(0f, 2f);
                iAngle = iAngle - 10;

                if (iAngle < -360)
                {
                    iAngle += 360;
                }
            }
            else if (Input.Down())
            {
                intertia = intertia.Plus(0f, -2f);
                iAngle = iAngle + 10;

                if (iAngle >= 360)
                {
                    iAngle -= 360;
                }
            }

            if (Input.Button03() && (readyToShoot = !readyToShoot))
            {
                if (soundCounter++ >= 3)
                {
                    shootSound.Play();   
                    soundCounter = 0;
                }
                Add(new PlayerShot(this, -30f, -10f));
                Add(new PlayerShot(this, 30f, -10f));
            }

            if (iAngle != 0)
            {
                iAngle = iAngle - Math.Sign(iAngle) * 5;
            }

            Angle.X = MathHelper.DegreesToRadians(iAngle);

            if (Input.Left())
            {
                intertia = intertia.Plus(-2f, 0f);
            }
            else if (Input.Right())
            {
                intertia = intertia.Plus(2f, 0f);
            }

            if (enemyCollision.OnCollision(out EnemyBase enemy))
            {
                Add(new Explosion(new Vector3F(Location.X, enemy.Location.Y + 1f, Location.Z)));
                enemy.Damage(100);
                Die();
                Scene.SendMessage(GameMessages.Died);
                return;
            }

            RelativeLocation = RelativeLocation.Plus(intertia);

            RelativeLocation.X = RelativeLocation.X.Clamp(-250f, 250f);
            RelativeLocation.Y = RelativeLocation.Y.Clamp(-200f, 200f);

            intertia = intertia.Scale(0.75f);

            Location = GetRenderLocation();
        }

        public override Vector3F GetRenderLocation()
        {
            return camTarget.Location.Plus(RelativeLocation.X, 0f, RelativeLocation.Y);
        }

        internal class PlayerShipView : WireframeMeshView
        {
            public PlayerShipView() : base(typeof(Meshes).Assembly, "Assets.Models.SpaceShip.mqo")
            {
                ScaleX = 0.2f;
                ScaleY = 0.2f;
                ScaleZ = 0.2f;
            }
        }
    }
}
