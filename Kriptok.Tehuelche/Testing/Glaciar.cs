using Kriptok.Common;
using Kriptok.Core;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Mapping.VoxelSpace;
using Kriptok.Maps.Terrains;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Regions.Pseudo3D.VoxelSpace;
using Kriptok.Scenes;
using Kriptok.Views.Shapes;
using System;

namespace Kriptok.Tests.Pseudo3D.Voxel
{
    static class Glaciar
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
                p.Mode = WindowSizeEnum.W340x192;
                p.Title = "Kriptok - Tests";
            });
        }

        class InitScene : SceneBase
        {
            protected override void Run(SceneHandler h)
            {
                var voxel = VoxelTerrain.Create(
                    Resource.Get(typeof(Init).Assembly, "Testing.Assets.Clouds.png"),
                    new ByteTerrainData(typeof(Init).Assembly, "Testing.Assets.Clouds.png"), 0.5f);

                var m7v = h.StartPseudo3D(new VoxelSpaceRegion(h.ScreenRegion.Rectangle, voxel)
                {
                    TextureScale = 4f
                });

                m7v.Ambience.SetLightSource(1, 1, 1);
                m7v.SetCamera(new Pseudo3DWithMouseLookCamera(h.Add(m7v, new Init(m7v)))
                {
                    // YShearing = -70f,
                    Height = 360
                });

                //var bg = new TexturedCubeCilinderView(Textures.Get(RmMvResources.Parallax("Ocean2.png"), false), 4);
                //bg.SwapAllFaces();
                //bg.RemoveShapesWhen(f => f.AllVertices(p => p.Y.In(0.5f, -0.5f)) || f.AllVertices(p => p.Y < 0f));
                //bg.ScaleTransform(1f, 0.95f, 1f);
                //bg.ScaleTransform(20000000f, 20000000f, 20000000f);

                //m7v.SetBackground(bg);

                //Region.SetFog(200, 6000, Color.White);

                h.Add(m7v, new BasicObject(new FlatCubeView(Material.Blue)
                {
                    Scale = new Vector3F(256f)
                })
                {
                    Location = new Vector3F(200, 0, 128)
                });
            }
        }

        class Init : EntityBase
        {
            private const float rotation = MathHelper.PIF / 128;
            private readonly VoxelSpaceRegion region;

            public Init(VoxelSpaceRegion voxel)
            {
                this.region = voxel;
            }

            protected override void OnFrame()
            {
                var elapsedTime = Sys.TimeDelta * 0.0625f;

                if (Input.L1())
                {
                    Angle.Z -= rotation * elapsedTime;
                }
                else if (Input.R1())
                {
                    Angle.Z += rotation * elapsedTime;
                }

                if (Input.Down())
                {
                    Advance2D(-5f * elapsedTime);

                    if (Input.Button04())
                    {
                        Advance2D(-10 * elapsedTime);
                    }
                }
                else if (Input.Up())
                {
                    Advance2D(5f * elapsedTime);

                    if (Input.Button04())
                    {
                        Advance2D(10f * elapsedTime);
                    }
                }

                if (Input.Left())
                {
                    Strafe2D(-5f * elapsedTime);
                    if (Input.Button04())
                    {
                        Strafe2D(-5f * elapsedTime);
                    }
                }
                else if (Input.Right())
                {
                    Strafe2D(5f * elapsedTime);
                    if (Input.Button04())
                    {
                        Strafe2D(5f * elapsedTime);
                    }
                }

                Angle.Z += region.RotationWithMouseHorizontally();
            }
        }
    }
}
