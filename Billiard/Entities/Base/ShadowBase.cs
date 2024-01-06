using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billiard.Entities.Base
{
    /// <summary>
    ///  Proceso sombra
    ///  Muestra las sombras de la bola y el palo
    /// </summary>
    public abstract class ShadowBase<T> : EntityBase<T> where T : IView
    {
        private readonly EntityBase owner;

        public ShadowBase(EntityBase owner, T view) : base(view)
        {
            this.owner = owner;
        }

        protected override void OnFrame()
        {            
            Angle.Z = owner.Angle.Z;

            if (!owner.IsAlive())
            {
                Die();
            }
        }

        public override Vector3F GetRenderLocation()
        {
            return new Vector3F(owner.Location.X + 8, owner.Location.Y + 8, owner.Location.Z + 1);
        }       
    }
}
