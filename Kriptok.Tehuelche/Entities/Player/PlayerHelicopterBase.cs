using Kriptok.Audio;
using Kriptok.Div;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Regions.Context.Base;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Tehuelche.Entities.Player;
using Kriptok.Tehuelche.Regions;
using Kriptok.Views.Base;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using Kriptok.Views.Shapes.Vertices;
using System;
using System.Drawing;
using System.Linq;

namespace Kriptok.Tehuelche.Entities
{
    internal abstract class PlayerHelicopterBase : EntityBase
    {
        internal const float ThirdPersonAngleModifier = MathHelper.QuarterPIF * 1.625f;

        private const float modifier = 0.25f;
        
        private readonly ITerrain terrain;

        /// <summary>
        /// Límites del área de juego.
        /// </summary>
        private readonly BoundF2 playArea;
        
        internal float CameraAngle;

        /// <summary>
        /// Velocidad de movimiento actual del jugador.
        /// </summary>
        private Vector2F movementSpeed;

        /// <summary>
        /// Objeto utilizado para detectar enemigos.
        /// </summary>
        private PlayerAutoAim autoAim;

        /// <summary>
        /// Utilizado para mover la cámara cuando le disparan.
        /// </summary>
        private float shakeCamera = 0f;

        /// <summary>
        /// Sonido de disparo del jugador.
        /// </summary>
        private ISoundHandler shootSound;

        public PlayerHelicopterBase(ITerrain terrain, float x, float y, float viewScale) 
            : base(new PlayerHelicopterView()
        {
            Scale = new Vector3F(viewScale)
        })
        {
            this.terrain = terrain;
            this.playArea = terrain.GetPlayArea();// region.PlayArea;
            Location.X = x;
            Location.Y = y;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.autoAim = Add(new PlayerAutoAim(this));

            h.SetCollision3DViewOBB();
            this.shootSound = h.Audio.GetWaveHandler(DivResources.Sound("Guerra.DISPARO9.WAV"));
        }

        protected override void OnFrame()
        {
            var timeDelta = Sys.TimeDelta;            
            var fowardSpeed = timeDelta * 0.002f;
            var backSpeed = fowardSpeed * 0.5f;
            var strafeSpeed = fowardSpeed * 0.2f;
            
            CameraAngle -= (CameraAngle - Angle.Z) * modifier * modifier * 0.5f;

            if (Input.Up())
            {                
                var cos = (float)Math.Cos(Angle.Z) * fowardSpeed;
                var sin = (float)Math.Sin(Angle.Z) * fowardSpeed;
                movementSpeed = movementSpeed.Plus(cos, sin);
                Angle.Y += strafeSpeed;
            }
            else if (Input.Down())
            {                
                var cos = (float)Math.Cos(Angle.Z) * backSpeed;
                var sin = (float)Math.Sin(Angle.Z) * backSpeed;
                movementSpeed = movementSpeed.Minus(cos, sin);
                Angle.Y -= strafeSpeed;
            }

            if (Input.Right())
            {
                var cos = (float)Math.Cos(Angle.Z + MathHelper.HalfPIF) * strafeSpeed;
                var sin = (float)Math.Sin(Angle.Z + MathHelper.HalfPIF) * strafeSpeed;
                movementSpeed = movementSpeed.Plus(cos, sin);
                Angle.X += strafeSpeed;
            }
            else if (Input.Left())
            {                
                var cos = (float)Math.Cos(Angle.Z - MathHelper.HalfPIF) * strafeSpeed;
                var sin = (float)Math.Sin(Angle.Z - MathHelper.HalfPIF) * strafeSpeed;
                movementSpeed = movementSpeed.Plus(cos, sin);
                Angle.X -= strafeSpeed;
            }

            if (Mouse.RightPressed())
            {                
                shootSound.Play();
                if (autoAim.LockedOnEnemy(out EnemyBase enemy))
                {
                    Add(new PlayerMissile(this, terrain, enemy));
                }
                else
                {
                    Add(new PlayerMissile(this, terrain));
                }
            }

            // ----------------------------------------------
            // Ajusto los valores.
            // ----------------------------------------------
            movementSpeed.X = movementSpeed.X.Clamp(-2f, 2f);
            movementSpeed.Y = movementSpeed.Y.Clamp(-2f, 2f);
            Angle.Y = Angle.Y.Clamp(-0.1f, 0.3f);
            Angle.X = Angle.X.Clamp(-0.1f, 0.1f);

            // ----------------------------------------------
            // Avanzo el helicóptero.
            // ----------------------------------------------
            Location.X += movementSpeed.X;
            Location.Y += movementSpeed.Y;

            var terrainHeight = terrain.GetHeight(Location.XY());

            // var ideal = terrainHeight + 50;
            Location.Z = Location.Z * 0.5f + terrainHeight * 0.5f + 25f;

            // ----------------------------------------------
            // Listo, ajusto todos los valores.
            // ----------------------------------------------
            movementSpeed = movementSpeed.Scale(0.99f);
            Angle.Y = Angle.Y * 0.99f;
            Angle.X = Angle.X * 0.99f;
            Location.X = Location.X.Clamp(playArea.MinX, playArea.MaxX);
            Location.Y = Location.Y.Clamp(playArea.MinY, playArea.MaxY);
        }

        internal virtual Vector3F GetShootingDirection() =>  Angle;

        internal virtual Vector3F GetShootingLocation() => Location;

        internal void Hit()
        {            
            if (shakeCamera == 0f)
            {
                //Scene.SendMessage(PlayerMessagesEnum.Hit);

            }
            shakeCamera = 1f;            
        }

        internal Vector2F GetRandomShake()
        {
            if (shakeCamera == 0f)
            {
                return Vector2F.Empty;
            }

            var rnd = new Vector2F(Rand.NextF(-2f, 2f) * shakeCamera);

            if ((shakeCamera *= 0.9f) <= 0.1f)
            {
                shakeCamera = 0f;
            }

            return rnd;
        }

        internal abstract Vector2F GetAngles();

        public class PlayerHelicopterView : MqoMeshView
        {
            private const float length = 100;
            private Vector3F v0;
            private Vertex3 v1;
            private Vertex3 v2;
            private Vertex3 v3;
            private Vertex3 v4;
            private float angle;
            private PlayerHelicopterBase heli;

            public PlayerHelicopterView() : base(typeof(PlayerHelicopterView).Assembly, "Assets.Models.Helicopter.mqo")
            {

            }

            protected override void Build(GdipShapeCollectionInitializer shapes)
            {
                base.Build(shapes);

                var mainVertex = shapes.GetVertices().OrderByDescending(p => p.GetLocation().Y).First();
                ((Vertex3)mainVertex).Y -= 2f;
                var stroke = Strokes.Get(Color.Black.SetAlpha(128), 2f);

                this.v1 = new Vertex3(1, mainVertex.GetLocation().Y, 0f);
                this.v2 = new Vertex3(-1, mainVertex.GetLocation().Y, 0f);

                this.v3 = new Vertex3(0f, mainVertex.GetLocation().Y, 1);
                this.v4 = new Vertex3(0f, mainVertex.GetLocation().Y, -1);
                this.angle = 0f;

                this.v0 = mainVertex.GetLocation();
                shapes.Add(stroke, mainVertex, v1);
                shapes.Add(stroke, mainVertex, v2);
                shapes.Add(stroke, mainVertex, v3);
                shapes.Add(stroke, mainVertex, v4);
            }

            public override void SetOwner(IViewOwner entity)
            {
                base.SetOwner(entity);
                this.heli = (PlayerHelicopterBase)entity;
            }

            public override bool IsVisible3D(IRenderContext3D context)
            {
                if (heli.IsVisible())
                {
                    return base.IsVisible3D(context);
                }
                
                return false;
            }

            public override void RenderOn(IRenderContext context)
            {                
                base.RenderOn(context);

                angle += 0.25f;
                var angle2 = angle + MathHelper.HalfPIF;

                var cos = (float)Math.Cos(angle) * length;
                var sin = (float)Math.Sin(angle) * length;
                var cos2 = (float)Math.Cos(angle2) * length;
                var sin2 = (float)Math.Sin(angle2) * length;

                v1.Location.X = v0.X + cos;
                v1.Location.Z = v0.Z + sin;

                v2.Location.X = v0.X - cos;
                v2.Location.Z = v0.Z - sin;

                v3.Location.X = v0.X + cos2;
                v3.Location.Z = v0.Z + sin2;

                v4.Location.X = v0.X - cos2;
                v4.Location.Z = v0.Z - sin2;
            }
        }

        protected abstract bool IsVisible();
    }
}
