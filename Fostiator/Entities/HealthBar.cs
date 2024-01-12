using Kriptok.Entities.Base;
using Fostiator.Scenes;
using Kriptok.Views;
using Kriptok.Views.Sprites;
using System;
using System.Drawing;

namespace Fostiator.Entities
{
    /// <summary>    
    /// Barra de energía del jugador.
    /// </summary>
    public class HealthBar : EntityBase<HealthBar.HealthBarView>
    {
        private readonly Fighter owner;
        private readonly Func<int, Rectangle> update;
        private int energy = 0;

        public HealthBar(Fighter owner, int x, int y, int graph, Func<int, Rectangle> update) : base(new HealthBarView(graph))
        {
            this.owner = owner;
            this.update = update;
            Location.X = x;
            Location.Y = y;
            Location.Z = 10;   // Lo pone por debajo de otros graficos
        }

        protected override void OnFrame()
        {
            if (owner != null)
            {
                if (!owner.IsAlive())
                {
                    Die();
                }

                var newEnergy = owner.Health;
                if (newEnergy != energy)
                {
                    energy = newEnergy;

                    if (View != null)
                    {
                        View.SetClip(update(energy));
                    }
                }
            }
        }

        internal void SetClip(int x, int y, int w, int h)
        {
            if (View != null)
            {
                View.SetClip(new Rectangle(x, y, w, h));
            }
        }

        public class HealthBarView : ClippedViewBase<SpriteView>
        {
            private Rectangle rectangle;

            public HealthBarView(int graph) : base(new SpriteView(typeof(HealthBarView).Assembly, $"Images.HealthBar{graph}.png"))
            {
                rectangle = new Rectangle(0, 0, 640, 480);
            }

            protected override void Clip(Graphics g) => g.SetClip(rectangle);

            internal void SetClip(Rectangle rectangle)
            {
                this.rectangle = rectangle;
            }
        }
    }
}
