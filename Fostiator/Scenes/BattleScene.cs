using Fostiator.Entities;
using Kriptok.Common;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Base;
using Kriptok.Scenes;
using Kriptok.Views.Gdip;
using System.Drawing;
using static Fostiator.Global2;

namespace Fostiator.Scenes
{
    internal class BattleScene : SceneBase
    {
        private readonly int scenario;
        private int wins1;
        private int wins2;
        
        private readonly int fighter1Id;
        private readonly int fighter2Id;

        private readonly int mode = 0;

        public BattleScene(int scenario, int fighter1, int fighter2, int mode)
            : this(scenario, fighter1, fighter2, mode, 0, 0)
        {
        }

        public BattleScene(int scenario, int fighter1, int fighter2, int mode, int wins1, int wins2)
        {
            this.scenario = scenario;

            // Inicializa las variables del juego
            this.wins1 = wins1;
            this.wins2 = wins2;
            Global2.BloodCount = 0;
            this.fighter1Id = fighter1;
            this.fighter2Id = fighter2;
            this.mode = mode;
        }

        protected override void Run(SceneHandler h)
        {            
            var scroll = h.StartScroll(new BattleBackgroundScroll(h.ScreenRegion.Rectangle, scenario));

            h.Add(new GenericObject(320, 49, 200, 0)); // Pone los marcadores de energia

            // Crea los muniecos  que se han seleccionado
            // y el tipo de control de los mismos
            switch (mode)
            {
                // Ordenador contra ordenador
                case 0:
                    Fighter1 = h.Add(scroll, new Fighter(260, 440, Global2.FilesX[fighter1Id], FlipEnum.FlipX, FighterControlEnum.Computer, fighter1Id));
                    Fighter2 = h.Add(scroll, new Fighter(700, 440, Global2.FilesX[fighter2Id], FlipEnum.None, FighterControlEnum.Computer, fighter2Id, fighter1Id == fighter2Id));
                    break;

                // Jugador contra ordenador
                case 1:
                    Fighter1 = h.Add(scroll, new Fighter(260, 440, Global2.FilesX[fighter1Id], FlipEnum.FlipX, FighterControlEnum.Keyboard1, fighter1Id));
                    Fighter2 = h.Add(scroll, new Fighter(700, 440, Global2.FilesX[fighter2Id], FlipEnum.None, FighterControlEnum.Computer, fighter2Id, fighter1Id == fighter2Id));
                    break;

                // Ordenador contra jugador
                case 2:
                    Fighter1 = h.Add(scroll, new Fighter(260, 440, Global2.FilesX[fighter1Id], FlipEnum.FlipX, FighterControlEnum.Keyboard2, fighter1Id));
                    Fighter2 = h.Add(scroll, new Fighter(700, 440, Global2.FilesX[fighter2Id], FlipEnum.None, FighterControlEnum.Computer, fighter2Id, fighter1Id == fighter2Id));
                    break;

                // Jugador contra jugador
                case 3:
                    Fighter1 = h.Add(scroll, new Fighter(260, 440, Global2.FilesX[fighter1Id], FlipEnum.FlipX, FighterControlEnum.Keyboard2, fighter1Id));
                    Fighter2 = h.Add(scroll, new Fighter(700, 440, Global2.FilesX[fighter2Id], FlipEnum.None, FighterControlEnum.Keyboard1, fighter2Id, fighter1Id == fighter2Id));
                    break;
            }

            scroll.SetTarget(new Target(Fighter1, Fighter2));

            // Inicializa las variables enemigo de los muniecos que son
            // identificadores al proceso del enemigo
            Fighter1.Enemy = Fighter2;
            Fighter2.Enemy = Fighter1;

            // Escribe el nombre del escenario
            h.Write(Font1, 320, 0, ScenarioNames[scenario]).CenterTop();

            // Pone las fotos y los nombres de los muniecos
            h.Add(new GenericObject(Global2.FilesX[fighter1Id], 44, 52, 100 + wins2, FlipEnum.FlipX));

            h.Write(Font1, 45, 100, FighterNames[fighter1Id]).CenterTop();

            h.Add(new GenericObject(Global2.FilesX[fighter2Id], 596, 52, 100 + wins1, FlipEnum.None, fighter1Id == fighter2Id));

            h.Write(Font1, 596, 100, FighterNames[fighter2Id]).CenterTop();

            // Creo las barras de energía.
            h.Add(new HealthBar(Fighter1, 193, 37, 1, e => new Rectangle(296 - e, 28 - 1, e + 1, 20 + 2)));
            h.Add(new HealthBar(Fighter2, 445, 37, 2, e => new Rectangle(345, 28 - 1, e + 1, 20 + 2)));

            // Crea las estrellas que marcan los combates ganados
            Stars(h, wins1, wins2);

            var round = GetRoundMessage(h);


            // Muestro la pantalla.
            h.FadeFrom(Color.White);

            h.WaitFrames(48);

            // Borra el texto de inicio de combate
            round.Die();

            // Quita la pausa al juego
            Global2.GameState = 1;

            h.WaitWhile(() => Global2.GameState == 1);            

            if (((Fighter)Fighter1).Health == 0) // Gana el jugador 2
            {
                wins2++;
            }
            else
            {
                wins1++;            // Gana el jugador 1
            }

            // Actualiza las estrellas            
            Stars(h, wins1, wins2);

            h.WaitFrames(48);


            if (wins1 < 2 && wins2 < 2)
            {
                h.FadeTo(Color.White);
                h.Set(new BattleScene(scenario, fighter1Id, fighter2Id, mode, wins1, wins2));
            }
            else
            {
                h.FadeOff();
                h.Set(new OptionsScene());
            }
        }

        
        private void Stars(SceneHandler h, int wins1, int wins2)
        {
            // Elimina cualquier estrella que hubiera antes            
            foreach (var p in h.FindAll<Star>())
            {
                p.Die();
            }

            // Pon las estrellas del jugador 1
            switch (wins1)
            {
                // Dependiendo de los combates ganados pone  unas estrellas u otras
                case 0:
                    h.Add(new Star(111, 84, 203));
                    h.Add(new Star(143, 84, 203));
                    break;
                case 1:
                    h.Add(new Star(111, 84, 204));
                    h.Add(new Star(143, 84, 203));
                    break;
                case 2:
                    h.Add(new Star(111, 84, 204));
                    h.Add(new Star(143, 84, 204));
                    break;
            }

            // Pon las estrellas del jugador 2
            switch (wins2)
            {
                case 0:
                    h.Add(new Star(530, 84, 203));
                    h.Add(new Star(498, 84, 203));
                    break;
                case 1:
                    h.Add(new Star(530, 84, 204));
                    h.Add(new Star(498, 84, 203));
                    break;
                case 2:
                    h.Add(new Star(530, 84, 204));
                    h.Add(new Star(498, 84, 204));
                    break;
            }
        }        

        private ITextEntity GetRoundMessage(SceneHandler h)
        {
            // Pone el mensaje de inicio de combate
            switch (wins1 + wins2)
            {
                case 0:
                    return h.Write(Font2, 320, 200, "ASALTO 1").CenterMiddle();
                case 1:
                    return h.Write(Font2, 320, 200, "ASALTO 2").CenterMiddle();
                case 2:
                    return h.Write(Font2, 320, 200, "ASALTO 3").CenterMiddle();
            }

            return null;
        }

        internal class Target : IScrollTarget
        {
            private readonly Fighter fighter1;
            private readonly Fighter fighter2;

            public Target(Fighter fighter1, Fighter fighter2)
            {
                this.fighter1 = fighter1;
                this.fighter2 = fighter2;
            }

            public Vector2F GetLocation2D()
            {
                return Vector2F.Average(fighter1.GetLocation2D(), fighter2.GetLocation2D());
            }
        }

        internal class BattleBackgroundScroll : ScrollRegion
        {
            public BattleBackgroundScroll(Rectangle region, int scenario) 
                : this(region, scenario, 
                      new GdipImageScrollLayer(Resource.Get(typeof(BattleScene).Assembly, $"Assets.Backgrounds.back{scenario}.png"), false, false))
            {
            }

            private BattleBackgroundScroll(Rectangle region, int scenario, GdipImageScrollLayer layer0) 
                : base(region, layer0)
            {
                layer0.MakeTransparent(Color.Black);

                var background = Resource.Get(typeof(BattleScene).Assembly, $"Assets.Backgrounds.landscape{scenario % 2}.png");                
                var layer1 = AddLayer(new GdipImageScrollLayer(background, false, false));

                layer1.Priority -= 1;
            }
        }
    }
}