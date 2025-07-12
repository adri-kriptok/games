using Kriptok.Drawing.Algebra;
using Kriptok.Helpers;
using Kriptok.JazzJackRabbit.Entities.Weapons;
using Kriptok.JazzJackRabbit.Maps;
using Kriptok.JazzJackRabbit.Scenes.Base;
using Kriptok.Mapping.Tiles;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Views.Gdip;
using Kriptok.Views.Primitives;
using Kriptok.Views.Shapes;
using Kriptok.Views.Sprites;
using System;
using System.Diagnostics;
using System.Drawing;
using Kriptok.JazzJackRabbit.Core.Wapons;
using Kriptok.JazzJackRabbit.Core;

namespace Kriptok.JazzJackRabbit.Entities
{
    public enum JazzStatusEnum
    {
        /// <summary>
        /// En tierra.
        /// </summary>
        Grounded = 0,

        /// <summary>
        /// Saltando o cayendo.
        /// </summary>
        OnTheAir = 1,
        
        /// <summary>
        /// En tierra, disparando.
        /// </summary>
        GroundShooting = 2
    }

    class Jazz : EntityBase<IndexedSpriteView>
    {
        private const int HeadModifier = -16;
        private const int FeetModifier = 16;
        private const int FrontModifierX12 = 12;
        private const int FrontModifierY12 = -4;

        private const int walkSpeed = 3;
        internal const int RunSpeed = walkSpeed * 2;

        private static readonly int[] walkingCycle = IntHelper.Iota(8, 13);
        private static readonly int[] runCycle = IntHelper.Iota(16, 20);

        /// <summary>
        /// Nivel actual en el que está el objeto.
        /// </summary>
        private readonly LevelSceneBase level;

        /// <summary>
        /// Arma que tiene seleccionada actualmente.
        /// </summary>
        private readonly WeaponBase weapon;

        /// <summary>
        /// Estado actual del objeto.
        /// </summary>
        private JazzStatusEnum state = JazzStatusEnum.Grounded;

        /// <summary>
        /// Controlador de caídas y saltos.
        /// </summary>
        private readonly JumpFallHandlerY jumpFallHandler = new JumpFallHandlerY(9.81f);

        /// <summary>
        /// Utilizado para calcular el gráfico de renderizado al caminar.
        /// </summary>
        private float walkingIndex = 0f;

        public Jazz(LevelSceneBase level) : base(new IndexedSpriteView(typeof(Jazz).Assembly, "Assets.Entities.Jazz.png", 8, 9))
        {
            this.level = level;
            this.weapon = new Toaster(this);
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

#if DEBUG
            Add(new FeetVertex(this, 0));
            //Add(new FeetVertex(this, FrontModifier));
            //Add(new FeetVertex(this, -FrontModifier));
            Add(new HeadVertex(this));
            Add(new FrontVertex(this));
            Add(new LowerTile(this));
#endif
        }

        protected override void OnFrame()
        {
            if (state == JazzStatusEnum.GroundShooting)
            {
                // Si estoy disparando parado, pero apreto algún botón
                // de movimiento, rompo lo que estoy haciendo.
                if(Input.Left() || Input.Right() || JumpButton())
                {
                    state = JazzStatusEnum.Grounded;
                }
                else 
                { 
                    ShootingGrounded();                   
                }
            }

            switch (state)
            {
                case JazzStatusEnum.Grounded:
                    Grounded();
                    break;
                case JazzStatusEnum.OnTheAir:
                    JumpingFalling();
                    break;
                // case JazzStatusEnum.GroundShooting:
                //     ShootingGrounded();
                //     break;
            }
        }

        private void Grounded()
        {
#if DEBUG
            var runningButton = RunButton();
#endif
            if (JumpButton())
            {
                state = JazzStatusEnum.OnTheAir;
                jumpFallHandler.StartJump(FeetY, 45f);
                return;
            }
            else if (Input.Right())
            {
                View.Flip = FlipEnum.None;
                if (RunButton())
                {
                    Run(TileFlagsEnum.BlockedFromLeft, RunSpeed);
                }
                else
                {
                    Walk(TileFlagsEnum.BlockedFromLeft, walkSpeed);
                }
            }
            else if (Input.Left())
            {
                View.Flip = FlipEnum.FlipX;
                if (RunButton())
                {
                    Run(TileFlagsEnum.BlockedFromRight, -RunSpeed);
                }
                else
                {
                    Walk(TileFlagsEnum.BlockedFromRight, -walkSpeed);
                }
            }
            else if (weapon.ReadyToShoot() && ShootButton())
            {                
                state = JazzStatusEnum.GroundShooting;
                weapon.Shoot();
            }
            else
            {
                View.Graph = 0;
            }

            var feetY = FeetY;
            int lowerPlatform = GetLowerPlatform(FeetY);// FeetY /*+ FrontModifierY12*/);

            if (lowerPlatform < feetY)
            {
                // Por acá entra cuando "sube un escalón".

                //if (state == JazzStatusEnum.Standing)
                //{
                    Location.Y = lowerPlatform - FeetModifier;
                //}
                // Trace.WriteLine($"{Location.X}; {Location.Y}");
            }
            else if (lowerPlatform > feetY)
            {
                // Este if es lo que hace que cuando avanzás por una diagonal no "caigas",
                // por la diferencia de altitud.
                if (lowerPlatform - feetY > RunSpeed)
                {
                    StartFalling(feetY);
                }
                else
                {
                    Location.Y = lowerPlatform - FeetModifier;
                }
            }
        }

        private static bool ShootButton() => Input.Button04();        

        private static bool JumpButton() => Input.Button03();        

        private static bool RunButton()
        {
            return true; // Input.L1() || Input.Up();;
        }

        private void Walk(TileFlagsEnum wallFlag, int direction)
        {
            MoveHorizontally(() => Location.X += direction, wallFlag, false);
            walkingIndex += 0.25f;
            View.Graph = walkingCycle[((int)walkingIndex) % walkingCycle.Length];
        }

        private void Run(TileFlagsEnum wallFlag, int direction)
        {
            MoveHorizontally(() => Location.X += direction, wallFlag, false);
            
            walkingIndex += 0.25f;
            View.Graph = runCycle[((int)walkingIndex) % runCycle.Length];
        }

        private void StartFalling(float feetLocation)
        {
            state = JazzStatusEnum.OnTheAir;
            jumpFallHandler.StartFall(feetLocation);
        }

        private void JumpingFalling()
        {
#if DEBUG
            var ticks = DateTime.Now.Ticks;
#endif
            if (jumpFallHandler.Status == JumpFallStatusEnum.Jumping)
            {
                View.Graph = 40;

                var upperPlatform = Math.Max(
                    level.GetUpperPlatformY(Location.X, FeetY) - HeadModifier + FeetModifier,
                    level.GetUpperPlatformY(FrontX, FrontY) - HeadModifier + FeetModifier);

                if (jumpFallHandler.IncTimer(JazzConsts.Timer, float.MaxValue, upperPlatform, out float newHeight))
                {
                    Location.Y = newHeight - FeetModifier;
                }
            }
            else if (jumpFallHandler.Status == JumpFallStatusEnum.Falling)
            {
                View.Graph = 41;
                
                int lowerPlatform = GetLowerPlatform(FeetY);

                if (jumpFallHandler.IncTimer(JazzConsts.Timer, lowerPlatform, float.MinValue, out float newHeight))
                {
#if DEBUG
                    if (newHeight == lowerPlatform)
                    {
                    }
#endif

                    Location.Y = newHeight - FeetModifier;
#if DEBUG
                    if (newHeight == lowerPlatform)
                    {
                        // Para DEBUG.
                    }
#endif
                }
#if DEBUG
                else
                {
                    // Cuando corto la caída.
                }
#endif
            }

            if (jumpFallHandler.Status == JumpFallStatusEnum.None)
            {
                state = JazzStatusEnum.Grounded;
                //Trace.WriteLine($"{ticks} - Standing");
            }
            else
            {
                if (Input.Right())
                {
                    View.Flip = FlipEnum.None;

                    if (RunButton())
                    {
                        MoveHorizontally(() => Location.X += 4, TileFlagsEnum.BlockedFromLeft, true);
                    }
                    else
                    {
                        MoveHorizontally(() => Location.X += 2, TileFlagsEnum.BlockedFromLeft, true);
                    }
                }
                else if (Input.Left())
                {
                    View.Flip = FlipEnum.FlipX;
                    if (RunButton())
                    {
                        MoveHorizontally(() => Location.X -= 4, TileFlagsEnum.BlockedFromRight, true);
                    }
                    else
                    {
                        MoveHorizontally(() => Location.X -= 2, TileFlagsEnum.BlockedFromRight, true);
                    }
                }
                else
                {
                    CheckShootingOnMovement();
                }
            }
        }

        private int GetLowerPlatform(float feetY)
        {           
            return level.GetLowerPlatformY(Location.X, feetY);
        }

        private float FeetY => Location.Y + FeetModifier;

        private float FrontX
        {
            get
            {
                //if (state == JazzStatusEnum.Falling)
                //{
                //    return Location.X;
                //}
                //else 
                if (View.Flip == FlipEnum.FlipX)
                {
                    return Location.X - FrontModifierX12;
                }
                else
                {
                    return Location.X + FrontModifierX12;
                }
            }
        }

        private float FrontY 
        {
            get
            {
                //if (state == JazzStatusEnum.Standing)
                //{
                    return Location.Y + FrontModifierY12;
                //}
                //else
                //{
                //     return FeetY;
                //}
            }
        }

#if DEBUG
        private float HeadY => Location.Y + HeadModifier;        

        internal class FeetVertex : EntityBase<EllipseView>
        {
            private readonly Jazz jazz;
            private readonly int modifierX;

            public FeetVertex(Jazz jazz, int modifierX) : base(new EllipseView(5, 5, Color.Orange))
            {
                this.jazz = jazz;

                Location.Z = -99999;
                this.modifierX = modifierX;
            }

            public override Vector3F GetRenderLocation()
            {
                var b = jazz.GetRenderLocation();
                b.X += modifierX;
                b.Y = jazz.FeetY;
                return b;
            }

            protected override void OnFrame()
            {                
            }
        }

        internal class HeadVertex : EntityBase<EllipseView>
        {
            private readonly Jazz jazz;

            public HeadVertex(Jazz jazz) : base(new EllipseView(5, 5, Color.Orange))
            {
                this.jazz = jazz;

                Location.Z = -99999;
            }

            public override Vector3F GetRenderLocation()
            {
                var b = jazz.GetRenderLocation();
                b.Y = jazz.HeadY;
                return b;
            }

            protected override void OnFrame()
            {
            }
        }

        internal class FrontVertex : EntityBase<EllipseView>
        {
            private readonly Jazz jazz;

            public FrontVertex(Jazz jazz) : base(new EllipseView(5, 5, Color.Orange))
            {
                this.jazz = jazz;

                Location.Z = -99999;
            }

            public override Vector3F GetRenderLocation()
            {
                var b = jazz.GetRenderLocation();

                b.X = jazz.FrontX;
                b.Y = jazz.FrontY;
                return b;
            }

            protected override void OnFrame()
            {
            }
        }

        private class LowerTile : EntityBase<RectangleView>
        {
            private readonly Jazz jazz;

            public LowerTile(Jazz jazz) : base(
                new RectangleView(JazzConsts.TileSize-1, JazzConsts.TileSize-1, null, Strokes.White))
            {
                this.jazz = jazz;

                View.Center = new PointF(0f, 0f);
            }

            protected override void OnFrame()
            {
                // var jl = jazz.GetRenderLocation();
                // 
                // if (jazz.level.GetTileFlags(LowerTileX(jl), LowerTileY(jl)).HasFlag(JazzTilesetCustomFlags.DiagonalDown) ||
                //     jazz.level.GetTileFlags(LowerTileX(jl), LowerTileY(jl)).HasFlag(JazzTilesetCustomFlags.DiagonalUp))
                // {
                // 
                // }
            }

            public override Vector3F GetRenderLocation()
            {
                var jl = jazz.GetRenderLocation();

                return new Vector3F(LowerTileX(jl), LowerTileY(), 0f);
            }

            private int LowerTileY()
            {
                return ((((int)jazz.FeetY) / JazzConsts.TileSize)) * JazzConsts.TileSize;
            }

            private static int LowerTileX(Vector3F jl)
            {
                return (((int)jl.X) / JazzConsts.TileSize) * JazzConsts.TileSize;
            }
        }
#endif

        private void MoveHorizontally(Action action, TileFlagsEnum wallFlag, bool onTheAir)
        {
            // Guardo la posición actual.
            var current = Location;

            // Muevo el personaje.
            action();

            if (level.OutOfBoundsX(FrontX))
            {
                // Vuelvo atrás el movimiento.
                Location = current;
                return;
            }

            // Me fijo si estoy disparando.
            CheckShootingOnMovement();

            if (onTheAir)
            {
                //// Me fijo si quedó en una posición válida.            
                //if (!level.IsValid(wallFlag, FrontX, Location.Y) ||
                //    !level.IsValid(wallFlag, FrontX, FeetY))
                //{
                //    // Vuelvo atrás el movimiento.
                //    Location = current;
                //}

                // Me fijo si quedó en una posición válida.            
                if (!level.IsValid(wallFlag, FrontX, Location.Y))
                {
                    // Vuelvo atrás el movimiento.
                    Location = current;
                }
                else
                {
                    var height = level.GetLowerPlatformY(FrontX, Location.Y);

                    if (height < Location.Y && (Location.Y - height) > RunSpeed)
                    {
                        // Vuelvo atrás el movimiento.
                        Location = current;
                    }
                }
            }
            else
            {
                //-------------------------------------------------------------------------------------
                // Control para poder subir y bajar por las diagonales.
                //-------------------------------------------------------------------------------------
                var flags = level.SampleTileFlags(Location.X, FeetY);

                if (flags.HasFlag(JazzTilesetCustomFlags.DiagonalDown) ||
                    flags.HasFlag(JazzTilesetCustomFlags.DiagonalUp))
                {
                    return;
                }
                //-------------------------------------------------------------------------------------

                // Me fijo si quedó en una posición válida.            
                if (!level.IsValid(wallFlag, FrontX, Location.Y))
                {
                    // Vuelvo atrás el movimiento.
                    Location = current;
                }
            }            
        }

        private void CheckShootingOnMovement()
        {
            if (weapon.ReadyToShoot() && ShootButton())
            {
                weapon.Shoot();
            }
        }

        private void ShootingGrounded()
        {
            switch (weapon.GetState())
            {
                case 13:
                    View.Graph = 28;
                    return;
                case 9:
                    View.Graph = 27;
                    weapon.Shoot();
                    break;                
                case 4:
                    View.Graph = 0;
                    return;                
                case 0:
                    state = JazzStatusEnum.Grounded;
                    return;
            }
        }

        internal void Blaster(Vector3F location, FlipEnum flip) => Add(new BlasterShot(location, flip));

        internal void Toaster(Vector3F location, FlipEnum flip) => Add(new ToasterShot(location, flip));       
    }
}
