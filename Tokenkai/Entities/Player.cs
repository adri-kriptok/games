using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Queries;
using Kriptok.Helpers;
using Kriptok.Mapping;
using Kriptok.Mapping.Grid;
using Kriptok.Views.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenkai.Entities
{
    internal class Player : EntityBase<DirectionalSpriteView>
    {
        private const float vertMove = 1f;
        private const float horizMove = 2f;
        private static readonly float multiplier = (float)Math.Sqrt(2d) / 2;
        private static readonly float vertDiagMove = vertMove * multiplier;
        private static readonly float horizDiagMove = horizMove * multiplier;

        private static readonly object pathOn = new object();

        private readonly TileMapGrid mapGrid;

        public Player(TileMapGrid mapGrid, Point point) : base(new DirectionalSpriteView(typeof(Player).Assembly, "Assets.Player.png", 11, 9, new int[,]
        {
            /* ▲ */ { +00, +01, +02, +03, +04, +05, +06, +07, +08, +09, +10 },
            /*   */ { +11, +12, +13, +14, +15, +16, +17, +18, +19, +20, +21 },
            /*   */ { +22, +23, +24, +25, +26, +27, +28, +29, +30, +31, +32 },
            /*   */ { +33, +34, +35, +36, +37, +38, +39, +40, +41, +42, +43 },
            /* ► */ { +44, +45, +46, +47, +48, +49, +50, +51, +52, +53, +54 },
            /*   */ { +55, +56, +57, +58, +59, +60, +61, +62, +63, +64, +65 },
            /*   */ { +66, +67, +68, +69, +70, +71, +72, +73, +74, +75, +76 },
            /*   */ { +77, +78, +79, +80, +81, +82, +83, +84, +85, +86, +87 },
            /* ▼ */ { +88, +89, +90, +91, +92, +93, +94, +95, +96, +97, +98 },
            /*   */ { -77, -78, -79, -80, -81, -82, -83, -84, -85, -86, -87 },
            /*   */ { -66, -67, -68, -69, -70, -71, -72, -73, -74, -75, -76 },
            /*   */ { -55, -56, -57, -58, -59, -60, -61, -62, -63, -64, -65 },
            /* ◄ */ { -44, -45, -46, -47, -48, -49, -50, -51, -52, -53, -54 },
            /*   */ { -33, -34, -35, -36, -37, -38, -39, -40, -41, -42, -43 },
            /*   */ { -22, -23, -24, -25, -26, -27, -28, -29, -30, -31, -32 },
            /*   */ { -11, -12, -13, -14, -15, -16, -17, -18, -19, -20, -21 }
        })
        {
            Center = new PointF(0.5f, 0.85f),
            ScaleX = 0.8f,
            ScaleY = 0.8f
        })
        {
            this.mapGrid = mapGrid;
            Location.X = point.X;
            Location.Y = point.Y;            
        }

        private int i = 0;
        private IMouseLocationQuery mouseLocationQuery;
        private Vector2F[] path;

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            mouseLocationQuery = h.GetMouseScrollLocationQuery();
        }

        protected override void OnFrame()
        {
            Vector2F? first = null;
            lock (pathOn) 
            {
                if (path != null && path.Length > 0)
                {
                    first = path[0];
                }
            }

            if (first.HasValue)
            {
                if (HasToMove(Location.XY(), first.Value, out Vector2F movement))
                {
                    // LookAt2D(first.Value);
                    var nextLocation = Location.XY().Plus(movement.Scale(0.5f).Scale(horizMove, vertMove));
                    Angle.Z = PolarVector.AngleAvg(Angle.Z, MathHelper.GetAngleF(Location.XY(), nextLocation));

                    //Move(() =>
                    //{
                    
                        // var newLocation = Vector2F.Average(Location.XY(), Location.XY().Plus(movement.Scale(0.5f).Scale(horizMove, vertMove)));
                        Location.X = nextLocation.X;
                        Location.Y = nextLocation.Y;
                    //});
                }
                else
                {
                    lock (pathOn)
                    {
                        path = path.Skip(1).ToArray();
                    }
                }

                if (i++ > 3)
                {
                    i = 0;
                    View.RotateBetween(0, 7);
                }
            }
            else
            {
                // LookAt2D(mouseLocationQuery.Result);

                Angle.Z = PolarVector.AngleAvg(Angle.Z, MathHelper.GetAngleF(Location.XY(), mouseLocationQuery.Result));

                View.Graph = 8;
            }

            // var moved = Move(() =>
            // {
            //     if (Input.Up())
            //     {
            //         if (Input.Right())
            //         {
            //             Location.X += horizDiagMove;
            //             Location.Y -= vertDiagMove;
            //         }
            //         else if (Input.Left())
            //         {
            //             Location.X -= horizDiagMove;
            //             Location.Y -= vertDiagMove;
            //         }
            //         else
            //         {
            //             Location.Y -= vertMove;
            //         }
            //     }
            //     else if (Input.Down())
            //     {
            //         if (Input.Right())
            //         {
            //             Location.X += horizDiagMove;
            //             Location.Y += vertDiagMove;
            //         }
            //         else if (Input.Left())
            //         {
            //             Location.X -= horizDiagMove;
            //             Location.Y += vertDiagMove;
            //         }
            //         else
            //         {
            //             Location.Y += vertMove;
            //         }
            //     }
            //     else if (Input.Right())
            //     {
            //         Location.X += horizMove;
            //     }
            //     else if (Input.Left())
            //     {
            //         Location.X -= horizMove;
            //     }
            //     else
            //     {
            //         View.Graph = 0;
            //     }
            // });
            // 

            
        }

        private bool HasToMove(Vector2F from, Vector2F to, out Vector2F movement)
        {
            var distX = (to.X - from.X) / horizMove;
            var distY = (to.Y - from.Y) / vertMove;

            movement = new Vector2F(distX, distY);

            return movement.GetNorm() > 1f;
        }

        internal void MoveToCursor()
        {
            var location = mouseLocationQuery.Result.Scale(TokenkaiConsts.InverseTileSize);
            var tileTo = location.Fixed().Clamp(0, 0, mapGrid.Width - 1, mapGrid.Height - 1);
            var path = mapGrid.FindPath(this, tileTo, PathFindTypeEnum._8_Ways);
            
            if (path.Length > 0)
            {
                var newPath = new Vector2F[path.Length];
                var pathLength_1 = path.Length - 1;
                for (int i = 0; i < pathLength_1; i++)
                {
                    var step = path[i];
                    newPath[i] = new Vector2F(
                        step.X * TokenkaiConsts.TileSize + TokenkaiConsts.HalfTileSize, 
                        step.Y * TokenkaiConsts.TileSize + TokenkaiConsts.HalfTileSize);
                }
                var last = path[pathLength_1];
                newPath[pathLength_1] = new Vector2F(
                    last.X * TokenkaiConsts.TileSize + location.X % TokenkaiConsts.TileSize, 
                    last.Y * TokenkaiConsts.TileSize + location.Y % TokenkaiConsts.TileSize);
                
                lock (pathOn)
                {
                    this.path = newPath;
                }
            }    
            else
            {
                lock (pathOn)
                {
                    this.path = null;
                }
            }
        }

        public bool Move(Action action)
        {
            var current = Location.XY();
        
            action();
        
            var after = Location.XY();
        
            if (!after.Equals(current))
            {
                var aX = after.X;//  / tileLength;
                var aY = after.Y;//  / tileLength;
                var r = (float)Radius;// / (float)tileLength;
        
                if (!mapGrid.Valid(this, aX, aY))//, r))
                {
                    if (mapGrid.Valid(this, aX, current.Y /*/ tileLength*/))//, r))
                    {
                        Location.Y = current.Y;
                        return true;
                        // PostMoved();
                    }
                    else if (mapGrid.Valid(this, current.X /*/ tileLength*/, aY))//, r))
                    {
                        Location.X = current.X;
                        return true;
                        // PostMoved();
                    }
                    else
                    {
                        Location.Set(current);
                        return false;
                    }
                }
                else
                {
                    return true;
                    // PostMoved();
                }
            }
            return false;
        }
    }
}
