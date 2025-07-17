using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Regions.Scroll;
using Kriptok.Views.Base;
using System;
using System.Diagnostics;

namespace Kriptok.Adventure.Entities.Monsters
{
    public abstract class MapEntityBase<TView> : EntityBase<TView>, IScrollCircleEntity
        where TView : IView
    {
        private readonly CircleCollisionable circleCollisionable;

        protected MapEntityBase(TView view) : base(view)
        {
            this.circleCollisionable = new CircleCollisionable(this);
        }

        /// <inheritdoc/>
        public abstract float GetWeight();

        internal void CheckCollisions() => circleCollisionable.CheckCollisions();

        internal void ResolvePushes() => circleCollisionable.ResolvePushes();

        /// <inheritdoc/>
        public IScrollCircleEntity FindClosestCollisionable() => CircleCollisionable.FindClosest(this);

        [DebuggerStepThrough]
        public void AddPush(IScrollCircleEntity other) => circleCollisionable.AddPush(other);

        [DebuggerStepThrough]
        public virtual void RejectFrom(IScrollCircleEntity other) => circleCollisionable.RejectFrom(other);

        [DebuggerStepThrough]
        public virtual void RejectFrom(IScrollCircleEntity other, float distance) => circleCollisionable.RejectFrom(other, distance);

        /// <inheritdoc/>
        public void Push(Vector2F vector)
        {
            Location = Location.Plus(vector);
        }

        public void UpdateData(TileScrollData tileScrollData)
        {
            Location.X = tileScrollData.Location.X;
            Location.Y = tileScrollData.Location.Y;
        }        
    }
}
