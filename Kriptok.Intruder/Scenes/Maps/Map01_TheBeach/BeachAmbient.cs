using Kriptok.Div;
using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Kriptok.Intruder.Entities;
using Kriptok.Intruder.Entities.Enemies;
using Kriptok.Intruder.Regions;
using Kriptok.Maps;
using Kriptok.Regions.Pseudo3D.Partitioned.Terrain;
using System.Collections.Generic;
using System.Linq;

namespace Kriptok.Intruder.Scenes.Maps.Map01_TheBeach
{
    partial class TheBeachMapScene
    {
        private class BeachAmbient : ExteriorAmbientSound
        {
            private readonly Player player;
            private readonly IPseudo3DTerrainRegion mapRegion;

            /// <summary>
            /// Particiones donde pueden aparecer raptores.
            /// </summary>
            private readonly IDictionary<int, Vector3F> raptorPartitions;

            /// <summary>
            /// Particiones donde pueden aparecer moscas.
            /// </summary>
            private readonly IDictionary<int, Vector3F> flyPartitions;

            /// <summary>
            /// Indica si el jugador ya se acercó lo suficiente a un braquiosaurio.
            /// </summary>
            private bool seenBrachiosaurus = false;

            public BeachAmbient(Player player, IslandBeachRegion mapRegion) : base(player)
            {
                this.player = player;
                this.mapRegion = mapRegion;

                raptorPartitions = mapRegion.GetRaptorPartitions()
                    .ToDictionary(p => p.Id, p => p.GetCenter());
                flyPartitions = mapRegion.GetSmallerPartitions()
                    .ToDictionary(p => p.Id, p => p.GetCenter());
            }

            protected override void OnFrame()
            {
                base.OnFrame();

                if (Rand.Next(0, 65) == 0)
                {
                    // Tengo que agregar un enemigo.
                    if (raptorPartitions.Keys.Contains(player.PartitionId))
                    {
                        // Voy a agregar un raptor.
                        if (Find.All<Raptor>().Count() < 10)
                        {
                            // No puede haber más de 7 raptors juntos.
                            AddRaptor(FindBetterLocation(raptorPartitions));
                        }
                    }
                    else if (flyPartitions.Keys.Contains(player.PartitionId))
                    {
                        // Voy a agregar una mosca.
                        if (Find.All<Fly>().Count() < 15)
                        {
                            // No puede haber más de 10 moscas juntas.
                            AddFly(FindBetterLocation(flyPartitions));
                        }
                    }                    
                }

                if (!seenBrachiosaurus)
                {
                    if (player.SeenBrachiosaurus())
                    {
                        Audio.PlayWave(DivResources.Sound("MusEfect.MUSICA9.WAV"));
                        seenBrachiosaurus = true;
                    }
                }
            }

            private void AddFly(Vector3F location)
            {
                if (!location.Equals(Vector3F.Empty))
                {
                    Add(new Fly(mapRegion, player)
                    {
                        Location = location.Plus(Rand.NextF(-500, 500), Rand.NextF(-500, 500), 0f)
                    });
                }
            }

            private void AddRaptor(Vector3F location)
            {
                if (!location.Equals(Vector3F.Empty))
                {
                    Add(new Raptor(mapRegion, player)
                    {
                        Location = location
                    });
                }
            }

            /// <summary>
            /// Busca la mejor partición para agregar un enemigo.
            /// </summary>            
            private Vector3F FindBetterLocation(IDictionary<int, Vector3F> partitions)
            {
                var playerLoc = player.GetLocation2D();
                var bestLocation = partitions
                    .Select(p => new { p, distance = Vector2F.GetDistance(playerLoc, p.Value.XY()) })                    
                    .OrderBy(p => p.distance)
                    .FirstOrDefault(p => p.distance > 4000f)/*
                    .Take(5)
                    .OrderByRandom()
                    .FirstOrDefault()*/;

                return bestLocation.p.Value;                
            }
        }
    }
}
