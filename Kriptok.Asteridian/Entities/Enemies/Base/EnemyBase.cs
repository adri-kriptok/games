using Kriptok.Entities.Base;
using Kriptok.Entities.Queries.Base;
using Kriptok.Extensions;
using Kriptok.Views.Base;
using Kriptok.Views.Gdip.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Entities.Enemies.Base
{
    internal abstract class EnemyBase : EntityBase
    {
        private IQuery<bool?> outOfScreenQuery;
        private bool isOnScreen = false;

        protected EnemyBase()
        {
            Location.Z = GlobalConsts.ZLevel.EnemyInAir;
        }

        public EnemyBase(IView view) : base(view)
        {
            Location.Z = GlobalConsts.ZLevel.EnemyInAir;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.outOfScreenQuery = h.GetOutOfScreenQuery();
        }

        protected override void OnFrame()
        {
            if (outOfScreenQuery.Result.HasValue)
            {
                if (isOnScreen)
                {
                    if (outOfScreenQuery.Result.Value)
                    {
                        Die();
                        return;
                    }
                }
                isOnScreen = !outOfScreenQuery.Result.Value;
            }
        }

        internal virtual void StartOnTop(float y)
        {            
            if (View is GdipRenderizableBase rect)
            {
                var height = rect.GetSize().Height;
  
                Location.Y = y + height * rect.Center.Y - height;
            }            
        }
    }
}
