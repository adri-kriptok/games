using Kriptok.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.BlastemUp.Player
{
    public class LetterFactory : EntityBase
    {
        private int? addValue = null;

        protected override void OnFrame()
        {
            if (addValue.HasValue)
            {
                Add(new LetterBonus(addValue.Value));
                addValue = null;
            }
        }

        internal void Add(int value)
        {
            addValue = value;
        }
    }
}
