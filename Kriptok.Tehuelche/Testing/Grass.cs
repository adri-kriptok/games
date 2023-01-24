using Kriptok.Common;
using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Mapping.VoxelSpace;
using Kriptok.Maps.Terrains;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Regions.Pseudo3D.VoxelSpace;
using Kriptok.Scenes;
using System;

namespace Kriptok.Tests.Pseudo3D.Voxel
{
    static class Grass
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
                var voxel = VoxelTerrain.Create(
                    Resource.Get(typeof(Init).Assembly, "Testing.Assets.GrassTexture.png"),
                    new ByteTerrainData(typeof(Init).Assembly, "Testing.Assets.GrassTerrain.png"),
                    1f);

                var m7v = h.StartPseudo3D(new VoxelSpaceRegion(h.ScreenRegion.Rectangle, voxel)
                {
                    TextureScale = 5f
                });

                m7v.SetCamera(new Pseudo3DWithMouseLookCamera(h.Add(m7v, new Init()))
                {
                    //YShearing = -70f,
                    Height = 360
                });

                //var bg = new TexturedCubeCilinderView(Textures.Get(RmMvResources.Parallax("Ocean2.png"), false), 4);
                //bg.SwapAllFaces();
                //bg.RemoveShapesWhen(f => f.AllVertices(p => p.Y.In(0.5f, -0.5f)) || f.AllVertices(p => p.Y <= 0f));
                //bg.ScaleTransform(1f, 0.95f, 1f);
                //bg.ScaleTransform(20000000f, 20000000f, 20000000f);

                //m7v.SetBackground(bg);

                // Region.Ambience.SetFog(1000, 15000, Color.White);
                m7v.Ambience.SetLightSource(1, 1, 1);
            }
        }

        class Init : EntityBase
        {
            private const float rotation = MathHelper.PIF / 128;

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
            }
        }
    }
}
