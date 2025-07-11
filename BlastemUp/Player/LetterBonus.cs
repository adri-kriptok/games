using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;

namespace Kriptok.Games.BlastemUp.Player
{
    public class LetterBonus : EntityBase<IndexedSpriteView>
    {
        /// <summary>
        /// Datos de pantalla.
        /// </summary>
        private static int[] xLocations = new int[5] { 495, 526, 557, 588, 619 };

        public LetterBonus(int posicion) : base(new BonusView())
        {
            // Si la letra ya esta puesta entonces retorna
            if (Global.BonusLetters[posicion] != null)
            {
                Die();
                return;
            }

            // Guarda el identificador para luego poder eliminarlo
            Global.BonusLetters[posicion] = this;           
            
            // Selecciona coordenadas
            Location.Y = 21;
            Location.X = xLocations[posicion];

            // y grafico            
            View.Graph = 16 + posicion* 8;
        }

        protected override void OnFrame()
        {
        }
    }

    internal class BonusView : IndexedSpriteView
    {
        public BonusView() : base(typeof(LetterBonus).Assembly, "Assets.Images.Bonus.png", 8, 8)
        {
        }
    }
}
