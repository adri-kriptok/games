using Kriptok.Audio;
using Kriptok.Div.Extensions;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Tehuelche.Enemies;
using Kriptok.Tehuelche.Entities.Player;
using Kriptok.Tehuelche.Regions;
using Kriptok.Tehuelche.Scenes.Base;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Vertices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Kriptok.Tehuelche.Entities.Enemies.Battleship;
using static Kriptok.Tehuelche.Entities.Enemies.BattleshipCannon;

namespace Kriptok.Tehuelche.Entities.Enemies
{
    internal class Battleship : EntityBase<BattleshipView>
    {
        private ISoundHandler dyingSound;
        private BattleshipReflection reflection;
        private readonly Vector2F initialLocation;
        private float movementAngle = 0f;

        public Battleship(LevelBuilder builder, int x, int y)
            : base(new BattleshipView())
        {
            // var cos = Math.Cos(angle);
            // var sin = Math.Sin(angle);
            initialLocation = new Vector2F(x, y);
            // Angle.Z = angle;

            builder.Add(new BattleshipCannon(builder, this, 83f, 0f, 35f, 0f));
            builder.Add(new BattleshipCannon(builder, this, -86f, 0f, 35f, MathHelper.PIF));

            builder.Add(new BattleshipCannon(builder, this, 0f, 37.5f, 35f, MathHelper.HalfPIF));
            builder.Add(new BattleshipCannon(builder, this, -53f, 35f, 35f, MathHelper.HalfPIF));
            builder.Add(new BattleshipCannon(builder, this, 40f, 35f, 35f, MathHelper.HalfPIF));

            builder.Add(new BattleshipCannon(builder, this, 0f, -37.5f, 35f, -MathHelper.HalfPIF));
            builder.Add(new BattleshipCannon(builder, this, -53f, -35f, 35f, -MathHelper.HalfPIF));
            builder.Add(new BattleshipCannon(builder, this, 40f, -35f, 35f, -MathHelper.HalfPIF));
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision3DViewOBB();
            dyingSound = h.Audio.GetDivWaveHandler("Guerra.EXPLOS00.WAV");

            reflection = Add(new BattleshipReflection(this));
        }

        protected override void OnFrame()
        {
            var location = initialLocation.Plus(PolarVector.NewVector(movementAngle += 0.001f, 200f));
            Location.X = location.X;
            Location.Y = location.Y;

            Angle.Z = movementAngle + MathHelper.HalfPIF;
            reflection.Angle.Z = Angle.Z;
        }

        //internal override void OnDying()
        //{
        //    base.OnDying();
        //    dyingSound.Play();
        //}

        //public class BattleshipView : MqoMeshView
        //{
        //    public BattleshipView() : base(typeof(BattleshipView).Assembly, "Assets.Models.Battleship.mqo")
        //    {
        //        Scale = new Vector3F(0.5f);
        //    }

        //    /// <summary>
        //    /// La tienda es convexa, no pierdo tiempo ordenando caras.
        //    /// </summary>            
        //    protected override bool IsConvex() => false;
        //}

        public class BattleshipView : GdipShapeSplitterView<MqoMeshView>
        {
            public BattleshipView() : base(new MqoMeshView(typeof(BattleshipView).Assembly, "Assets.Models.Battleship.mqo"))
            {
                View.Scale = new Vector3F(0.5f);
            }
        }
    }

    internal class BattleshipReflection : EntityBase<GdipShapeView>
    {
        private readonly Battleship owner;

        public BattleshipReflection(Battleship owner) : base(new ReflectiveBattleshipView())
        {
            this.owner = owner;          
        }

        public override Vector3F GetRenderLocation() => owner.GetRenderLocation();

        public override bool IsAlive() => owner.IsAlive();

        protected override void OnFrame()
        {            
        }

        public class ReflectiveBattleshipView : MqoMeshView, IRenderizableReflection
        {
            public ReflectiveBattleshipView() : base(typeof(BattleshipView).Assembly, "Assets.Models.Battleship.mqo")
            {
                Scale = new Vector3F(0.5f);
                SwapAllFaces();
                ScaleZ = -ScaleZ;
            }

            /// <summary>
            /// La tienda es convexa, no pierdo tiempo ordenando caras.
            /// </summary>            
            protected override bool IsConvex() => false;
        }
    }

    internal class BattleshipCannon : EnemyBase<BattleshipCannonView>
    {
        private const float maxRotationAngle = MathHelper.HalfPIF * 1.1f;

        private readonly Battleship owner;
        private readonly PlayerHelicopterBase player;
        private readonly Vector3F locationMod;
        private readonly float angleMod;
        private EnemyAim aim;

        /// <summary>
        /// Contador para disparar;
        /// </summary>
        private static float shootCounter = 0;

        /// <summary>
        /// Sonidos del tanque.
        /// </summary>
        private ISoundHandler explosi7Sound, dyingSound;

        private readonly ITerrain terrain;
        private float relativeHatchAngle = 0f;
        private float cannonAngle = 0f;

        public BattleshipCannon(LevelBuilder builder, Battleship battleship, float xMod, float yMod, float zMod, float angleMod)
            : base(builder, new BattleshipCannonView(), 60f)
        {
            this.owner = battleship;
            this.player = builder.Player;
            this.terrain = builder.Terrain;
            this.locationMod = new Vector3F(xMod * battleship.View.ScaleX, yMod * battleship.View.ScaleY, zMod * battleship.View.ScaleZ);
            this.angleMod = angleMod;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision3DViewOBB();

            aim = Add(new EnemyAim(this));
            UpdateAimLocation();

            explosi7Sound = h.Audio.GetDivWaveHandler("Guerra.EXPLOSI7.WAV");
            dyingSound = h.Audio.GetDivWaveHandler("Guerra.EXPLOS02.WAV");
        }

        internal override Vector3F GetAimAngle() => new Vector3F()
        {
            Y = cannonAngle,
            Z = GetHatchAngle()
        };

        private float GetHatchAngle() => relativeHatchAngle + Angle.Z;        

        protected override void OnFrame()
        {
            base.OnFrame();
            Location = GetRenderLocation();
            Angle.Z = owner.Angle.Z + angleMod;

            var pointCannonTimeDelta = Sys.TimeDelta * 0.0625f;

            var distToPlayer = GetDistance2D(player);

            // -----------------------------------------------------------------------
            // Ataque.
            // -----------------------------------------------------------------------
            PointToPlayer(distToPlayer, pointCannonTimeDelta);

            if (distToPlayer > 384f)
            {
                return;
            }

            if ((shootCounter += Sys.TimeDelta) > 1000f && Rand.NextF() > 0.9f)
            {
                if (aim.Target)
                {
                    explosi7Sound.Play();

                    Add(new EnemyMissile(this, terrain, View.CannonTip.GetCalculatedLocation(), GetHatchAngle(), cannonAngle));
                    shootCounter = 0f;
                }
            }

            UpdateAimLocation();
        }

        private void UpdateAimLocation()
        {
            // -----------------------------------------------------------------------
            // Check de datos.
            // -----------------------------------------------------------------------            
            aim.Location = View.GetAimLocation();
            aim.Angle = GetAimAngle();
        }

        private void PointToPlayer(float distToPlayer, float timeDelta)
        {
            // --------------------------------------------------------------------------------
            // Rotación de la escotilla.
            // --------------------------------------------------------------------------------
            ResetHatchAngle();

            float lookAtAngle = 0f;
            float newCannonAngle = 0f;

            // Ángulo en reposo para el cañón.
            float disabledAngle = owner.Angle.Z + angleMod - Angle.Z;

            if (distToPlayer < 384f)
            {
                lookAtAngle = GetAngle2D(player) - Angle.Z;
                newCannonAngle = View.Cannon.GetAngleRotationToY(distToPlayer, player.Location.Z);

                //-----------------------------------------------------------------------------------
                // Y si la diferencia es muy grande, vuelvo al reposo.
                //-----------------------------------------------------------------------------------
                if (Math.Abs(PolarVector.MinAngleDifference(disabledAngle, lookAtAngle)) > maxRotationAngle)
                {
                    lookAtAngle = disabledAngle;
                    newCannonAngle = 0f;
                }
            }
            else
            {
                lookAtAngle = disabledAngle;
                newCannonAngle = 0f;
            }

            var difference = PolarVector.MinAngleDifference(relativeHatchAngle, lookAtAngle);
#if DEBUG
            if (difference > Math.PI)
            {
                Debugger.Break();
            }
#endif

            // --------------------------------------------------------------------------------
            // Altura del cañón.
            // --------------------------------------------------------------------------------
            PointCannon(timeDelta, difference, newCannonAngle);
        }

        private void PointCannon(float timeDelta, float difference, float newCannonAngle)
        {
            const float pointAtSpeed = 0.025f;

            relativeHatchAngle = relativeHatchAngle + (difference * 0.5f).Clamp(-pointAtSpeed, pointAtSpeed) * timeDelta;
            View.Hatch.RotateZ(relativeHatchAngle);

            cannonAngle = cannonAngle + ((newCannonAngle - cannonAngle) * 0.5f).Clamp(-pointAtSpeed, pointAtSpeed) * timeDelta;

            View.Cannon.Reset();
            View.Cannon.RotateY(cannonAngle);
        }

        internal override void OnDying()
        {
            base.OnDying();
            dyingSound.Play();
            Add(new PlayerMissileExplosion(Location, 2f));
        }

        //private float ResetAngle()
        //{
        //    // ... vuelvo a una posición de "reposo".
        //    var angleToDefault = owner.Angle.Z  -angleMod + MathHelper.PIF;// (owner.Angle.Z + angleMod);

        //    if (angleToDefault < hatchAngle)
        //    {
        //        angleToDefault += MathHelper.TwoPIF;
        //    }
        //    else
        //    {
        //        angleToDefault -= MathHelper.TwoPIF;
        //    }

        //    var difference = (angleToDefault - hatchAngle);
        //    hatchAngle = MathHelper.SimplifyAngle(hatchAngle);
        //    return difference;
        //}

        private void ResetHatchAngle()
        {
            // Reseteo el ángulo de la escotilla a cero, para que se pueda
            // calcular todo independientemente del ángulo del tanque.
            View.Hatch.Reset();            
        }

        public override Vector3F GetRenderLocation()
        {
            return owner.GetRenderLocation().Plus(locationMod.RotateZ(-owner.GetRotation3DZ()));
        }

        internal class BattleshipCannonView : HierarchicalShapeViewBase
        {
            public HierarchicalBranchVertex Hatch;
            public HierarchicalBranchVertex Cannon;
            public HierarchicalLeafVertex CannonTip;

            protected override void Build(HierarchicalShapeIntializer builder)
            {
                Hatch = builder.Central.AppendH(0f, 5f, 0f);
                Cannon = Hatch.AppendH(0f, 0f, 2.5f);

                var cannon = new MqoBuilder(Assembly, "Assets.Models.BattleshipCannon.mqo", 0);
                cannon.ScaleTransform(0.05f, 0.05f, 0.05f);
                cannon.TranslateToCenter();
                cannon.TranslateTo000(false, false, true);
                builder.AppendMesh(Cannon, cannon);

                // Guardo la longitud del cañón para saber después de dónde
                // tienen que salir los disparos.
                CannonTip = Cannon.AppendP(0f, 0f, cannon.Vertices.Max(p => p.Location.Z));

                var hatch = new MqoBuilder(Assembly, "Assets.Models.BattleshipCannon.mqo", 1);
                hatch.TranslateToCenter();
                hatch.ScaleTransform(0.05f, 0.05f, 0.05f);
                builder.AppendMesh(Hatch, hatch);
            }

            internal Vector3F GetAimLocation() => CannonTip.GetCalculatedLocation();
        }
    }
}
