using Kriptok.Entities.Base;
using System;
using Kriptok.Entities;
using Kriptok.Views.Sprites;

namespace Pacoman.Entities
{
    public class Title : EntityBase<SpriteView>
    {
        /// <summary>
        /// Contador para mover el título.
        /// </summary>
        private int counter = 0;    

        private readonly ITextEntity txt;

        /// <summary>
        /// Ángulo para la coordenada vertical del título.
        /// </summary>
        private float ang = 0f;

        public Title(ITextEntity txt) : base(new SpriteView(typeof(Title).Assembly, "Title.png"))
        {
            this.txt = txt;
            
            // Pone coordenada horizontal
            Location.X = 320;          
            Location.Z = -10;
        }
        
        protected override void OnFrame()
        {            
            counter = (counter + 1) % 14;

            // Impr¡melo dentro de pantalla
            if (counter == 0)
            {
                txt.LocationY = 320;
            }

            // Impr¡melo fuera de pantalla lo que hace al texto intermitente
            if (counter == 7)
            {
                txt.LocationY = 640;
            }

            // Mueve el titulo
            Location.Y = 50f + (float)Math.Sin(ang += 0.5f) * 3f;            
        }
    }
}
