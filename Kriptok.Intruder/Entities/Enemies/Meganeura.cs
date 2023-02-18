using Kriptok.Audio;
using Kriptok.Common;
using Kriptok.Div;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Entities.Partitioned;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Maps.Terrains;
using Kriptok.Regions.Partitioned;
using Kriptok.Regions.Partitioned.Wld;
using Kriptok.Regions.Pseudo3D.Partitioned.Terrain;
using Kriptok.Sdk.RM2000;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Intruder.Entities.Enemies
{    
    internal class Meganeura : EntityBase<MeganeuraView>, IEnemy
    {
        private const float damageCounterMax = 500f;

        private readonly IPartitionedPseudo3DRegion map;
        private readonly Player player;
        private TerrainShadow shadow;

        private float swing = 0f;
        private bool hitFlag = false;

        /// <summary>
        /// El daño de las moscas funciona con un contador. Tiene que estar cerca hasta
        /// que se carga el damageCounter, y ahí aplica daño.
        /// </summary>
        private float damageCounter;

        /// <summary>
        /// Indica si esta mosca ya hizo algún sonido.
        /// </summary>
        private bool madeSound = false;

        /// <summary>
        /// Sonidos de la mosca.
        /// </summary>
        private ISoundHandler flySound, attackSound;

        public Meganeura(IPartitionedPseudo3DRegion map, Player player) : base(new MeganeuraView())
        {
            this.map = map;
            View.ScaleX = 1.75f;
            View.ScaleY = 1.75f;
            
            this.player = player;                        
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            View.RotateTransform(343 + Rand.Next(-10, 10));
            
            Radius = 30;
            h.SetCollision3DSphere();

            Location.Z = map.GetFloorHeight(this) + 72;

            if (map is ITerrainRegion terr)
            {
                this.shadow = Add(new TerrainShadow(this, terr, Radius * 0.75f));
            }

            this.flySound = h.Audio.GetWaveHandler(DivResources.Sound("Animales.AVISPA.WAV"));
            this.attackSound = h.Audio.GetWaveHandler(Rm2kResources.Sound("Cancel1.wav"));
        }        

        protected override void OnFrame()
        {          
            if (hitFlag)
            {
                // Add(new FlyExplosion(Location, View.View));
                Die();
                return;
            }

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
            var distToPlayer = GetDistance2D(player);

            // Si el jugador se aleja demasiado, la elimino.
            if (distToPlayer > 6000f)
            {
                Die();
                return;
            }
            else
            {
                if (!madeSound == true &&
                    distToPlayer < 1000f)
                {                    
                    flySound.Play();
                    madeSound = true;
                }
                else if (distToPlayer > 1250f)
                {
                    // Si se alejó más, es como si no lo hubiera hecho.
                    madeSound = false;
                    damageCounter = 0f;
                }

                var rnd = (float)Math.Cos(swing += Rand.NextF(0.05f));

                Angle.X = (float)Math.Sin(swing*2f) * 0.25f;
                View.Rotate();

                Angle.Z = GetAngle2D(player) + rnd;

                if (distToPlayer > 50f)
                {
                    Advance2D(Sys.TimeDelta * 0.5f);
                }
                else
                {
                    damageCounter += Sys.TimeDelta;

                    // Si llego al límite, hago daño.
                    if (damageCounter > damageCounterMax)
                    {                        
                        attackSound.Play();

                        player.Damage(Rand.Next(15, 20));
                        damageCounter = 0f;
                    }
                }

                // -----------------------------------------------------------------------
                // Para que no se superpongan las moscas.
                // -----------------------------------------------------------------------
                if (OnRadiusCollision3D(out DistanceTo3D<Fly> found))
                {
                    found.RejectMeHalfRadius();
                }

                var terrZ = map.GetFloorHeight(this);
                shadow.Location = new Vector3F(Location.XY(), terrZ);
                Location.Z = terrZ + 100f + rnd * 50f;
            }       
        }

        public void Hit(int damage)
        {
            // Las moscas mueren de un tiro.
            hitFlag = true;
        }

#if DEBUG || SHOWFPS
        private bool pointed = false;        

        public void Pointed()
        {
            pointed = true;
        }

#endif

        private class FlyExplosion : EntityBase<IndexedSpriteView>
        {
            private float anim = 0f;

            public FlyExplosion(Vector3F location, IndexedSpriteView view) : base(view)
            {
                Location = location;
                View.Graph = 10;
            }

            protected override void OnFrame()
            {
                anim += Sys.TimeDelta;

                if (anim > 100)
                {
                    anim = 0f;

                    if (View.Graph < 12)
                    {
                        View.Graph++;
                    }
                    else
                    {
                        Die();
                    }
                }
            }
        }
    }

    class MeganeuraView : DirectionalSpriteView
    {
        internal static readonly Resource MeganeuraResource = Resource.Get(typeof(FlyView).Assembly, "Assets.Images.Dinosaurs.Meganeura.png");

        private static readonly int[,] matrix = new int[8, 2]
        {
            {0,  1 },
            {0,  1 },
            {0,  1 },
            {0,  1 },
            {2, -1 },
            {2, -1 },
            {2, -1 },
            {2, -1 }
        };

        public MeganeuraView() : base(MeganeuraResource, 2, 1, matrix)
        {
            View.Center = new PointF(0.5f, 0.5f);
        }
    }
}
