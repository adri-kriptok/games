using Kriptok.Audio;
using Kriptok.Drawing;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Extensions;
using Kriptok.Games.Alien.Entities.Enemies;
using Kriptok.Games.Alien.Regions;
using Kriptok.Games.Alien.Scenes;
using Kriptok.Views.Sprites;
using System;
using System.Drawing;
using static Kriptok.Games.Alien.Scenes.LevelScene;

namespace Kriptok.Games.Alien.Entities
{
    internal class RobotLegs : ProcessBase<IndexedSpriteView>
    {
        public int contador_pasos = 0;   // Controla los graficos
        public int siguiente_x_mapa;   // Siguiente coordenada x en el mapa
        public int siguiente_y_mapa;   // Siguiente coordenada y en el mapa        
        public int x_mapa;             // Coordenada X en el mapa
        public int y_mapa;             // Coordenada Y en el mapa
        public int incremento_x;       // Incremento X
        public int incremento_y;       // Incremento Y

        /// <summary>
        /// Numero de los graficos.
        /// </summary>
        private int numero_grafico;

        /// <summary>
        /// Indica dónde esá el mapa actualmente.
        /// </summary>
        private readonly AlienScrollTarget scrollCam;

        /// <summary>
        /// Mapa de durezas actual.
        /// </summary>
        private readonly FastBitmap8 hardnesses;

        public RobotLegs(AlienScrollTarget scrollCam, FastBitmap8 hardnesses)
            : base(new IndexedSpriteView(typeof(RobotLegs).Assembly, "Assets.Images.RobotLegs.png", 5, 8))
        {
            View.Center = new PointF(0.5f, 11f / 45f);
            this.scrollCam = scrollCam;
            this.hardnesses = hardnesses;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = Collision2DTypeEnum.Auto;
        }

        protected override void OnBegin()
        {
            // Inicializa la energ¡a del robot
            Global.energia_robot = Global.Consts.MaxInitialHealth;

            // Inicializa coordenadas del mapa.
            x_mapa = 180;
            y_mapa = Global.Consts.InitialRobotLocationY;

            // Repite hasta que se quede sin vidas
            While(() => Global.Lives >= 0, () =>
            {
                // Variables temporales para el incremento
                incremento_x = 0;
                incremento_y = 0;

                numero_grafico = 0;

                // Identificador para eliminar
                var robot = Add(new Robot());
                var se_mueve = false;

                Repeat(() =>
                {
                    // Reinicializa las variables de incremento
                    incremento_x = 0;
                    incremento_y = 0;

                    // Control de teclas
                    if (Input.Up()) { incremento_y = -5; }
                    if (Input.Down()) { incremento_y = 5; }
                    if (Input.Left()) { incremento_x = -5; }
                    if (Input.Right()) { incremento_x = 5; }

                    se_mueve = true;

                    switch ((incremento_y * 10) + incremento_x) // Selecciona el grafico de las piernas
                    {
                        case 0: se_mueve = false; break;
                        case -5: numero_grafico = 2; break;  // Para la izquierda
                        case 5: numero_grafico = 6; break;   // Para la derecha
                        case -50: numero_grafico = 4; break; // Para la arriba
                        case 50: numero_grafico = 0; break;  // Para la abajo
                        case -55: numero_grafico = 3; break; // Para la izquierda - arriba
                        case -45: numero_grafico = 5; break; // Para la derecha - arriba
                        case 45: numero_grafico = 1; break;  // Para la izquierda - abajo
                        case 55: numero_grafico = 7; break;  // Para la derecha - abajo
                    }

                    // Mira la posicion en el mapa
                    siguiente_x_mapa = x_mapa + incremento_x;
                    siguiente_y_mapa = y_mapa + incremento_y;

                    var obstaculo = false;
                    // Comprueba los obstaculos                    
                    if (hardnesses.Sample((ushort)(siguiente_x_mapa / 2), (ushort)Math.Min(1449, siguiente_y_mapa / 2 + 12)) == 24)
                    {
                        obstaculo = true;
                    }

                    // Mueve al robot si se puede mover
                    if (se_mueve != false && obstaculo == false)
                    {
                        // Cambia el grafico de las piernas
                        if (contador_pasos < 4)
                        {
                            contador_pasos = contador_pasos + 1;
                        }
                        else
                        {
                            contador_pasos = 1;
                        }

                        if (contador_pasos == 1)
                        {
                            Audio.PlayMidiNote(MidiInstrumentEnum.SynthDrum, 1, 33, 127);
                        }
                        else if (contador_pasos == 3)
                        {
                            Audio.PlayMidiNote(MidiInstrumentEnum.SynthDrum, 1, 32, 127);
                        }

                        // Movimiento horizontal
                        if ((incremento_x > 0 && x_mapa < 350) || (incremento_x < 0 && x_mapa > 10))
                        {
                            if ((incremento_x > 0 && scrollCam.X < 40 && x_mapa - scrollCam.X > 60) ||
                                (incremento_x < 0 && scrollCam.X > 0 && x_mapa - scrollCam.X < 260))
                            {
                                scrollCam.X += incremento_x;
                            }
                            x_mapa = siguiente_x_mapa;
                        }

                        // Congela la cámara si hay helicópteros.
                        var ableToMove = Find.First<HelicopterBase>() == null;

                        // Movimiento vertical
                        if ((incremento_y > 0 && y_mapa < 2890 && ((ableToMove || Global.enemigo_f == 0) || y_mapa < scrollCam.Y + 190))
                        || (incremento_y < 0 && y_mapa > scrollCam.Y && y_mapa > scrollCam.Y + 10))
                        {
                            if ((incremento_y > 0 && scrollCam.Y < 2700 && y_mapa - scrollCam.Y > 190)
                            || (incremento_y < 0 && scrollCam.Y > 0 && y_mapa - scrollCam.Y < 150) && (ableToMove || Global.enemigo_f == 0))
                            {                                
                                scrollCam.Y += incremento_y;                                
                            }
                            y_mapa = siguiente_y_mapa;
                        }
                    }
                    else
                    {
                        contador_pasos = 0;
                    }

                    // Actualiza la posicion del grafico del robot al completo
                    Location.X = x_mapa;
                    Location.Y = y_mapa;
                    robot.Location.X = Location.X;
                    robot.Location.Y = Location.Y;
                    // Actualiza el grafico de las piernas
                    View.Graph = (numero_grafico * 5) + contador_pasos;
                    Frame();
                }, () => Global.energia_robot <= 0);     // Repite hasta que muera

                Frame();

                // Elimina el cuerpo del robot y explota
                robot.Die();
                Add(new Explosion2(Location.Plus(0f, -16f, 0f), 0.5f));

                Frame();

                // Las piernas del robot andando
                if (se_mueve)
                {
                    // Cambia el contador de animaciones
                    if (contador_pasos < 4)
                    {
                        contador_pasos = contador_pasos + 1;
                    }
                    else
                    {
                        contador_pasos = 1;
                    }

                    View.Graph = (numero_grafico * 5) + contador_pasos;

                    // // Actualiza coordenadas
                    // Location.X = Location.X + get_distx(angulo0, incremento_x);
                    // Location.Y = Location.Y + get_disty(angulo0, incremento_y);

                    // Da un pantallazo con pausa                    
                    Frame(10);
                }

                // Quita una vida
                Global.Lives--;

                // Comprueba si se ha queda sin vidas
                if (Global.Lives >= 0)
                {
                    // Reinicia la energía.
                    Global.energia_robot = Global.Consts.MaxInitialHealth;

                    // // Restaura la barra de energ¡a
                    // foreach (var bar in Find.All<EnergyBar>()) bar.Die();
                    // Add(new EnergyBar());                    
                }
                else
                {
                    // write(fuente_j,160,100,4,"FIN DEL JUEGO");
                    Scene.SendMessage(LevelMessageEnum.GameOver);
                }

                View.Graph = 0;                // Quita el grafico y
                Add(new Explosion1(Location)); // pone una explosion
                Frame(20);
            });

            //// Espero hasta que presione una tecla.
            //While(() => !Input.KeyPressed(), () => Frame());

            //// // Quita todos los sonido y empieza de nuevo
            //// // porque significa que se ha quedado sin vidas
            //// stop_sound(all_sound);
            //// seleccion=0;
            //// // Espera hasta que se pulse el espacio
            //// While(() =>  (NOT key(_space))
            ////     Frame();
            //// }
            //// esta_muerto=1;                  // Vuelve al menu principal            
        }
    }
}
