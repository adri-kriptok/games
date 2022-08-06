using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Noid.Scenes;
using System;

namespace Noid.Entities.Pills
{
    internal class LifePill : PillBase
    {
        internal LifePill(Vector3F location) : base(location, "P_Life.png")
        {
        }

        protected override void OnPick()
        {
            Global.LifeCount += 1;
           
            // Genero vidas.
            CreateLife();
        }

        private void CreateLife()
        {
            var count = Math.Min(Consts.MaxLivesOnScreen, Global.LifeCount);

            for (int i = 0; i < count; i++)
            {
                if (Global.Lives[i] == null || !Global.Lives[i].IsAlive())
                {
                    Global.Lives[i] = Add(new Life(i));
                    return;
                }
            }
        }
    }
}
