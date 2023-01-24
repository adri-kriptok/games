using Kriptok.Core;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Helpers;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Regions.Pseudo3D.Mode7;
using Kriptok.Scenes;
using Kriptok.Tehuelche.Entities;
using Kriptok.Tehuelche.Entities.Enemies;
using Kriptok.Views.Shapes;
using Kriptok.Views.Shapes.Base;
using Kriptok.Views.Shapes.Vertices;
using System;
using System.Linq;
using System.Windows.Forms;
using static Kriptok.Tehuelche.Enemies.Tank;

namespace Kriptok.Tehuelche.Testing
{
    internal class TestTank
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
                p.Mode = WindowSizeEnum.W680x384;
                p.Title = "Kriptok - Tests";
            });
        }

        class InitScene : SceneBase
        {
            protected override void Run(SceneHandler h)
            {
                var plane = new GdipMode7Plane(Assembly, "Testing.Assets.GrassTexture.png")
                {
                    InfiniteScroll = true
                };
                var m7v = h.StartPseudo3D(new GdipMode7Region(h.ScreenRegion.Rectangle, plane));

                var tank = h.Add(m7v, new TankTest());
                var target = h.Add(m7v, new Init());
                m7v.SetCamera(new Pseudo3DCustomizerCamera2(target)
                {
                    YShearing = -160f,
                    Distance = 25,
                    Height = 20
                });
        
                m7v.Ambience.SetLightSource(1, 1, 1);
                m7v.Ambience.AbsoluteLight = true;

                //h.Add(m7v, new RotTank());
            }
        }

        private class Pseudo3DCustomizerCamera2 : Pseudo3DCustomizerCamera
        {
            public Pseudo3DCustomizerCamera2(IPseudo3DTarget target) : base(target)
            {
            }
#if DEBUG || SHOWFPS
            protected override float GetModifier() => base.GetModifier() * 0.1f;
#endif
        }

        class RotTank : EntityBase<MqoMeshView>
        {
            public RotTank() : base(new MqoMeshView(typeof(RotTank).Assembly, "Assets.Models.Tank.mqo")
            {
                ScaleX = 0.05f,
                ScaleY = 0.05f,
                ScaleZ = 0.05f
            })
            {
                Location = new Vector3F(20f, 20f, 0f);
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                // h.SetCollision3DViewAABB();
                h.SetCollision3DViewOBB();
            }

            protected override void OnFrame()
            {
                Angle.Z += 0.01f;
            }
        }

        class Init : EntityBase
        {
            private const float rotation = MathHelper.PIF / 128 * 0.1f;

            protected override void OnFrame()
            {
                var elapsedTime = Sys.TimeDelta;
                if (Input.Left())
                {
                    Angle.Z -= rotation * elapsedTime;
                }
                else if (Input.Right())
                {
                    Angle.Z += rotation * elapsedTime;
                }

            }
        }

        class TankTest : EntityBase<TankView>
        {
            private TestEnemyAim aim;

            private float hatchAngle = 0f;
            private float cannonAngle = 0f;

            public TankTest() : base(new TankView())
            {
                View.ScaleTransform(5f, 5f, 5f);
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                Radius = 7;
                // h.SetCollision3DSphere();
                //h.SetCollision3DViewAABB();
                h.SetCollision3DViewOBB();

                this.aim = Add( new TestEnemyAim(this));
            }

            protected override void OnFrame()
            {
                Angle.Z += 0.01f;
                Advance2D(0.1f);

                // View.ScaleX = 1f + (float)Math.Cos(Angle.Z) * 0.25f;
                // View.ScaleY = 1f + (float)Math.Sin(Angle.Z) * 0.25f;

                if (Input.L1())
                {
                    hatchAngle-=0.1f;
                }
                else if (Input.R1())
                {
                    hatchAngle+=0.1f;
                }

                if (Input.Key(Keys.R))
                {
                    cannonAngle-=0.1f;
                }
                else if (Input.Key(Keys.F))
                {
                    cannonAngle += 0.1f;
                }

                View.Cannon.Reset();
                View.Cannon.RotateY(cannonAngle);

                View.Hatch.Reset();
                View.Hatch.RotateZ(hatchAngle);

            }

            public Vector3F GetAimLocation() => View.GetAimLocation();            

            internal Vector3F GetAimAngle() => new Vector3F()
            {
                Y = cannonAngle,
                Z = hatchAngle + Angle.Z
            };

        }

        class TestEnemyAim : EntityBase<GdipShapeView>
        {
            private readonly TankTest owner;
            
            public TestEnemyAim(TankTest owner) : base(new TestEnemyAimView(owner))
            {
                this.owner = owner;
            }

            /// <summary>
            /// Indica si está apuntando al jugador.
            /// </summary>
            public bool Target { get; private set; }

            protected override void OnFrame()
            {
                Angle = owner.GetAimAngle();

            }

            public override Vector3F GetRenderLocation() => owner.GetAimLocation();

            private class TestEnemyAimView : GdipShapeView
            {
                private readonly TankTest owner;

                public TestEnemyAimView(TankTest owner) : base(new VertexBase[2]
                {
                    new Vertex3(0f, 0f, 0f),
                    new Vertex3(0f, 0f, 256f)
                }, shapes =>
                {
                    var verts = shapes.GetVertices().ToArray();

                    shapes.Add(Strokes.Red, 0, 1);

                })
                {
                    this.owner = owner;
                }
            }
        }
    }
}
