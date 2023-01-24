using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Regions.Scroll;
using Kriptok.Tehuelche.Entities.Hud;
using Kriptok.Tehuelche.Regions;
using Kriptok.Tehuelche.Scenes.Base;
using Kriptok.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Tehuelche.Entities.Enemies
{
    internal abstract class EnemyBase : EntityBase
    {
        private readonly BoundF2 playArea;
        private float energy;

        protected EnemyBase(LevelBuilder builder, IView view, float energy) : base(view)
        {
            this.playArea = builder.Terrain.PlayArea;
            this.energy = energy;
        }

        /// <summary>
        /// Daña al enemigo.
        /// </summary>        
        internal void Damage(float damage)
        {
            energy -= damage;
        }

        protected override void OnFrame()
        {
            Location.X = Location.X.Clamp(playArea.MinX, playArea.MaxX);
            Location.Y = Location.Y.Clamp(playArea.MinY, playArea.MaxY);

            if (energy < 0f)
            {
                OnDying();
                Die();
            }
        }

        internal virtual void OnDying()
        {            
        }

        internal abstract Vector3F GetAimAngle();
    }

    internal abstract class EnemyBase<T> : EnemyBase
        where T : IView
    {
        private readonly TehuelcheMapRegion terrain;

        protected EnemyBase(LevelBuilder builder, T view, float energy) 
            : base(builder, view, energy)
        {
            terrain = builder.Terrain;
            builder.Add(new MinimapEnemy(this));
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            Location.Z = terrain.SampleHeight(Location.XY());
        }

        public new T View => (T)base.View;
    }
}
