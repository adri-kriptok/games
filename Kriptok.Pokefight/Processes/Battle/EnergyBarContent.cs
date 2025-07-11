using Kriptok.Drawing;
using Kriptok.Pokefight.Processes.Base;
using Kriptok.Entities.Base;
using Kriptok.Views.Primitives;
using System.Drawing;

namespace Kriptok.Pokefight.Processes.Battle
{
    class EnergyBarContent : ProcessBase<RectangleView>
    {
        public EnergyBarContent(PokemonBase pokemon, int x, int regionWidth)
            : base(new RectangleView(104, 10, Color.FromArgb(0, 255, 0))
            {
                Center = new PointF(x < (regionWidth / 2) ? 0f : 1f, 0.5f)
            })
        {
            Location.X = x;
            Location.Y = 11;
            Location.Z = -100;

            this.pokemon = pokemon;
        }

        public PokemonBase pokemon;

        protected override void OnBegin()
        {
            Loop(() =>
            {
                if (View.Width != pokemon.LifePoints)
                {
                    View.Width = (int)(pokemon.LifePoints * 1.04);

                    var r = 255 - (pokemon.LifePoints * 255) / 100;
                    var g = (pokemon.LifePoints * 255) / 100;

                    if (r > 255) r = 255;
                    //if (g > 255) g = 255;
                    //if (r < 0) r = 0;
                    if (g < 0) g = 0;

                    View.FillColor = Color.FromArgb(r, g, 0);
                }

                Frame();
            });
        }
    }
}
