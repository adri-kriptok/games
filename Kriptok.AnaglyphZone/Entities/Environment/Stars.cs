//using Kriptok.Drawing;
//using Kriptok.Drawing.Algebra;
//using Kriptok.Entities.Base;
//using Kriptok.Extensions;
//using Kriptok.Regions.Context.Base;
//using Kriptok.Vector3D.Scenes;
//using Kriptok.Vector3D.Views;
//using Kriptok.Views.Base;
//using Kriptok.Views.Shapes;
//using Kriptok.Views.Shapes.Base;
//using Kriptok.Views.Shapes.Vertices;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Kriptok.Vector3D.Entities.Stars;
//using static Kriptok.Vector3D.Scenes.EnclosedScene;

//namespace Kriptok.Vector3D.Entities
//{
//    internal class Stars : EntityBase<StarsView>
//    {
//        private readonly CamTargetBase cam;

//        public Stars(CamTargetBase camTarget) : base(new StarsView())
//        {
//            this.cam = camTarget;
//            Location.Z = Vector3DConsts.CamHeight;
//        }       

//        protected override void OnFrame()
//        {
//            //if (cam.Location.X > 0)
//            //{
//            //    Die();
//            //}

//           // var diff = cam.Location.X - Location.X;
//           // if (diff >= Vector3DConsts.GridSize)
//           // {
//           //     Location.X += Vector3DConsts.GridSize;
//           // }
//        }        

//        internal class StarsView : GdipShapeView
//        {
//            protected override bool IsConvex() => true;

//            public StarsView() : base(Array.Empty<VertexBase>(), shapes =>
//            {
//                var vertex0 = new Vertex3(0f, 0f, 0f);

//                var list = new List<VertexBase>();

//                var distLayer = 2000;                

//                for (int i = 0; i < 200; i++)
//                {
//                    shapes.Add(new StarParticle(//Strokes.Fuchsia, // vertex0,
//                        new Vertex3(-distLayer,
//                        Rand.NextF(-distLayer / 2f * 1.3f, distLayer / 2f * 1.3f),
//                        Rand.NextF(-distLayer * 16f / 9f, distLayer * 16f / 9f))));
//                }

//                distLayer = 4000;                
//                for (int i = 0; i < 200; i++)
//                {
//                    shapes.Add(new StarParticle(//Strokes.Fuchsia, // vertex0,
//                        new Vertex3(-distLayer,
//                        Rand.NextF(-distLayer / 2f * 1.3f, distLayer / 2f * 1.3f),
//                        Rand.NextF(-distLayer * 16f / 9f, distLayer * 16f / 9f))));
//                }
                
//                distLayer = 8000;
//                for (int i = 0; i < 200; i++)
//                {
//                    shapes.Add(new StarParticle(//Strokes.Fuchsia, // vertex0,
//                      new Vertex3(-distLayer,
//                      Rand.NextF(-distLayer / 2f * 1.3f, distLayer / 2f * 1.3f),
//                      Rand.NextF(-distLayer * 16f / 9f, distLayer * 16f / 9f))));
//                }
//            })
//            {
//            }

//            public override float GetPriority() => float.MinValue;

//            internal class StarParticle : ParticleBase
//            {
//                private readonly VertexBase vertex = null;
//                private readonly Pen pen = Pens.Fuchsia;

//                public StarParticle(VertexBase vertex) : base(vertex)
//                {
//                    this.vertex = vertex;
//                }
            
//                public override float ScaleX { get => 1f; set => throw new NotImplementedException(); }
//                public override float ScaleY { get => 1f; set => throw new NotImplementedException(); }
            
//                public override IShape Clone(IDictionary<VertexBase, VertexBase> clonedVertices)
//                {
//                    throw new NotImplementedException();
//                }
            
//                public override void Render2D(IRenderContext2D context)
//                {
//                    throw new NotImplementedException();
//                }
            
//                public override void Render3D(IRenderContext3D context)
//                {
//                    var p = context.ProjectV(vertex);
//                    context.Graphics.DrawRectangle(pen, (int)p.X, (int)p.Y, 1, 1);
//                }
            
//                protected override BoundF2 RecalculateRenderBoundingBox(IProjector projector, Vector3F location)
//                {
//                    var proj = projector.ProjectV(location);
//                    return new BoundF2()
//                    {
//                        MinX = proj.X,
//                        MaxX = proj.X + 1f,
//                        MinY = proj.Y,
//                        MaxY = proj.Y + 1f,
//                    };                    
//                }
//            }
//        }
//    }
//}
