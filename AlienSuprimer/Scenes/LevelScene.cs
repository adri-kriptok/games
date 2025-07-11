using Kriptok.Common;
using Kriptok.Drawing;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Games.Alien.Entities;
using Kriptok.Games.Alien.Entities.Enemies;
using Kriptok.Games.Alien.Regions;
using Kriptok.Regions.Context.Base;
using Kriptok.Scenes;
using Kriptok.Texts;
using Kriptok.Views.Base;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace Kriptok.Games.Alien.Scenes
{
    internal class LevelScene : SceneBase
    {
        private static readonly SuperFont GameFont = SuperFont.Load(typeof(IntroScene).Assembly, "Assets.Fonts.Game.fntx");
        private static readonly SuperFont GameOverFont = SuperFont.Load(typeof(IntroScene).Assembly, "Assets.Fonts.GameOver.fntx");

        protected override void Run(SceneHandler h)
        {
            // Inicializa el movimiento de pantalla (scroll) y las variables de coordenadas del mismo
            var scroll = h.StartScroll(new ScrollX2Region());

            // Reinicia las variables necesarias
            Global.enemigo_f = 1;
            Global.contador_misiles = 6;
            Global.misiles_libres = 1;
            Global.TankShootFrequency = 18;
            Global.HelicopterShootFrequency = 40;
            Global.puntuacion = 0;

            Global.Lives = Global.Consts.InitialLives;

            h.Write(GameFont, h.ScreenRegion.Size.Width, 0, () => Global.puntuacion.ToString()).RightTop();
            h.Write(GameFont, 60, 0, () => Global.Lives.Clamp(0, 9).ToString()).LeftTop();

            // Crea el grafico de las vidas                    
            h.Add(new Life());
            h.Add(new Missiles());

            // Muestra la barra de energ¡a                                                      
            h.Add(new EnergyBar());

            var hardnessMap = new FastBitmap8(Assembly, "Assets.Images.MapHardnesses.png");
            Global.MainRobot = h.Add(scroll, new RobotLegs(scroll.Cam, hardnessMap));

            h.Add(scroll, new EnemyLauncher(hardnessMap, scroll.Cam));

            h.FadeOn();
        }

        protected override void OnMessage(SceneHandler h, object message)
        {
            base.OnMessage(h, message);

            if (message is LevelMessageEnum msg)
            {
                switch (msg) 
                {
                    case LevelMessageEnum.GameOver:
                        h.Write(GameOverFont, h.ScreenRegion.Size.Width / 2, h.ScreenRegion.Size.Height / 2, "FIN DEL JUEGO");

                        h.WaitForKeyPress();

                        h.FadeTo(Color.White);
                        h.StopMidiChannels();
                        h.Set(new MainMenuScene());

                        break;
                } 
            }
        }
    }

    internal class EnemyLauncher : ProcessBase
    {
        private readonly FastBitmap8 map;
        private readonly AlienScrollTarget cam;
        private const float x0 = -110;
        private const float x1 = -60f;
        private const float x2 = 60f;
        private const float x3 = 110f;

        public EnemyLauncher(FastBitmap8 map, AlienScrollTarget cam)
        {
            this.map = map;
            this.cam = cam;
        }

        protected override void OnBegin()
        {
            // Va creando los distintos enemigos

            // Primero crea los proceso estaticos
            Add(new Bunker(0, 54, 1506));
            Add(new Bunker(0, 32, 1184));
            Add(new Bunker(0, 122, 463));
            Add(new Bunker(1, 329, 2248));
            Add(new Bunker(1, 306, 1942));
            Add(new Bunker(1, 306, 1234));
            
            Add(new TankCannon(map, 50, 2600, 0));     // (x,y, direccion)
            Add(new TankCannon(map, 359, 2539, -1));   // direccion:
            Add(new TankCannon(map, 359, 2484, -1));    //  0  >>> estatico
            Add(new TankCannon(map, 100, 2146, 0));    //  1  >>> hacia la derecha
            Add(new TankCannon(map, 322, 2146, 0));    // -1  >>> hacia la izquierda
            Add(new TankCannon(map, 75, 2080, 1));
            Add(new TankCannon(map, 225, 1767, 0));
            Add(new TankCannon(map, 129, 1767, 0));
            Add(new TankCannon(map, 140, 1629, 1));
            Add(new TankCannon(map, 319, 1375, -1));
            Add(new TankCannon(map, 169, 1014, 0));
            Add(new TankCannon(map, 273, 1014, 0));
            Add(new TankCannon(map, 65, 1014, 0));
            Add(new TankCannon(map, 253, 871, -1));
            Add(new TankCannon(map, 137, 600, -1));
            Add(new TankCannon(map, 215, 600, 1));
            Add(new TankCannon(map, 286, 359, 0));
            Add(new TankCannon(map, 225, 302, 0));
            Add(new TankCannon(map, 142, 258, 0));

            // Agrego el controlador de sonidos de los tanques.
            Add(new TankSoundManager());
            Add(new HelicopterSoundManager());

            Add(new FinalEnemy());

            // Espera a la una posicion concreta del mapa
            While(() => (cam.Y > 2410), () =>
            {
                Frame();
            });
            // Y va creando nuevos enemigos
            Add(new Helicopter3(x0)); Add(new Helicopter3(x3));
            While(() => (cam.Y > 2210), () =>
            {
                Frame();
            });
            Add(new Helicopter3(x0)); Add(new Helicopter3(x3));
            While(() => (cam.Y > 2110), () =>
            {
                Frame();
            });
            Add(new Helicopter3(x0)); Add(new Helicopter3(x3));
            While(() => (cam.Y > 1990), () =>
            {
                Frame();
            });
            Add(new Helicopter2(x0)); Add(new Helicopter2(x3));
            While(() => (cam.Y > 1850), () =>
            {
                Frame();
            });
            Add(new Helicopter1(x0)); Add(new Helicopter1(x3));
            While(() => (cam.Y > 1700), () =>
            {
                Frame();
            });
            Add(new Helicopter1(x0)); Add(new Helicopter1(x3));
            Global.TankShootFrequency = 16;
            Global.HelicopterShootFrequency = 30;
            While(() => (cam.Y > 1390), () =>
            {
                Frame();
            });
            Add(new Helicopter2(x0)); Add(new Helicopter2(x3));
            Add(new Helicopter3(x1)); Add(new Helicopter3(x2));
            While(() => (cam.Y > 1280), () =>
            {
                Frame();
            });
            Add(new Helicopter3(x0)); Add(new Helicopter3(x3));
            While(() => (cam.Y > 1110), () =>
            {
                Frame();
            });
            Add(new Helicopter1(x0)); Add(new Helicopter1(x3));
            While(() => (cam.Y > 930), () =>
            {
                Frame();
            });
            Add(new Helicopter1(x0)); Add(new Helicopter1(x3));
            Add(new Helicopter3(x1)); Add(new Helicopter3(x2));
            While(() => (cam.Y > 770), () =>
            {
                Frame();
            });
            Add(new Helicopter3(x0)); Add(new Helicopter3(x3));
            While(() => (cam.Y > 670), () =>
            {
                Frame();
            });
            Add(new Helicopter1(x0)); Add(new Helicopter1(x3));
            Global.TankShootFrequency = 14;    // Incrementa la dificultad
            Global.HelicopterShootFrequency = 25;
            While(() => (cam.Y > 620), () =>
            {
                Frame();
            });
            Add(new Helicopter1(x0)); Add(new Helicopter1(x3));
            While(() => (cam.Y > 490), () =>
            {
                Frame();
            });
            Add(new Helicopter3(x0)); Add(new Helicopter3(x3));
            While(() => (cam.Y > 350), () =>
            {
                Frame();
            });
            Add(new Helicopter1(x0)); Add(new Helicopter1(x3));
            Add(new Helicopter3(x1)); Add(new Helicopter3(x2));
            While(() => (cam.Y > 300), () =>
            {
                Frame();
            });
            Add(new Helicopter3(x0)); Add(new Helicopter3(x3));
            While(() => (cam.Y > 260), () =>
            {
                Frame();
            });
            Add(new Helicopter3(x0)); Add(new Helicopter3(x3));
        }
    }
}