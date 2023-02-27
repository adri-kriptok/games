using Kriptok.Div;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.IO;
using Kriptok.Views.Shapes;
using Kriptok.Views.Texts;
using System.Drawing;

namespace Exploss
{
    public class ExplossConfiguration : BaseConfiguration
    {
        /// <summary>
        /// Dificultad del juego.
        /// 
        /// Por defecto es el nivel medio.
        /// </summary>
        public int DifficultyLevel { get; set; } = 2;
    }

    static class Global
    {
        public static SuperFont Font0 = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Arial", 30, FontStyle.Italic | FontStyle.Bold);
            builder.SetColor(Color.Orange, Color.OrangeRed);
            builder.SetShadow(1, 1, Color.DarkRed);
        });

        public static SuperFont Font1 = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Broadway", 40, FontStyle.Bold);
            builder.SetColor(Color.Yellow);
            builder.SetShadow(2, 2, Color.Red);
        });

        public static SuperFont Font2 = SuperFont.Build(builder =>
        {
            builder.Font = new Font("OCR A", 15, FontStyle.Bold);
            builder.SetColor(Color.White);
            builder.SetShadow(2, 2, Color.Gray);
        });            

        public static SuperFont Font3 = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Arial", 20, FontStyle.Bold);
            builder.SetColor(Color.White);
            builder.Border = Strokes.Get(Color.CornflowerBlue, 2f);
        });

        /// <summary>
        /// Porcentaje de terminado el nivel.
        /// </summary>
        public static int BlocksBroken = 0;

        /// <summary>
        /// Porcentaje de terminado el nivel.
        /// </summary>
        public static int BlocksPercentage => (BlocksBroken * 100f / 91f).Floor();

        /// <summary>
        /// Puntuación.
        /// </summary>
        public static int Score;

        /// <summary>
        /// Record de puntos.
        /// </summary>
        public static int ScoreRecord = 0;

        /// <summary>
        /// Próxima puntuación a la que se recibe una vida.
        /// </summary>
        public static int NextLiveScore = Consts.ScoreLivesInterval;

        /// <summary>
        /// Numero de vidas.
        /// </summary>
        public static int CurrentLives;

        /// <summary>
        /// Numero de bombas.
        /// </summary>
        public static int CurrentBombs;
    }
}
