using Kriptok.Drawing;
using Kriptok.Pokefight.Common;
using Kriptok.Helpers;
using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Views.Gdip;
using Kriptok.Views.Sprites;
using System;
using Kriptok.Entities;
using Kriptok.Audio;

namespace Kriptok.Pokefight.Processes.Base
{
    abstract class PokemonBase : ProcessBase<IndexedSpriteView>
    {
        private readonly string bitmapPath;
        private readonly string alternatePalletteBitmapPath;

        public int LifePoints;
        private PokemonBase rival;
        private readonly ControlConfig controls;

        public int PokemonX;
        public int PokemonY;        

        // Stand By.            
        private readonly int standByMaxIndex;
        private readonly int standByMinIndex;
        private float standByIndexModifier;
        private float standByViewIndex;        
        
        // Walk.        
        private readonly int[] walkIndexes;
        private float walkIndex;
        private ISoundHandler blowSound;
        private ISoundHandler evasSound;

        // Jump.
        private const int floorY = 265;
        private const float jumpTempInterval = 0.375f * 3f / 4f;

        private readonly int jumpingUpGraphIndex;
        private readonly int jumpingDownGraphIndex;

        // Punching.
        private readonly int punchGraphIndex;
        private readonly int duckGraphIndex;
        private int jumpCharger;
        private readonly int deadGraphIndex;

        protected abstract int AttackMovement { get; }        

        /// <summary>
        /// Controlador de salto.
        /// </summary>
        private readonly JumpFallHandlerZ jumpFallHandler = new JumpFallHandlerZ(9.81f);

        public PokemonBase(ControlConfig controls,
            string bitmapPath, string alternatePalletteBitmapPath,
            int standByMinIndex, int standByMaxIndex,
            int jumpingUpGraphIndex, int jumpingDownGraphIndex,
            int punchGraphIndex, int duckGraphIndex, int deadGraphIndex)   
            : base(GetView(bitmapPath))          
        {
            View.ScaleX = 1f;
            View.ScaleY = 1f;
            this.walkIndexes = GetWalingIndexes();            

            Location.Y = floorY;

            LifePoints = 100;

            this.bitmapPath = bitmapPath;
            this.alternatePalletteBitmapPath = alternatePalletteBitmapPath;
            this.controls = controls;

            this.standByMinIndex = standByMinIndex;
            this.standByMaxIndex = standByMaxIndex;
            this.standByIndexModifier = 0.2f;
            this.standByViewIndex = 0;
            this.PokemonY = 0;
            
            this.jumpingUpGraphIndex = jumpingUpGraphIndex;
            this.jumpingDownGraphIndex = jumpingDownGraphIndex;
            this.jumpCharger = 0;

            this.punchGraphIndex = punchGraphIndex;

            this.duckGraphIndex = duckGraphIndex;

            this.deadGraphIndex = deadGraphIndex;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);

            blowSound = h.Audio.GetWaveHandler("Resources.Sound.Blow3.wav");
            evasSound = h.Audio.GetWaveHandler("Resources.Sound.Evasion2.wav");
        }

        internal abstract int[] GetWalingIndexes();

        private static IndexedSpriteView GetView(string bitmap)
        {
            //// Si es el jugador 2, y son del mismo tipo de pokemon, uso la paleta alternativa.
            //if (x > 500 && rival.GetType().Equals(GetType()))
            //{
            //    return new IndexedSpriteView(typeof(PokemonBase).Assembly, alternative, 4, 5);
            //}
            //else
            //{
                return new IndexedSpriteView(typeof(PokemonBase).Assembly, bitmap, 4, 5);
            //}

        }

        public void SetStart(PokemonBase rival, int x)
        {
            Location.X = x;
            this.rival = rival;
           
            standByViewIndex = standByMinIndex;
        }

        protected override void OnBegin()
        {
            While(() => LifePoints > 0, () =>
            {
                StandBy();

                if (controls != null)
                {
                    if (Input.Key(controls.Down))
                    {
                        Duck();
                    }
                    else
                    {
                        if (Input.Key(controls.Up))
                        {
                            Jump();
                        }
                        else if (Input.Key(controls.Right))
                        {
                            WalkRight();
                        }
                        else if (Input.Key(controls.Left))
                        {
                            WalkLeft();
                        }
                        else if (Input.Key(controls.Punch))
                        {
                            Punch(() => StandBy());
                        }

                        if (jumpCharger > 0)
                        {
                            jumpCharger--;
                        }
                    }
                }

                AdjustPokemonX();
                AdjustPokemonY();

                OnBeforeFrame();

                Frame();
            });

            Loop(() =>
            {
                View.Graph = deadGraphIndex;
                AdjustPokemonX();

                Location.Y = floorY + 10;

                Frame();
            });
        }

        protected virtual void OnBeforeFrame()
        {            
        }

        private void AdjustPokemonX()
        {
            PokemonX = (int)Location.X;            
            if (rival != null)
            {
                if (Math.Abs(PokemonY - rival.PokemonY) < 25)
                {
                    if (Location.X < rival.Location.X)
                    {
                        if ((rival.Location.X - Location.X) < 37)
                        {
                            Location.X -= 4;
                            rival.Location.X += 1;
                        }
                    }
                    else if (Location.X > rival.Location.X)
                    {
                        if (Location.X - rival.Location.X < 37)
                        {
                            Location.X += 4;
                            rival.Location.X -= 1;
                        }
                    }
                    else
                    {
                        Location.X += 3;
                        rival.Location.X -= 3;
                    }
                }                
            }

            if (Location.X < 25)
            {
                Location.X = 25;
            }
            else if (Location.X > 975)
            {
                Location.X = 975;
            }
        }

        protected virtual void AdjustPokemonY()
        {
            if (PokemonY < 0)
            {
                PokemonY = 0;
            }

            Location.Y = floorY - PokemonY;
        }

        protected virtual void WalkLeft()
        {
            if (walkIndex > 0)
            {
                walkIndex = 0;
            }
            else
            {
                walkIndex -= 0.25f;
            }

            if ((int)walkIndex < -(walkIndexes.Length - 1))
            {
                walkIndex = 0;
            }

            Location.X -= 5;
            AutoFlip();

            View.Graph = walkIndexes[-(int)walkIndex];
        }

        protected virtual void Punch(Action afterPunch, Action onFrame = null, Func<bool> afterPunchCondition = null)
        {
            var lastGraph = View.Graph;
            View.Graph = punchGraphIndex;

            var movement = (View.Flip == FlipEnum.FlipX) ? AttackMovement : -AttackMovement;

            Location.X += movement;
            if (onFrame != null) { onFrame(); }
            Frame();
            Location.X += movement;
            if (onFrame != null) { onFrame(); }
            Frame();
            if (onFrame != null) { onFrame(); }

            CheckPunch();

            Frame();
            Location.X -= movement;
            if (onFrame != null) { onFrame(); }
            Frame();
            if (onFrame != null) { onFrame(); }
            Frame();
            Location.X -= movement;
            if (onFrame != null) { onFrame(); }
            Frame();


            View.Graph = lastGraph;

            if (onFrame != null) { onFrame(); }
            Frame();
            if (onFrame != null) { onFrame(); }
            Frame();

            While(() => afterPunchCondition == null || afterPunchCondition(), () =>
            {
                afterPunch();
                if (!Input.Key(controls.Punch))
                {
                    return false;
                }
                return Frame();
            });
        }

        private void CheckPunch()
        {
            if (Math.Abs(PokemonY - rival.PokemonY) < 13 && Math.Abs(rival.Location.X - Location.X) < 38)
            {                
                blowSound.Play();
                rival.LifePoints -= Rand.Next(5, 6);
            }
            else
            {                
                evasSound.Play();
            }
        }

        protected virtual void WalkRight()
        {
            if(walkIndex < 0)
            {
                walkIndex = 0;
            }
            else
            {
                walkIndex += 0.25f;
            }

            if ((int)walkIndex > walkIndexes.Length - 1)
            {
                walkIndex = 0;
            }

            Location.X += 5;
            AutoFlip();

            View.Graph = walkIndexes[(int)walkIndex];
        }

        protected virtual void Duck()
        {
            View.Graph = duckGraphIndex;

            if(jumpCharger < 15)
            {
                jumpCharger++;
            }            
        }

        protected virtual void Jump()
        {            
            jumpFallHandler.StartJump(0f, GetJumpInitialSpeed());

            While(() => jumpFallHandler.Status != JumpFallStatusEnum.None, () =>
            {
                View.Graph = WhileJumping(true);

                AdjustPokemonX();

                if (Input.Key(controls.Punch))
                {
                    Punch(() => WhileJumping(false), () => WhileJumping(true), () => PokemonY > 0);
                }

                AutoFlip();

                return Frame();
            });

            jumpCharger = 0;
        }

        protected abstract float GetJumpInitialSpeed();

        private int WhileJumping(bool allowMovement)
        {
            if (jumpFallHandler.IncTimer(jumpTempInterval, 0f, float.MaxValue, out float newHeight))
            {
                PokemonY = (int)newHeight;

                if (allowMovement)
                {
                    if (Input.Key(controls.Left))
                    {
                        Location.X -= 5;
                    }
                    else if (Input.Key(controls.Right))
                    {
                        Location.X += 5;
                    }
                }

                Location.Y = floorY - newHeight;
            }

            switch (jumpFallHandler.Status)
            {
                case JumpFallStatusEnum.Jumping:
                    return jumpingUpGraphIndex;
                case JumpFallStatusEnum.Falling:
                    return jumpingDownGraphIndex;
            }
            return 0;
        }

        private void StandBy()
        {
            standByViewIndex += standByIndexModifier;

            int asInt = 0;
            if (standByIndexModifier > 0)
            {
                asInt = (int)Math.Truncate(standByViewIndex);
            }
            else
            {
                asInt = (int)Math.Truncate(standByViewIndex);
            }

            if (asInt > standByMaxIndex || asInt < standByMinIndex)
            {
                standByIndexModifier = -standByIndexModifier;

                if (asInt >= standByMaxIndex)
                {
                    asInt = standByMaxIndex;
                }
                else if (asInt <= standByMinIndex)
                {
                    //asInt = (Random.Next(1, 10) < 5) ? standByMinIndex : (standByMaxIndex + 1);
                    // El parpadeo no funcionaba para Pikachu porque el gráfico está en otro index... paja...
                    asInt = standByMinIndex;
                }
            }

            View.Graph = asInt;
            
            AutoFlip();
        }

        private void AutoFlip()
        {
            if (rival != null)
            {
                View.Flip = (Location.X > rival.Location.X) ? FlipEnum.None : FlipEnum.FlipX;
            }
        }
    }
}
