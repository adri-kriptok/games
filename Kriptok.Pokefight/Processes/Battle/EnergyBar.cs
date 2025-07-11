using Kriptok.Extensions;
using Kriptok.Pokefight.Processes.Base;
using Kriptok.Entities.Base;
using Kriptok.Views;
using Kriptok.Views.Gdip;
using Kriptok.Views.Sprites;

namespace Kriptok.Pokefight.Processes.Battle
{
    class EnergyBar : ProcessBase<SpriteView>
    {
        private readonly PokemonBase pokemon;

        public EnergyBar(PokemonBase pokemon, int x, FlipEnum flip)
            : base(new SpriteView(typeof(EnergyBar).Assembly, "Resources.Images.HealContainer.png"))
        {
            this.pokemon = pokemon;
            Location.X = x;
            Location.Y = 12;
            Location.Z = -1000;
            View.Flip = flip;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);

            var mod = 52 * (Location.X < (h.RegionSize.Width / 2) ? -1 : 1);

            Add(new EnergyBarContent(pokemon, Location.X.Round() + mod, h.RegionSize.Width));
        }

        protected override void OnBegin()
        {
            Loop(() =>
            {
                Frame();
            });
        }
    }
}
