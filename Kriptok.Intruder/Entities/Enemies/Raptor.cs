using Kriptok.Common;
using Kriptok.Div;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Entities.Debug;
using Kriptok.Entities.Partitioned;
using Kriptok.Entities.Terrain;
using Kriptok.Entities.Wld;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Maps.Terrains;
using Kriptok.Regions.Context.Base;
using Kriptok.Regions.Partitioned;
using Kriptok.Regions.Partitioned.Wld;
using Kriptok.Regions.Pseudo3D.Partitioned.Terrain;
using Kriptok.Regions.Pseudo3D.Partitioned.Wld;
using Kriptok.Sdk.RM.VX;
using Kriptok.Sdk.RM2000;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Kriptok.Intruder.Entities.Enemies
{
    internal enum VelociraptorStatusEnum 
    {
        Idle = 0,
        Chasing = 1,
        Stunned = 2,
        Jumping = 3,
    }

    internal class Raptor : TerrainEntityBase<Raptor.RaptorView>, IEnemy
    {        
        private readonly IPartitionedPseudo3DRegion map;
        private readonly Player player;
        private TerrainShadow shadow;

        private VelociraptorStatusEnum status;

        /// <summary>
        /// Contador de tiempo que el velociraptor pasa en estado "atontado"
        /// luego de recibir un disparo.
        /// </summary>
        private float stunnedCounter;

        /// <summary>
        /// Estilo de ataque. Si es zigzag hacia un lado, hacia el otro, o en línea recta.
        /// </summary>
        private float attackStyle;
        private float walkingIndex = 0f;
        private int rotation;
        private float swing = 0f;
        private uint height;

        /// <summary>
        /// Vida actual del velociraptor.
        /// </summary>
        private int life = 100;

        /// <summary>
        /// Indica si ha recibido un disparo.
        /// </summary>
        private bool hitFlag = false;

        /// <summary>
        /// Objeto para determinar líneas de visión de la entidad.
        /// </summary>
        private readonly DinosaurSenses senses;

        /// <summary>
        /// Contador de tiempo hasta que ataca al jugador cuando está cerca.
        /// </summary>
        private float scratchCounter = 0f;

        public Raptor(IPseudo3DTerrainRegion map, Player player) : base(map, new RaptorView())
        {
            this.map = map;
            this.player = player;
#if DEBUG            
            this.senses = new DinosaurSenses(this, player, 10000f, 14000f, 200f);
#else
            this.senses = new DinosaurSenses(this, player, 10000f, 14000f, 200f);
#endif
        }

        public Raptor(IPseudo3DWldRegion map, Player player) : base(map, new RaptorView())
        {
            this.map = map;
            this.player = player;
#if DEBUG            
            this.senses = new DinosaurSenses(this, player, 10000f, 14000f, 200f);
#else
            this.senses = new DinosaurSenses(this, player, 10000f, 14000f, 200f);
#endif
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            status = VelociraptorStatusEnum.Idle;

            Angle.Z = (float)Math.Acos(Math.Cos(Location.X + Location.Y));
            this.rotation = Math.Sign(Math.Tan(Location.X + Location.Y));

            View.ScaleX = 1.75f + MathHelper.CosF(Location.X) * 0.15f;
            View.ScaleY = View.ScaleX;

            Radius = (ushort)(20 * View.ScaleX);
            height = (uint)(64 * View.ScaleY);

            h.SetCollision3DAACilinder();

            if (map is ITerrainRegion terr)
            {
                this.shadow = Add(new TerrainShadow(this, terr, Radius));                
            }
        }

        protected override void OnFrame(FramePhysicsHandler handler)
        {            
            // Si el jugador se aleja demasiado, la elimino.
            if (GetDistance2D(player) > 15000f)
            {
                Die();
                return;
            }

            // Si le dieron un tiro al raptor.
            if (hitFlag)
            {
                if (life <= 0)
                {
                    if (handler.Inertia.Z == 0f)
                    {
                        //Audio.PlayWave(DivResources.Sound("Animales.BESTIA02.WAV"));
                        Scream(DivResources.Sound("Animales.BESTIA02.WAV"));
                        Add(new DyingRaptor(this, player));
                        Die();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {                    
                    Scream(DivResources.Sound("Animales.BESTIA04.WAV"));

                    // ... Le indico a la vista que se ponga en rojo.
                    View.Hit();

                    // Empujo el velociraptor hacia atrás.
                    XAdvance2D(50f, Location.XY().Minus(player.Location.XY()).GetAngle());

                    // Lo giro un pco para demostrar lo atontado que queda.
                    Angle.Z = Angle.Z + Rand.NextF(-1f, 1f);
                    ResetAttackStyle();

                    hitFlag = false;
                }
            }
            else
            {
#if DEBUG || SHOWFPS
                if (IntruderConsts.Freeze)
                {
                    if (pointed)
                    {
                        View.Transform = new ColorTransform()
                        {
                            A = 0.5f,
                            R = new ColorVector(255, 0, 0),
                            G = new ColorVector(255, 0, 0),
                            B = new ColorVector(255, 0, 0)
                        };
                    }
                    else
                    {
                        View.Transform = ColorTransform.Identity;
                    }

                    pointed = false;
                    return;
                }
#endif
                // Resuelvo todos los empujones de los otros raptors.
                ResolvePushes();

                // Actualizo el estado de mis sentidos.
                senses.Update();

                // Si no encuentro al jugador, me mantengo en estado "perdiendo el tiempo".
                if (!senses.PerceivingThePlayer)
                {
                    status = VelociraptorStatusEnum.Idle;                    
                }

                switch (status)
                {
                    case VelociraptorStatusEnum.Idle:
                        {
                            Angle.Z += 0.01f * rotation;
                            Advance(handler, 1f);

                            // Me fijo si el jugador está dentro del área de visión.
                            if (senses.PerceivingThePlayer)
                            {
                                if (Rand.Next(1, 8) == 8)
                                {
                                    Scream(DivResources.Sound("Animales.FIERA03.WAV"));
                                }
                                status = VelociraptorStatusEnum.Chasing;

                                // Cuando cambio de estado, calculo cómo va a atacar.
                                ResetAttackStyle();                                
                            }
                        }
                        break;
                    case VelociraptorStatusEnum.Chasing:
                        {
                            var rnd = (float)Math.Cos(swing += Rand.NextF(0.1f)) * attackStyle;

                            //Angle.Z = GetAngle2D(player);
                            Angle.Z = GetAngle2D(player) + rnd;

                            if (attackStyle != 0f)
                            {
                                if (Rand.Next(0, 30) == 0)
                                {
                                    ResetAttackStyle();
                                }

                                // Si ya estoy muy cerca, corre en línea recta.
                                if (senses.DistanceToPlayer < 500f)
                                {
                                    attackStyle = 0f;
                                }                                
                            }

                            if (senses.DistanceToPlayer < 1000f &&
                                senses.DistanceToPlayer > 200f &&
                                Rand.Next(0, 20) == 0)
                            {
                                if (Rand.Next(1, 5) == 5)
                                {
                                    Scream(DivResources.Sound("Animales.FIERA03.WAV"));
                                }
                                LookAt2D(player);
                                handler.Jump(Rand.NextF(0.9f, 1f));
                                handler.Advance2D(Rand.NextF(1f, 1.5f) * senses.DistanceToPlayer * 0.002f);
                                status = VelociraptorStatusEnum.Jumping;
                            }
                            else
                            {                                
                                if (Radius2DCollision(player, out DistanceTo2D<Player> found))
                                {
                                    found.RejectMeHalfRadius();

                                    if ((scratchCounter -= handler.TimeDelta) <= 0f)
                                    {
                                        var snd = Rand.Next(1, 9);
                                        if (snd >= 6) snd += 2;
                                        Audio.PlaySound(Rm2kResources.Sound($"Kill{snd}.wav"));
                                        player.Damage(Rand.Next(30, 60));
                                        scratchCounter = Rand.NextF(10, 20) * handler.TimeDelta;
                                    }

                                    WalkCycle(10f);
                                }
                                else
                                {
                                    // A medida que se aleja el jugador, me muevo más rápido, para no dejarlo ir.
                                    XAdvance(handler, Math.Max(20f, (float)found.Distance / 400f), Angle.Z + rnd);
                                }

                                // CheckCollisionsWithRaptors();
                            }
                            break;
                        }
                    case VelociraptorStatusEnum.Stunned:
                        {
                            // Acá no hace nada, va para cualquier lado.
                            var rnd = (float)Math.Cos(swing += Rand.NextF(0.1f)) * attackStyle * 0.01f;

                            Angle.Z = Angle.Z + rnd;

                            // Y avanzo para donde estoy mirando.
                            Advance(handler, 2f);

                            if ((stunnedCounter -= handler.TimeDelta) <= 0)
                            {
                                status = VelociraptorStatusEnum.Idle;
                            }
                            
                            break;
                        }
                    case VelociraptorStatusEnum.Jumping:
                        {
                            var inertia = handler.Inertia;
                            if (inertia.Z == 0f)
                            {
                                View.Graph = 0;
                                status = VelociraptorStatusEnum.Chasing;
                                break;
                            }

                            var z = inertia.Normalized().Z;

                            if (z > 0f)
                            {
                                // Está yendo para arriba.
                                if (z > 0.5f)
                                {
                                    View.Graph = 6;
                                }
                                else
                                {
                                    View.Graph = 7;
                                }
                            }
                            else if (z < 0f)
                            {
                                // Está yendo para arriba.
                                if (z < -0.5f)
                                {
                                    View.Graph = 9;
                                }
                                else
                                {
                                    View.Graph = 8;
                                }
                            }

                            if (Radius2DCollision(player, out DistanceTo2D<Player> found))
                            {
                                handler.Stop();
                            }
                            break;
                        }
                }
                CheckCollisionsWithRaptors();
            }
        }

        private void Scream(Resource resource)
        {
            Audio.PlayWave(resource);

            // Si reproduce un sonido, tengo que avisar a los raptores de alrededor.
            foreach (var raptor in FindCloseEntities2D<Raptor>())
            {
                raptor.Entity.InformSound(this);
            }
        }

        private void ResetAttackStyle()
        {
            attackStyle = Rand.Next(-1, 1);
        }

        private void CheckCollisionsWithRaptors()
        {
#if DEBUG
            if (GetDistance2D(player) < 100)
            {
            }
#endif
            // -----------------------------------------------------------------------
            // Para que no se superpongan con otros raptors.
            // -----------------------------------------------------------------------
            if (OnRadiusCollision2D(out DistanceTo2D<Raptor> found))
            {
#if DEBUG
                if (found.Distance < Radius)
                {
                }
#endif
                // Sólo puedo aplicar el rechazo al objeto que evalúa la colisión,
                // ya que el otro no está en un "contexto seguro" del mapa particionado.
                found.RejectMeHalfRadius();
                found.Entity.AddPush(this);
            }
        }

        private readonly IList<Raptor> pushes = new List<Raptor>();

        private void AddPush(Raptor raptor)
        {
            this.pushes.Add(raptor);
        }

        private void ResolvePushes()
        {
            foreach (var item in pushes)
            {
                if (Radius2DCollision(item, out DistanceTo2D<Raptor> found))
                {
                    found.RejectMeHalfRadius();
                }
            }
            pushes.Clear();
        }

        public override void UpdateData(TerrainEntityDataUpdate data)
        {
            base.UpdateData(data);
                        
            shadow.Location = new Vector3F(Location.XY(), data.FloorHeight);
        }

        public override uint GetHeight() => height;

        private void Advance(FramePhysicsHandler handler, float speed)
        {
            handler.Advance2D(0.015f * speed);
            WalkCycle(speed);
        }

        private void XAdvance(FramePhysicsHandler handler, float speed, float direction)
        {
            handler.XAdvance2D(0.02f * speed, direction);
            WalkCycle(speed);
        }

        private void WalkCycle(float speed)
        {
            walkingIndex += speed;
            if (walkingIndex > 40f)
            {
                // View.Rotate();
                View.RotateBetween(0, 5);
                walkingIndex = 0f;
            }
        }

        /// <summary>
        /// Método que dispara el arma cuando le acierta un tiro al dinosaurio.
        /// </summary>
        public void Hit(int damage)
        {
            // Si me pegaron un tiro, busco al jugador inmediatamente.
            senses.FindShooter();

            hitFlag = true;

            // Lo pongo en estado "bobo" por un rato.
            status = VelociraptorStatusEnum.Stunned;
            stunnedCounter = Rand.Next(15, 40) * Sys.TimeDelta;

            life -= damage;
        }

#if DEBUG || SHOWFPS
        private bool pointed = false;

        public void Pointed()
        {
            pointed = true;
        }
#endif

        internal void Scream()
        {
            // Audio.PlaySound(RmVxResources.Sound(""))
            // Audio.PlayWave(DivResources.Sound("Animales.BESTIA10.WAV"));
            Scream(DivResources.Sound("Animales.BESTIA10.WAV"));
        }

        /// <summary>
        /// Informa al raptor que se ha emitido un sonido.
        /// </summary>        
        private void InformSound(EntityBase emitter) => senses.InformSound(emitter);

        public class RaptorView : DirectionalSpriteView
        {
            internal static readonly Resource Resource = Resource.Get(typeof(RaptorView).Assembly, "Assets.Images.Dinosaurs.Velociraptor.png");

            private static readonly int[,] matrix = new int[4, 10]
            {
                {   6,   7,   8,   9,  10,  11, 18, 19, 20, 21 },
                { -12, -13, -14, -15, -16, -17, 18, 19, 20, 21 },
                {   0,   1,   2,   3,   4,   5, 18, 19, 20, 21 },
                {  12,  13,  14,  15,  16,  17, 18, 19, 20, 21 }
            };

            internal static readonly ColorTransform DefaultColoration = new ColorTransform()
            {
                A = 1f,
                R = new ColorVector(0.3f, 0f, 0f),
                G = new ColorVector(0.59f, 0.59f, 0.59f),
                B = new ColorVector(0f, 0f, 0.11f)
            };

            internal readonly ColorTransform InitialTransform;

            /// <summary>
            /// Cantidad de rojo a aplicar al dinosaurio cuando se renderiza después de ser golpeado.
            /// </summary>
            private float red = 0f;

            public RaptorView() : base(Resource, 0, 0, 288, 64, 12, 1, matrix)
            {
                View.Center = new PointF(0.5f, 0.975f);

                InitialTransform = DefaultColoration;
                InitialTransform.Rotate(Rand.Next(-10, 30));
                
                Transform = InitialTransform;

                Add(Resource, 0, 64, 288, 128, 3, 2);
                
                for (int i = 12; i < 18; i++)
                {
                    SetCenter(i, new PointF(0.25f, 0.975f));
                }

                Add(Resource, 0, 200, 48 * 4, 56, 4, 1);

                for (int i = 18; i < 22; i++)
                {
                    SetCenter(i, new PointF(0.5f, 0.975f));
                }
            }

            /// <summary>
            /// Le indica a la vista que el velociraptor fue golpeado.
            /// </summary>
            public void Hit()
            {
                red = 0.5f;
            }

            public override void RenderOn(IRenderContext context)
            {
                if (red > 0f)
                {
                    var newTransform = InitialTransform;
                    // newTransform.R.R += red;
                    // //newTransform.R.G += red;
                    // //newTransform.R.B += red;
                    // newTransform.G.R += red;
                    // //newTransform.G.G += red;
                    // //newTransform.G.B += red;
                    // newTransform.B.R += red;
                    // //newTransform.B.G += red;
                    // //newTransform.B.B += red;

                    newTransform.Add.R += red;
                    newTransform.Add.G -= red;
                    newTransform.Add.B -= red;

                    View.Transform = newTransform;

                    base.RenderOn(context);

                    // Cuando renderizo, voy achicando la cantidad de rojo.
                    red -= 0.05f;
                }
                else
                {
                    View.Transform = InitialTransform;
                    red = 0f;
                    base.RenderOn(context);
                }
            }
        }

        /// <summary>
        /// Instancia del velociraptor cuando está muriendo.
        /// </summary>
        internal class DyingRaptor : EntityBase<DyingRaptor.DyingRaptorView>
        {
            private readonly Player player;
            private float frameCounter = 0f;
            
            public DyingRaptor(Raptor raptor, Player player) : base(new DyingRaptorView(RaptorView.Resource))
            {
                this.player = player;
                Location = raptor.Location;
                Angle = raptor.Angle;
                View.Scale = raptor.View.Scale;
                View.Transform = raptor.View.InitialTransform;
            }

            protected override void OnFrame()
            {            
                // Si el jugador se aleja demasiado, la elimino.
                if (GetDistance2D(player) > 10000f)
                {
                    Die();
                    return;
                }

                if (View.Graph < 4)
                {
                    frameCounter += Sys.TimeDelta;

                    if(frameCounter > 100f)
                    {
                        frameCounter = 0f;
                        View.Graph++;
                    }
                }
            }

            internal class DyingRaptorView : DirectionalSpriteView
            {
                private const float oneThird = 1f / 3f;
                private const float fourElevenths = 4f / 11f;
                private const float twoThirds = 2f / 3f;

                private static readonly int[,] matrix = new int[4, 5]
                {
                    {  0,  1,  2,  5,  6 },
                    {  7, -1, -2, -5, -6 },
                    { -3, -4, -2, -5, -6 },
                    {  3,  4,  2,  5,  6 }
                };

                public DyingRaptorView() : this(RaptorView.Resource)
                {
                    Transform = RaptorView.DefaultColoration;
                }                

                public DyingRaptorView(Resource resource) : base(resource, 0, 256, 48, 64, 1, 1, matrix)
                {                    
                    /* Agrego un solo sprite nativamente, y el resto a mano. */
                    // Add(resource, 0, 256, 48, 64);
                    Add(resource, 48, 256, 72, 64);
                    Add(resource, 120, 264, 88, 56);

                    Add(resource, 0, 320, 48, 64);
                    Add(resource, 48, 320, 72, 64);
                    Add(resource, 120, 344, 72, 40);

                    Add(resource, 192, 360, 72, 24);

                    Center = new PointF(0.5f, 0.975f);

                    SetCenter(1, new PointF(oneThird, Center.Y));
                    SetCenter(4, new PointF(oneThird, Center.Y));
                    SetCenter(2, new PointF(fourElevenths, Center.Y));
                    SetCenter(5, new PointF(twoThirds, Center.Y));
                }
            }
        }
    }
}
