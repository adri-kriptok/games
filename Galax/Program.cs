using Galax.Scenes;
using Kriptok;
using Kriptok.Core;
using Kriptok.Div.Scenes;
using Kriptok.Entities.Base;
using System;

namespace Galax
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Engine.Start(new DivIntroVideoScene<TitleScene>(false), s =>
            {
                s.Title = "Galax | Kriptok";
                s.FullScreen = false;
                s.Mode = WindowSizeEnum.M320x200;
                s.TimerInterval = 26;
                s.ExtractMidiPlayer();
            });
        }
    }

    public static class Consts
    {
        /// <summary>
        /// Coordenada X de inici del escuadrón.
        /// </summary>
        public const int SquadStartX = 60;

        /// <summary>
        /// Numero de pixel que se mover  el escuadron.
        /// </summary>
        public const int SquadronSpeed = 1;
    }

    public static class Global
    {
        /// <summary>
        /// Puntuacion del jugador.
        /// </summary>
        public static int Score = 0;

        /// <summary>
        /// Maxima puntuacion obtenida.
        /// </summary>
        public static int MaxScore = 0;

        /// <summary>
        /// Nivel actual (1..n)
        /// </summary>
        public static int Level;

        /// <summary>
        /// Vidas restantes.
        /// </summary>
        public static int Lives;
       
        /// <summary>
        /// Esquina superior-izquierda del escuadron.
        /// </summary>
        public static int SquadCurrentX = Consts.SquadStartX;

        /// <summary>
        /// Cambia la direccion de movimiento del escuadron.
        /// </summary>
        public static bool ChangeSquadDirectionFlag;

        /// <summary>
        /// Direccion de movimiento del escuadron 0=izq.,1=der.
        /// </summary>
        public static int MovementDirection;

        /// <summary>
        /// Identificador de la nave.
        /// </summary>
        public static EntityBase Player;

        /// <summary>
        /// Formacion inicial: 0 vac¡o, 1 tipo_nave_1, 2 tipo_nave_2, 3 tipo_nave_3
        /// </summary>
        public static readonly int[] Formation = new int[]
        {
            3, 3, 3, 3, 3, 0, 0, 3, 3, 3, 3, 3,
            0, 2, 2, 2, 0, 2, 2, 0, 2, 2, 2, 0,
            2, 1, 2, 1, 0, 2, 2, 0, 1, 2, 1, 2,
            1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1
        };

        /// <summary>
        /// Formacion en juego.
        /// </summary>
        public static int[] EnemyType = new int[Formation.Length];
    }
}

