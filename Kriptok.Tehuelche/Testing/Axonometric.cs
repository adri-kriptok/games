using Kriptok.Common;
using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Mapping.VoxelSpace;
using Kriptok.Mapping.Terrains;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Regions.Pseudo3D.VoxelSpace;
using Kriptok.Regions.Scroll.Axonometric.VoxelSpace;
using Kriptok.Scenes;
using Kriptok.Views.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Tehuelche.Testing
{
    internal class Axonometric
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Engine.Start(new InitScene(), p =>
            {
                p.FullScreen();
                p.Mode = WindowSizeEnum.W340x192;
                p.Title = "Kriptok - Tests";
            });
        }

        class InitScene : SceneBase
        {
            protected override void Run(SceneHandler h)
            {
                var voxel = VoxelTerrain.Create(
                    Resource.Get(typeof(Init).Assembly, "Scenes.Map01.Texture.png"),
                    new ByteTerrainData(typeof(Init).Assembly, "Scenes.Map01.Terrain.png"),
                    1f);

                var scroll = h.StartScroll(new VoxelSpaceRegionAxonometric(h.ScreenRegion.Rectangle, voxel));

                scroll.Scale.X = (float)Math.Sqrt(2d);
                scroll.Scale.Y = (float)Math.Sqrt(2d);
                scroll.ReScale.Y = 0.5f;


                scroll.SetTarget(h.Add(scroll, new Init(scroll)));

                // m7v.SetCamera(new Pseudo3DWithMouseLookCamera(h.Add(m7v, new Init()))
                // {
                //     //YShearing = -70f,
                //     Height = 360
                // });

                //var bg = new TexturedCubeCilinderView(Textures.Get(RmMvResources.Parallax("Ocean2.png"), false), 4);
                //bg.SwapAllFaces();
                //bg.RemoveShapesWhen(f => f.AllVertices(p => p.Y.In(0.5f, -0.5f)) || f.AllVertices(p => p.Y <= 0f));
                //bg.ScaleTransform(1f, 0.95f, 1f);
                //bg.ScaleTransform(20000000f, 20000000f, 20000000f);

                //m7v.SetBackground(bg);

                // Region.Ambience.SetFog(1000, 15000, Color.White);
                scroll.Ambience.SetLightSource(1, 1, 1);
            }
        }

        class Init : EntityBase<EllipseView>
        {
            private const float rotation = MathHelper.PIF / 128;
            private VoxelSpaceRegionAxonometric voxel;

            public Init(VoxelSpaceRegionAxonometric voxel) : base(new EllipseView(10, 10, Color.Red))
            {
                this.voxel = voxel;
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

                voxel.Rotation = -Angle.Z - MathHelper.HalfPIF;

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

                Location.X = Location.X.Clamp(384, 4096-384);
                Location.Y = Location.Y.Clamp(384, 4096-384);

                Location.Z = voxel.SampleHeight(Location.XY());
                voxel.CameraHeight = (voxel.CameraHeight + Location.Z) * 0.5f - 16f;
            }
        }
    }
}
