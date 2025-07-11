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
    class Pidgey : PokemonBase
    {
        protected override int AttackMovement
        {
            get
            {
                return 11;
            }
        }

        public Pidgey(ControlConfig controls) : base(controls, "Resources.Images.Pokemons.Pidgey1.png", 
            "Resources.Images.Pokemons.Pidgey2.png", 12, 14, 2, 0, 3, 16, 11)
        {           
        }

        protected override void WalkLeft()
        {
            PokemonY = 20;
            AdjustPokemonY();
            base.WalkLeft();            
        }

        protected override void WalkRight()
        {
            PokemonY = 20;
            AdjustPokemonY();
            base.WalkRight();            
        }

        protected override void OnBeforeFrame()
        {
            base.OnBeforeFrame();
            PokemonY = 0;
        }

        internal override int[] GetWalingIndexes() => new int[] { 0, 1, 2, 1 };

        protected override float GetJumpInitialSpeed() => 45;
    }
}
