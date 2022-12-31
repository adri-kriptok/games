using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Debug;
using Kriptok.Helpers;
using Kriptok.Intruder.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Entities
{
    internal class DinosaurSenses : LineOfSight
    {
        private readonly Raptor dinosaur;
        private readonly Player player;
        
        private readonly float sightDistance;
        private readonly float hearDistance;
        private readonly float smellDistance;

        /// <summary>
        /// Indica si ya vio al jugador en algún momento.
        /// </summary>
        private bool hasSeenPlayer = false;

        /// <summary>
        /// Indica si escuchó un sonido.
        /// </summary>
        private bool heardNoise = false;

        /// <summary>
        /// Ubicación bidimensional de dónde proviene el sonido escuchado.
        /// </summary>
        private Vector2F noiseLocation;

        public DinosaurSenses(Raptor dinosaur, Player player,
            float sightDistance, float hearDistance, float smellDistance) 
            : base(dinosaur, player)
        {
            this.dinosaur = dinosaur;
            this.player = player;
            this.sightDistance = sightDistance;
            this.hearDistance = hearDistance;
            this.smellDistance = smellDistance;

            // Indico al jugador que cuando haga ruido, me avise.
            player.AddListener(this);
        }

        /// <summary>
        /// Variable que indica si está al tanto ahora mismo del jugador.
        /// </summary>
        internal bool PerceivingThePlayer { get; private set; } = false;

        /// <summary>
        /// Distancia calculada al jugador.
        /// </summary>
        internal float DistanceToPlayer { get; private set; } = 0f;

        //internal bool PerceivesThePlayer()
        //{
        //    // // Primero, me fijo si está a una distancia en que lo vea.
        //    // if (Vector3F.GetDistance(dinosaur.Location, player.Location) < sightDistance)
        //    // {
        //    //     // Si ya lo vio, listo.
        //    //     if (hasSeenPlayer)
        //    //     {
        //    //         return true;
        //    //     }
        //    // 
        //    //     // De lo contrario me fijo si está en la línea de visión.
        //    //     if (base.IsInSight(player))
        //    //     {
        //    //         return hasSeenPlayer = true;
        //    //     }
        //    // 
        //    //     return false;
        //    // }
        //    // else
        //    // {
        //    //     // Si se alejó lo suficiente, lo pierde de vista.
        //    //     return hasSeenPlayer = false;
        //    // }
        //}

        /// <summary>
        /// Busca al tirador.
        /// </summary>
        internal void FindShooter()
        {
            PerceivingThePlayer = true;
            dinosaur.LookAt2D(player);
        }

        /// <summary>
        /// Actualiza el estado de los sentidos del dinosaurio.
        /// </summary>        
        internal void Update()
        {
            DistanceToPlayer = Vector3F.GetDistance(dinosaur.Location, player.Location);

            // Si ya vio al jugador, igual lo puede perder de vista.
            if (hasSeenPlayer)
            {
                if (DistanceToPlayer > sightDistance)
                {
                    // Listo, lo perdió de vista.
                    PerceivingThePlayer = false;
                }                
            }
            else
            {
                // Primero me fijo si estoy a distancia de ser olido.
                if (DistanceToPlayer < smellDistance)
                {
                    if (!PerceivingThePlayer)
                    {
                        dinosaur.Scream();
                    }

                    // Es fácil, si te huele, listo.
                    PerceivingThePlayer = true;
                }
                else if (heardNoise)
                {
                    // Apunto a donde se escuchó el sonido.
                    dinosaur.LookAt2D(noiseLocation);
                    
                    // Y saco el flag.
                    heardNoise = false;
                }
                else if (DistanceToPlayer < sightDistance && IsInSight(player))
                {
                    if (!PerceivingThePlayer)
                    {
                        dinosaur.Scream();
                    }

                    // Está a distancia de verlo.
                    // Y le da el ángulo de visión, listo.
                    PerceivingThePlayer = true;                    
                }
            }

            // Limpio la variable que indica que el usuario hizo un sonido.
            heardNoise = false;
        }

        /// <summary>
        /// Informa que una entidad ha hecho un sonido.
        /// </summary>        
        /// <param name="emitter">Emisor del sonido.</param>
        internal void InformSound(EntityBase emitter)
        {
            if (Vector3F.GetDistance(emitter.GetRenderLocation(), dinosaur.GetRenderLocation()) < hearDistance)
            {
                heardNoise = true;
                noiseLocation = emitter.GetLocation2D();
            }
        }

        /// <summary>
        /// Indica si la entidad que escucha está aún viva.
        /// </summary>        
        internal bool IsAlive() => dinosaur.IsAlive();
    }
}
