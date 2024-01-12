using Kriptok.Div;
using Kriptok.Games.Dgs.Fostiator.Scenes;
using Kriptok.Views.Shapes;
using Kriptok.Views.Texts;
using System.Drawing;

namespace Kriptok.Games.Fostiator
{
    class Global2
    {
        public static DivFileX FileX = DivFileX.Load(typeof(Global2).Assembly, "Assets.Files.Game.fpgx");
        public static DivFileX[] FilesX = new DivFileX[4]
        {
            DivFileX.Load(typeof(Global2).Assembly, "Ripley.fpgx"),
            DivFileX.Load(typeof(Global2).Assembly, "Bishop.fpgx"),
            DivFileX.Load(typeof(Global2).Assembly, "Alien.fpgx"),
            DivFileX.Load(typeof(Global2).Assembly, "Nostromo.fpgx")
        };

        public static readonly SuperFont Font1 = SuperFont.Build(builder =>
        {
            builder.Font = Fonts.Arial10Bold;
            builder.SetColor(Color.FromArgb(255, 225, 192));
            builder.SetShadow(1, 1, Color.Red);
        });
            
        public static readonly SuperFont Font2 = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Algerian", 43, FontStyle.Bold);
            builder.SetColor(Color.Red, Color.Blue);
            builder.SetShadow(2, 2, Color.Orange);
            builder.Border = Strokes.Get(Color.Yellow, 1f);
        });

        // Textos de las opciones de combate
        public static string[] FighterNames = new string[] { "RIPLEY", "BISHOP", "ALIEN", "NOSTROMO" };
        public static string[] ScenarioNames = new string[] { "ESCENARIO 1 : CASTILLO IKA", "ESCENARIO 2 : LA CUEVA", "ESCENARIO 3: DESIERTO" };
        public static string[] niveles = new string[] { "DIFICULTAD : FACIL", "DIFICULTAD : NORMAL", "DIFICULTAD : DIFICIL" };
        public static string[] modos = new string[] { "ORDENADOR contra ORDENADOR", " TECLAS CURSOR contra ORDENADOR",
                "TECLAS UJHK-Q contra ORDENADOR","TECLAS UJHK-Q contra TECLAS CURSOR" };
        
        /// <summary>
        /// Nombres de niveles de Sangre.
        /// </summary>
        public static string[] BlodLevels = new string[] { "NIVEL DE SANGRE : NINGUNA", "NIVEL DE SANGRE : NORMAL", "NIVEL DE SANGRE : EXTASIS" };

        /// <summary>
        /// Nivel de sangre. Ver <see cref="BlodLevels"/>.
        /// </summary>
        public static int BloodLevel = 1;

        public static int BloodCount = 0;

        /// <summary>
        /// Dificultad del juego (0..2)
        /// </summary>
        public static int DifficultyLevel = 1;


        /// <summary>
        /// 0 - parado
        /// 1 - jugando
        /// 2 - fin combate
        /// </summary>
        public static int GameState;


        public static Fighter id_luchador1;
        public static Fighter id_luchador2;
    }
}
