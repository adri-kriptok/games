using Kriptok.Games.BlastemUp.Player;
using Kriptok.Entities.Base;
using Kriptok.Views.Texts;
using System.Drawing;
using Kriptok.Views.Shapes;

namespace Kriptok.Games.BlastemUp
{
    internal static class Global
    {
        /// <summary>
        /// Velocidad de movimiento actual.
        /// </summary>
        internal static float ScrollCurrentSpeed;

        /// <summary>
        /// Identificador para la nave del jugador.
        /// </summary>
        internal static PlayerShip PlayerShip;

        /// <summary>
        /// Puntuacion del jugador.
        /// </summary>
        internal static int Score;

        /// <summary>
        /// Numero de vidas actuales.
        /// </summary>
        internal static int LifeCount;

        /// <summary>
        /// Identificador para bonus.
        /// </summary>
        internal static EntityBase[] BonusLetters = new EntityBase[5];

        /// <summary>
        /// Identificador de vidas extra.
        /// </summary>
        internal static EntityBase[] Lives = new EntityBase[Consts.MaxLivesOnScreen + 1];

        // -----------------------------------------------------------------------
        // Datos del grupo de enemigos
        // -----------------------------------------------------------------------

        /// <summary>
        /// Numero de grupos 0..109.
        /// </summary>
        public static int LastGroup = 109;

        public static int grupo_pantalla;     // Numero del grupo en pantalla
        public static int[] vivos = new int[4];           // Numero de enemigos vivos en el grupo [i]
        public static int[] no_bonus = new int[4];        // Bandera. 1=Cuando algun enemigo en el grupo [i] no ha muerto

        public static float bonus_x;     // Posicion de los bonus
        public static float bonus_y;     
        
        /// <summary>
        /// Nivel actual.
        /// </summary>
        public static int CurrentLevel = Consts.StartLevel;

        /// <summary>
        /// Numero de grupo actual.
        /// </summary>
        public static int CurrentGroup;

        /// <summary>
        /// Fuente para mostrar el número de nivel.
        /// </summary>
        public static SuperFont LevelFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Bauhaus 93", 24);
            builder.SetColor(Color.Transparent);
            builder.Border = Strokes.Get(Color.Orange, 2);
        });            

        /// <summary>
        /// Fuente utilizada para representar datos del juego en pantalla.
        /// </summary>
        public static SuperFont GameplayFont = SuperFont.Build(builder =>
        {
            builder.Font =new Font("Bauhaus 93", 12);
            builder.SetColor(Color.Green, Color.Yellow);
            builder.Border = Strokes.Get(Color.DarkGreen, 1);
        });            
    }    
}
