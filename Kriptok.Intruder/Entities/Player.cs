using Kriptok.Div;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Partitioned;
using Kriptok.Entities.Terrain;
using Kriptok.Entities.Wld;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Intruder.Entities.Effects;
using Kriptok.Intruder.Entities.Enemies;
using Kriptok.Intruder.Entities.Hud;
using Kriptok.Intruder.Entities.ScreenWeapons;
using Kriptok.Intruder.Maps;
using Kriptok.Intruder.Scenes.Maps;
using Kriptok.Regions.Base;
using Kriptok.Regions.Partitioned;
using Kriptok.Regions.Partitioned.Wld;
using Kriptok.Regions.Pseudo3D;
using Kriptok.Regions.Pseudo3D.Cameras;
using Kriptok.Regions.Pseudo3D.Partitioned.Terrain;
using Kriptok.Regions.Pseudo3D.Partitioned.Wld;
using Kriptok.Views.Shapes.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Intruder.Entities
{
    partial class Player : TerrainEntityBase
    {
        private const float rotation = MathHelper.PIF / 128;
        private const float swingInc = 0.1f;
        private const float jumpEnergyConsumption = 100f;
        private const float minusHalf = -0.5f;
        private readonly IPartitionedPseudo3DRegion partitionedMap;
        private readonly Region3DBase region;

        /// <summary>
        /// Arma en pantalla.
        /// </summary>
        internal MsDosJpRifle Weapon;

        /// <summary>
        /// Cantidad de vida que tiene el jugador.
        /// </summary>
        private float life = BarBase.MaxValue;

        /// <summary>
        /// Cantidad de energía que tiene el jugador para correr y saltar.
        /// </summary>
        private float energy = BarBase.MaxValue;

        /// <summary>
        /// Para mover el arma en pantalla.
        /// </summary>
        public static float SwingCos = 0f;

        /// <summary>
        /// Movimiento acumulado del arma.
        /// </summary>
        private float swing;

        /// <summary>
        /// Textura del piso sobre el cuál está el jugador.
        /// </summary>
        private int groundTextureId = 0;

        /// <summary>
        /// Id de la partición donde se encuentra el jugador.
        /// </summary>
        internal int PartitionId { get; private set; } = 0;

        /// <summary>
        /// Indica si el jugador está en el agua.
        /// </summary>
        private bool isInWater;

        /// <summary>
        /// Indica si puede saltar o no.
        /// </summary>
        private bool readyToJump;

        /// <summary>
        /// Lista de dinosaurios que pueden escuchar cuando disparo.
        /// </summary>
        private readonly IList<DinosaurSenses> listeners = new List<DinosaurSenses>();

        // private bool deadScream = false;

        public Player(IPseudo3DWldRegion map) : base(map)
        {
            this.partitionedMap = map;
            this.region = (Region3DBase)map;
            Location = new Vector3F(0, 0, 0f);
            this.Weapon = new MsDosJpRifle(this);

            Radius = 16;
        }

        public Player(IPseudo3DTerrainRegion map) : base(map)
        {
            this.partitionedMap = map;
            this.region = (Region3DBase)map;
            Location = new Vector3F(0, 0, 0f);
            this.Weapon = new MsDosJpRifle(this);

            Radius = 16;
        }

        /// <summary>
        /// Indica cuánta munición queda para la escopeta.
        /// </summary>
        internal int ShotgunAmmo => Weapon.Ammo;

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            partitionedMap.SetCamera(new Cam(this));

            //Location.X = 1996.9436f;
            //Location.Y = 889.969849f;
        }

        protected override void OnFrame(FramePhysicsHandler physicsHandler)
        {
            if (life <= 0)
            {
                // if (!deadScream)
                // {
                    Audio.PlayWave(DivResources.Sound("Humanos.AAAH01.WAV"));
                //}
                //deadScream = true;
                Die();
                Scene.SendMessage(MapMessages.PlayerDied);
                return;
            }

            // Me fijo si está en el agua. Esto define la altura a la que debe estar la cámara.
            isInWater = ExteriorTexset.InWater(groundTextureId);

            // Si está en el agua, los movimientos están restringidos.
            if (isInWater)
            {
                energy = 0f;
            }

            var runButton = IsRunning();
            var running = energy >= 1f && runButton;

            // Guardo la posición antes de moverme.
            var beforeMoving = Location;

            var timeDelta = physicsHandler.TimeDelta * 0.03125f;

            if (Input.L1())
            {
                Angle.Z -= rotation * timeDelta;
            }
            else if (Input.R1())
            {
                Angle.Z += rotation * timeDelta;
            }

            var moved = false;

            if (Input.Down())
            {
                // Advance2D(-GetSpeed(running) * timeDelta * 0.5f);

                physicsHandler.Advance2D(GetSpeed(running) * minusHalf);

                moved = true;
            }
            else if (Input.Up())
            {
                // Advance2D(GetSpeed(running) * timeDelta);

                physicsHandler.Advance2D(GetSpeed(running));

                moved = true;
            }

            if (Input.Left())
            {
                physicsHandler.Strafe2D(GetSpeed(running) * minusHalf);
                moved = true;
            }
            else if (Input.Right())
            {
                physicsHandler.Strafe2D(GetSpeed(running) * 0.5f);
                moved = true;
            }

#if !DEBUG
            if (energy >= jumpEnergyConsumption)
            {
#endif
                if (Input.Button03())
                {
                    if (readyToJump)
                    {
                        if (physicsHandler.Jump(1f))
                        {
                            energy -= jumpEnergyConsumption;
                            readyToJump = false;
                        }
                    }
                }
                else
                {
                    readyToJump = true;
                }
#if !DEBUG
            }
#endif

            if (moved)
            {
                PostMoved(running);

                if (isInWater && RelativeZ == 0f && Rand.Next(0, 10) > 8)
                {
                    // if (Input.Key(Keys.J))
                    // {
                    Add(new WaterWave(this, beforeMoving));
                    //}
                }
            }
            Angle.Z += region.RotationWithMouseHorizontally();

            if (Mouse.Left)
            {
                if (Weapon.Shoot())
                {
                    // Agrego el disparo.
                    Add(new PlayerShot(this, partitionedMap));

                    // Y además emito el sonido.
                    foreach (var item in listeners.ToList())
                    {
                        // Primero me fijo si el objeto no está muerto.
                        if (!item.IsAlive())
                        {
                            // Y lo remuevo de la lista.
                            listeners.Remove(item);
                        }
                        else
                        {
                            // Y después le informo para que escuche.
                            item.InformSound(this);
                        }
                    }
                }
            }

            if (moved)
            {
                if (runButton)
                {
                    energy = Math.Max(0f, energy - 1f);
                }
                
                energy = Math.Min(BarBase.MaxValue, energy + 0.3f);
                
            }
            else
            {
                life = Math.Min(BarBase.MaxValue, life + 0.1f);
                energy = Math.Min(BarBase.MaxValue, energy + 3f);
            }
        }

        private float GetSpeed(bool running)
        {
#if DEBUG || SHOWFPS
            if (Mouse.Right)
            {
                return 5f;
            }
#endif
            if (isInWater)
            {
                return 0.075f;
            }
            else if (running)
            {
                //Trace.WriteLine($"{energy} - {DateTime.Now.Ticks} run");
                return 0.6f;
            }
            return 0.15f;
        }

        protected void PostMoved(bool running)
        {
            //base.PostMoved();
            swing += swingInc;

            if (running)
            {
                swing += swingInc;
            }

            SwingCos = SinCosHelper.TabCosF(swing * 0.5f);
        }

        private bool IsRunning() => Input.Button04();

        public override uint GetStepHeight() => 24;

        public override uint GetHeight() => IsAlive() ? (isInWater ? 24u : 72u) : 0u;

        public override float MaxSlopeAbleToClimb() => IntruderConsts.MaxSlopePlayerAbleToClimb;

        internal bool IsOnShore()
        {
            return groundTextureId.In(ExteriorTexset.SandTexture, ExteriorTexset.OceanTexture);
        }

        public override void UpdateData(TerrainEntityDataUpdate data)
        {
            base.UpdateData(data);

            PartitionId = data.PartitionId;
            groundTextureId = data.GroundTextureId;
        }

        public override void UpdateData(WldEntityDataUpdate data)
        {
            base.UpdateData(data);

            PartitionId = data.PartitionId;
            groundTextureId = data.FloorTextureId;
        }
        
        /// <summary>
        /// Obtiene la cantidad de vida (0..1) que tiene el jugador.
        /// </summary>        
        internal float GetLife() => life / BarBase.MaxValue;

        /// <summary>
        /// Obtiene la cantidad de energía (0..1) que tiene el jugador.
        /// </summary>        
        internal float GetEnergy() => energy / BarBase.MaxValue;

        /// <summary>
        /// Daña al jugador.
        /// </summary>        
        internal void Damage(int damage)
        {
            life = Math.Max(0f, life - damage);
        }

        /// <summary>
        /// Dispara la música épica cuando ve por primera vez un dinosaurio.
        /// </summary>        
        internal bool SeenBrachiosaurus()
        {
            var brach = FindClose3D<Brachiosaurus>(20000);
            if (brach.Length > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Agrega dinosaurios que pueden escuchar los disparos del usuario a la lista.
        /// </summary>        
        internal void AddListener(DinosaurSenses dinosaurSenses) => listeners.Add(dinosaurSenses);

        /// <summary>
        /// Indica si tiene que mostrar el arma lista para disparar.
        /// </summary>        
        internal bool WeaponUp()
        {
            if (isInWater)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Cámara para la vista en primera persona.
        /// </summary>
        private class Cam : Pseudo3DWithMouseLookCameraBase
        {
            private readonly Player target;
            private float height;

            public Cam(Player target) : base(target)
            {
                this.target = target;
#if DEBUG
                Distance = 120;
#endif
            }

            public override float GetCameraHeight()
            {                
                var nextHeight = target.GetLocationZ() + target.GetHeight();
                
                height = height + (nextHeight - height) * 0.5f;
                return height;
            }

            public override float GetYShearing(Pseudo3DRenderContext context, float tiltSin, float tiltCos)
            {
                // Actualizo el ángulo vertical del jugador, en base al Y-Shearing.
                target.Angle.Y = GetVerticalAngle();

                return base.GetYShearing(context, tiltSin, tiltCos);
            }
        }
    }
}
