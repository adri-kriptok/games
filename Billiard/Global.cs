using Kriptok.Common;
using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Billiard.Entities;
using Kriptok.Entities.Base;

namespace Billiard
{
    public static class Global
    {
        /// <summary>
        /// Numero de carambolas con la bola blanca.
        /// </summary>
        public static int ScorePlayer1;

        /// <summary>
        /// Numero de carambolas con la bola amarilla.
        /// </summary>
        public static int ScorePlayer2;

        /// <summary>
        /// Bola blanca.
        /// </summary>
        public static Ball WhiteBall;

        /// <summary>
        /// Bola amarilla.
        /// </summary>
        public static Ball YellowBall;

        /// <summary>
        /// Bola roja.
        /// </summary>
        public static Ball RedBall;

        /// <summary>
        /// Controla la ultima colision
        /// </summary>
        public static Ball LastBall;

        /// <summary>
        /// Identificador de la bola que colisiona.
        /// </summary>
        public static Ball CollisionBall;

        /// <summary>
        /// Controla turno de los jugadores.
        /// </summary>
        public static int CurrentTurn;

        /// <summary>
        /// Velocidad total de la bola blanca
        /// </summary>
        public static float WhiteBallTotalSpeed = 0f;

        /// <summary>
        /// Velocidad total de la bola amarilla
        /// </summary>
        public static float YellowBallTotalSpeed = 0f;

        /// <summary>
        /// Identificador de efectos.
        /// </summary>
        public static Effect EffectObject;

        /// <summary>
        /// Índice de la puntuación necesaria para ganar el partido.
        /// </summary>
        public static int CurrentScoreOption = 0;

        /// <summary>
        /// Opciones de puntuación para ganar el partido.
        /// </summary>
        public static int[] ScoreOptions = new int[] { 7, 21, 40 };

        /// <summary>
        /// Obtiene la puntuación seleccionada para ganar.
        /// </summary>
#if DEBUG
        public static int WinningScore => 1;
#else
        public static int WinningScore => ScoreOptions[CurrentScoreOption];
#endif        

        /// <summary>
        /// Obtiene la puntuación seleccionada para ganar.
        /// </summary>
        public static string WinningScoreString => $"{WinningScore} Puntos";

        /// <summary>
        /// Comprueba el choque con la bola contraria.
        /// </summary>
        public static bool CollisionOtherBall = false;

        /// <summary>
        ///  Comprueba el choque con la bola roja.
        /// </summary>
        public static bool CollisionRedBall = false;

        /// <summary>
        /// Identificador de modo.
        /// </summary>
        public static GameModeEnum Mode = GameModeEnum.None;
        
        /// <summary>
        /// Efecto del golpe a la bola.
        /// </summary>
        public static Vector2F EffectModifier;

        /// <summary>
        /// Sonido de banda.
        /// </summary>
        internal static readonly Resource BandaSound = Resource.Get(typeof(Global).Assembly, "Assets.BANDA.WAV");
    }
}
