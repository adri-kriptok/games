using Kriptok.Pokefight.Common;
using Kriptok.Pokefight.Processes.Base;
using Kriptok.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Pokefight.Processes.Pokemons
{
    class Pikachu : PokemonBase
    {
        public Pikachu(ControlConfig controls) : base(controls, "Resources.Images.Pokemons.Pikachu1.png", 
            "Resources.Images.Pokemons.Pikachu2.png", 12, 14, 9, 6, 5, 11, 7)
        {
        }

        protected override int AttackMovement => 8;

        protected override float GetJumpInitialSpeed() => 40;

        internal override int[] GetWalingIndexes() => new int[] { 4, 2, 3 };
    }
}
