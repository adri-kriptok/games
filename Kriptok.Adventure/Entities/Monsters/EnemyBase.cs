using Kriptok.Adventure.Entities.Base;
using Kriptok.Adventure.Extensions;
using Kriptok.Adventure.Scenes.Base;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Partitioned;
using Kriptok.Entities.Queries.Base;
using Kriptok.Mapping.Entities;
using Kriptok.Regions;
using Kriptok.Regions.Scroll;
using Kriptok.Views.Base;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Kriptok.Adventure.Entities.Monsters
{
    public abstract class EnemyBase<TView> : MapEntityBase<TView>, ISlashable
        where TView : IView
    {
        private readonly ILocationValidator validator;
        private IQuery<bool?> outOfScreenQuery;
        private Vector2F framePushes = Vector2F.Empty;

        protected EnemyBase(MapEntityCreationArgs h, TView view) : base(view)
        {
            this.validator = h.GetLocationValidator(this);

            var location = h.GetMapLocation();
            Location.X = location.X;
            Location.Y = location.Y;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            this.outOfScreenQuery = h.GetOutOfScreenQuery();
        }

        protected override sealed void OnFrame()
        {
            var onScreen = !outOfScreenQuery.Result.GetValueOrDefault(true);

            validator.ValidatingLocation(() =>
            {
                if (onScreen)
                {
                    OnValidatingFrame();
                }
                
                // Resuelvo los empujones del frame.
                Location = Location.Plus(framePushes = framePushes.Scale(0.5f));

                // Y ahora lo acomodo a las otras entidades.
                base.ResolvePushes();
            });
            base.CheckCollisions();
        }

        protected abstract void OnValidatingFrame();

        public void Slash(Vector2F push)
        {
            framePushes = framePushes.Plus(push);
        }
    }
}
