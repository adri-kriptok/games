using Kriptok.Audio;
using Kriptok.Div;
using Kriptok.Div.Extensions;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Tehuelche.Entities;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Tehuelche.Entities.Player;
using Kriptok.Tehuelche.Regions;
using Kriptok.Tehuelche.Scenes.Base;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Vertices;
using System;
using System.Diagnostics;
using System.Linq;
using static Kriptok.Tehuelche.Enemies.Tank;

namespace Kriptok.Tehuelche.Enemies
{
    internal class Tank : EnemyBase<TankView>
    {
        private readonly PlayerHelicopterBase player;
        private readonly ITerrain terrain;
        private float relativeHatchAngle = 0f;
        private float cannonAngle = 0f;
        private EnemyAim aim;

        /// <summary>
        /// Sonidos del tanque.
        /// </summary>
        private ISoundHandler explosi7Sound, dyingSound;

        /// <summary>
        /// Contador para disparar;
        /// </summary>
        private float shootCounter = 0;

        public Tank(LevelBuilder builder, int x, int y, float angle)
            : base(builder, new TankView(), 30f)
        {
            this.player = builder.Player;
            this.terrain = builder.Terrain;
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);

            Location.X = (float)(x + cos * 25f);
            Location.Y = (float)(y + sin * 25f);
            Angle.Z = angle;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision3DViewOBB();
            ResetHatchAngle();

            aim = Add(new EnemyAim(this));
            UpdateAim();
            explosi7Sound = h.Audio.GetDivWaveHandler("Guerra.EXPLOSI7.WAV");
            dyingSound = h.Audio.GetDivWaveHandler("Guerra.EXPLOS02.WAV");
        }

        protected override void OnFrame()
        {
            base.OnFrame();

            var distToPlayer = GetDistance2D(player);

            if (distToPlayer > 384f)
            {
                return;
            }

            // -----------------------------------------------------------------------
            // Movimiento del tanque.
            // -----------------------------------------------------------------------
            var rnd = (float)Math.Sin(Rand.NextF(0f, 0.01f));

            Angle.Z = Angle.Z + rnd;

            Advance2D(Sys.TimeDelta * 0.01f);

            if (OnRadiusCollision3D(out DistanceTo3D<EnemyBase> found))
            {
                found.RejectMeHalfRadius();
            }

            // -----------------------------------------------------------------------
            // Ataque del tanque.
            // -----------------------------------------------------------------------
            PointToPlayer(distToPlayer, Sys.TimeDelta * 0.0625f);

            if ((shootCounter += Sys.TimeDelta) > 1000f)
            {
                if (aim.Target)
                {
                    explosi7Sound.Play();

                    Add(new EnemyMissile(this, terrain, View.CannonTip.GetCalculatedLocation(), GetHatchAngle(), cannonAngle));
                    shootCounter = 0f;
                }
            }

            // -----------------------------------------------------------------------
            // Check de datos.
            // -----------------------------------------------------------------------
            Location.Z = (terrain.GetHeight(Location.XY()) + Location.Z) * 0.5f;
            UpdateAim();
        }

        private void UpdateAim()
        {
            aim.Location = View.GetAimLocation();
            aim.Angle = GetAimAngle();
        }

        private void PointToPlayer(float distToPlayer, float timeDelta)
        {
            const float pointAtSpeed = 0.025f;
            const float mPointAtSpeed = -pointAtSpeed;

            // --------------------------------------------------------------------------------
            // Rotación de la escotilla.
            // --------------------------------------------------------------------------------
            ResetHatchAngle();

            var difference = PolarVector.MinAngleDifference(relativeHatchAngle, GetAngle2D(player) - Angle.Z);

#if DEBUG
            if (difference > Math.PI)
            {
                Debugger.Break();
            }
#endif

            relativeHatchAngle = relativeHatchAngle + (difference * 0.5f).Clamp(mPointAtSpeed, pointAtSpeed) * timeDelta;
            View.Hatch.RotateZ(relativeHatchAngle);

            // --------------------------------------------------------------------------------
            // Altura del cañón.
            // --------------------------------------------------------------------------------                        
            var newCannonAngle = View.Cannon.GetAngleRotationToY(distToPlayer, player.Location.Z);

            cannonAngle = cannonAngle +
                ((newCannonAngle - cannonAngle) * 0.5f).Clamp(mPointAtSpeed, pointAtSpeed) * timeDelta;

            View.Cannon.Reset();
            View.Cannon.RotateY(cannonAngle);
        }

        private void ResetHatchAngle()
        {
            // Reseteo el ángulo de la escotilla a cero, para que se pueda
            // calcular todo independientemente del ángulo del tanque.
            View.Hatch.Reset();
        }

        internal override void OnDying()
        {
            base.OnDying();
            dyingSound.Play();
            Add(new PlayerMissileExplosion(Location, 2f));
        }

        internal override Vector3F GetAimAngle() => new Vector3F()
        {
            Y = cannonAngle,
            Z = GetHatchAngle()
        };

        private float GetHatchAngle() => relativeHatchAngle + Angle.Z;        

        public class TankView : HierarchicalShapeViewBase
        {
            public HierarchicalBranchVertex Hatch;
            public HierarchicalBranchVertex Cannon;
            public HierarchicalLeafVertex CannonTip;

            protected override void Build(HierarchicalShapeIntializer builder)
            {
                const string resourceName = "Assets.Models.Tank.mqo";

                Hatch = builder.Central.AppendH(0f, 5f, 0f);
                Cannon = Hatch.AppendH(0f, 0f, 2.5f);

                // var cannonEnd = Cannon.AppendP(0f, 0f, 8f);
                // builder.Add(Strokes.Blue, builder.Central, Hatch);
                // builder.Add(Strokes.Red,  Hatch, Cannon);
                // builder.Add(Strokes.Yellow, Cannon, cannonEnd);

                var chatapilar = new MqoBuilder(Assembly, resourceName, 0);
                chatapilar.ScaleTransform(0.05f, 0.05f, 0.05f);
                builder.AppendMesh(builder.Central, chatapilar);

                var cannon = new MqoBuilder(Assembly, resourceName, 1);
                cannon.ScaleTransform(0.05f, 0.05f, 0.05f);
                cannon.TranslateToCenter();
                cannon.TranslateTo000(false, false, true);
                builder.AppendMesh(Cannon, cannon);

                // Guardo la longitud del cañón para saber después de dónde
                // tienen que salir los disparos.
                CannonTip = Cannon.AppendP(0f, 0f, cannon.Vertices.Max(p => p.Location.Z));

                var hatch = new MqoBuilder(Assembly, resourceName, 2);
                hatch.TranslateToCenter();
                hatch.ScaleTransform(0.05f, 0.05f, 0.05f);
                builder.AppendMesh(Hatch, hatch);
            }

            internal Vector3F GetAimLocation() => CannonTip.GetCalculatedLocation();
        }
    }
}
