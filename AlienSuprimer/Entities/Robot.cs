using Kriptok.Audio;
using Kriptok.Common.Base;
using Kriptok.Div;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kriptok.Games.Alien.Entities
{
    internal class Robot : ProcessBase<IndexedSpriteView>
    {
        /// <summary>
        /// Cambia los graficos segun disparo.
        /// </summary>
        private int grafico_disparo = 0;          

        private bool puedo_disparar = true;           // Bandera. 1=disparo avaible
        private bool puedo_disparar_misiles = true;   // Bandera. 1=misiles avaible

        /// <summary>
        /// Identificador de sonido.
        /// </summary>        
        private ISoundHandler shootSound;

        public Robot() : base(new IndexedSpriteView(typeof(Robot).Assembly, "Assets.Images.Robot.png", 1, 2))
        {
            Location.Z = -5f;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;

            shootSound = h.Audio.GetWaveHandler(Assembly, "Assets.Sounds.LASER6.WAV");
        }

        protected override void OnBegin()
        {
            Loop(() =>
            {
                // Controla los misiles
                if (Input.Button03Pressed() && Global.contador_misiles > 0)
                {
                    if (puedo_disparar_misiles)
                    {
                        grafico_disparo = 1;
                        puedo_disparar_misiles = false;
                        //disparo_misiles();
                        Add(new PlayerMissile(Location));
                        Global.contador_misiles--;
                    }
                }
                else
                {
                    puedo_disparar_misiles = true;
                }

                // Control de disparos
                if (Input.Key(Keys.RControlKey))
                {
                    if (puedo_disparar)
                    {
                        // // Para cualqueir sonido de disparo que hubiera anteriormente
                        // stop_sound(id_sonido_disparo);

                        // Pone las variables para no tener disparo continuo
                        grafico_disparo = 1;
                        puedo_disparar = false;

                        // Crea los dos disparos
                        Add(new RobotShot(Location.X - 18f, Location.Y - 4f));
                        Add(new RobotShot(Location.X + 18f, Location.Y - 4f));

                        // Y hace el sonido del mismo
                        shootSound.Play();
                    }
                }
                else
                {
                    // Si no esta disparando, pone la variable que permite disparar
                    puedo_disparar = true;
                }

                // Pone el grafico dependiendo si se dispara o no
                View.Graph = grafico_disparo;
                grafico_disparo = 0;
                Frame();
            });
        }
    }
}
