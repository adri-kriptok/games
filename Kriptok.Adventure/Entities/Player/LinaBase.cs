using Kriptok.Adventure.Entities.Monsters;
using Kriptok.Adventure.Scenes.Base;
using Kriptok.Adventure.Views.Base;
using Kriptok.Common;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Components;
using Kriptok.Entities.Partitioned;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Regions;
using Kriptok.Regions.Scroll;
using Kriptok.Views.Shapes.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Kriptok.Adventure.Entities.Player.LinaBase;
using static System.Windows.Forms.AxHost;

namespace Kriptok.Adventure.Entities.Player
{
    public enum LinaState
    {
        /// <summary>
        /// Permite realizar acciones y caminar.
        /// </summary>
        Nothing = 0,

        /// <summary>
        /// Animación de blandir un arma.
        /// </summary>
        Slashing = 1,
    }

    public abstract partial class LinaBase : MapEntityBase<LinaView>
    {
        private static readonly float walkingVectorNormalizer = (float)Math.Sqrt(2d);
        private static readonly float invWalkingVectorNormalizer = -walkingVectorNormalizer;
        private const float walkingSpeedMultiplier = Consts.SpeedMultiplier * 0.125f;

        /// <summary>
        /// Validador de ubicación genérico.
        /// </summary>
        private readonly ILocationValidator validator;

        /// <summary>
        /// Último movimiento que intentó hacer.
        /// </summary>
        private Vector2F lastAttempedMovement = Vector2F.Empty;

        /// <summary>
        /// Estado del personaje.
        /// </summary>
        private LinaState state = LinaState.Nothing;

        /// <summary>
        /// Animación de ataque básico (espadas, por ejemplo).
        /// </summary>
        private EventIndexedAnimation slashAnimation = new EventIndexedAnimation(50, new int[] { 3, 4, 5, 5, 5 }, 2);

        /// <summary>
        /// Objeto que realiza la animación.
        /// </summary>
        private SwordSlash slasher;

        /// <summary>
        /// Contador de animación de "caminata".
        /// </summary>
        private float walkingCounter = 0f;

        /// <summary>
        /// Gráfico por defecto para cuando no está realizando ninguna acción.
        /// </summary>
        private int defaultGraph = 0;

        public LinaBase(ILocationValidatorProvider<ITileScrollEntity> provider)
            : base(new LinaView())
        {
            validator = provider.GetLocationValidator(this);
            Radius = 8;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Radius;
            View.Graph = (defaultGraph = View.GetDefaultGraph());

            this.slasher = Add(new SwordSlash(this));
        }

        protected override void OnFrame()
        {
            // Flag para cambiar de gráfico a "caminando".
            var walkingFlag = false;
            var prevLocation = Vector2F.Empty;
            var attempMovement = Vector2F.Empty;

            validator.ValidatingLocation(() =>
            {
                base.ResolvePushes();

                // Tomo la "ubicación previa después de resolver los "empujones"
                // de los demás objetos, para que no afecten la dirección.
                prevLocation = Location.XY();

                switch (state)
                {
                    case LinaState.Nothing:
                        {
                            if (Input.Button03())
                            {
                                slasher.Show();
                                slashAnimation.Restart();
                                state = LinaState.Slashing;

                                var angleVec = Vector2F.Empty;
                                if (Input.Up())
                                {
                                    angleVec.Y = invWalkingVectorNormalizer;
                                }
                                else if (Input.Down())
                                {
                                    angleVec.Y = walkingVectorNormalizer;
                                }
                                if (Input.Left())
                                {
                                    angleVec.X = invWalkingVectorNormalizer;
                                }
                                else if (Input.Right())
                                {
                                    angleVec.X = walkingVectorNormalizer;
                                }

                                if (!angleVec.IsEmpty())
                                {
                                    Angle.Z = angleVec.GetAngle();
                                }
                            }
                            else
                            {
                                if (Input.Up())
                                {
                                    attempMovement.Y = invWalkingVectorNormalizer;
                                }
                                else if (Input.Down())
                                {
                                    attempMovement.Y = walkingVectorNormalizer;
                                }
                                if (Input.Left())
                                {
                                    attempMovement.X = invWalkingVectorNormalizer;
                                }
                                else if (Input.Right())
                                {
                                    attempMovement.X = walkingVectorNormalizer;
                                }

                                if (attempMovement.IsEmpty())
                                {
                                    walkingCounter = 0f;
                                    View.Graph = defaultGraph;
                                }
                                else
                                {
                                    // Actualizo la ubicación.                                    
                                    Location = Location.Plus(attempMovement);
                                    walkingFlag = true;
                                }
                            }
                            break;
                        }
                    case LinaState.Slashing:
                        {
                            if (slashAnimation.Increment(Sys.TimeDelta, out int index, out int g, out bool trigger))
                            {
                                View.Graph = g;
                                slasher.Update(index, trigger);
                            }
                            else
                            {
                                // Sino, quedo a la espera, pero con la animación preparada.
                                View.Graph = (defaultGraph = 3);
                                slasher.Update(0, false);
                                state = LinaState.Nothing;
                            }                            
                            break;
                        }
                }
            });

            if (walkingFlag)
            {                
                slasher.Hide(); // Si me muevo, oculto la espada.
                defaultGraph = 1; // Y cambio el gráfico de descanso.

                var realMovement = Location.XY().Minus(prevLocation);

                if (realMovement.GetNorm() >= 0.1f)
                {
                    Angle.Z = realMovement.GetAngle();
                    View.SetWalkingGraph(walkingCounter += Sys.TimeDelta * walkingSpeedMultiplier);
                }
                else
                {
                    // Si no me moví, pero apunté a una dirección diferente
                    // a la que estoy mirando, igual la giro.
                    if (!lastAttempedMovement.Equals(attempMovement))
                    {
                        Angle.Z = attempMovement.GetAngle();
                    }

                    walkingCounter = 0f;
                    View.Graph = View.GetDefaultGraph();                
                }
            }
            
            lastAttempedMovement = attempMovement;
            base.CheckCollisions();
        }

        public override float GetWeight() => 1f;

        public class LinaView : CharacterViewBase
        {
            public LinaView() : this(Resource.Get(typeof(LinaBase).Assembly, "Lina.Lina.png"))
            {
            }

            private LinaView(Resource resource) : base(resource, 0, 0, new int[4, 6]
            {
                { 00, 01, 02, 12, 13, 14 },
                { 03, 04, 05, 15, 16, 17 },
                { 06, 07, 08, 18, 19, 20 },
                { 09, 10, 11, 21, 22, 23 },
            })
            {                
                base.Add(resource, 3 * 24, 0, 24 * 3, 32 * 4, 3, 4);                
            }
        }        
    }

    public class Lina2D : LinaBase
    {
        public Lina2D(ILocationValidatorProvider<ITileScrollEntity> provider)
            : base(provider)
        {
        }
    }

    //public class Lina3D : LinaBase
    //{
    //    public Lina3D(ILocationValidatorProvider<IPart2DEntity> provider)
    //        : base(provider)
    //    {
    //    }
    //}    
}
