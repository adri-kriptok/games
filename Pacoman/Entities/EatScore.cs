using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacoman.Entities
{
    /// <summary>
    /// Mensaje con puntaje que aparece en pantalla.
    /// </summary>
    public class EatScore : EntityBase<IndexedSpriteView>
    {
        public const int Frames = 15;
        private int counter;

        public EatScore(Vector3F loc, int scoreType, int index) 
            : base(new IndexedSpriteView(typeof(EatScore).Assembly, $"Assets.Images.Scores{scoreType}.png", 2 + scoreType, 2))
        {
            Location = loc;
            View.Graph = index;

            counter = Frames;
        }

        protected override void OnFrame()
        {
            if (counter <= 0)
            {                
                Die();
            }
            counter--;            
        }
    }
}
