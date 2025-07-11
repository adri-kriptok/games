using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using System.Drawing;
using Tokenkai.Entities;

namespace Tokenkai.Scenes
{
    internal class MenuScene : SceneBase
    {
        private static readonly int[] x = new int[] { 319, 318, 317, 317, 319 };
        private static readonly int[] y = new int[] { 213, 265, 319, 373, 428 };
     
        internal enum OptionSelectedEnum
        {
            StartNewGame = 0
        }

        protected override void Run(SceneHandler h)
        {            
            var cursor = h.Add(new MenuCursor());
            h.Add(new ShakeEffect());

            for (int i = 0; i < 5; i++)
            {
                h.Add(new MenuOption(cursor, i, x[i], y[i]));
            }

            h.ScreenRegion.SetBackground(Assembly, "Assets.Menu.MainMenu.png");
            h.FadeFrom(Color.Red, 8);
        }

        protected override void OnMessage(SceneHandler h, object message)
        {
            base.OnMessage(h, message);

            if (message is OptionSelectedEnum mm)
            {
                switch (mm)
                {
                    case OptionSelectedEnum.StartNewGame:
                        {
                            h.FadeOff();
                            h.StopMusic();
                            h.Set(new Level1Scene());
                            break;
                        }
                }
            }
        }

        private class MenuCursor : CursorBase
        {
            private int? selectedOption;

            public MenuCursor()
            {
                this.selectedOption = null;
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                h.CollisionType = Collision2DTypeEnum.Point;
            }

            protected override void OnFrame()
            {
                base.OnFrame();

                if (Mouse.Left && selectedOption.HasValue)
                {
                    switch (selectedOption)
                    {
                        case 0:
                            Scene.SendMessage(OptionSelectedEnum.StartNewGame);
                            break;
                    }
                }
            }

            internal void Select(int index)
            {
                selectedOption = index;
            }

            internal void Unselect(int index)
            {
                if (selectedOption.HasValue && selectedOption.Value == index)
                {
                    selectedOption = null;
                }
            }
        }

        private class MenuOption : EntityBase<SpriteView>
        {
            private ISingleCollisionQuery<MenuCursor> coll;
            private readonly int index;
            private readonly MenuCursor cursor;

            public bool Selected { get; internal set; }

            internal MenuOption(MenuCursor cursor, int index, float x, float y)
                : base(new SpriteView(typeof(MenuOption).Assembly, $"Assets.Menu.MenuOption{index}.png")
                {
                    Alpha = 0f
                })
            {
                this.index = index;
                this.cursor = cursor;
                Location.X = x;
                Location.Y = y;
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);

                h.CollisionType = Collision2DTypeEnum.Rectangle;
                this.coll = h.GetCollision2D<MenuCursor>();
            }

            protected override void OnFrame()
            {                
                if (coll.OnCollision(out MenuCursor val))
                {
                    val.Select(index);
                    View.Alpha = 1f;
                }
                else
                {
                    cursor.Unselect(index);
                    View.Alpha = 0f;
                }
            }
        }

        private class ShakeEffect : EntityBase<SpriteView>
        {
            public ShakeEffect() : base(new SpriteView(typeof(ShakeEffect).Assembly, "Assets.Menu.TitleSmall.png")
            {
                Alpha = 0.5f,
                Center = new PointF(0.5f, 0.5f)
            })
            {
                Location.X = 318f; 
                Location.Y = 96f;
            }

            protected override void OnFrame()
            {
                if (Rand.Next(0, 35) == 0)
                {
                    Location.X += Rand.Next(-32, 32) * 0.5f;
                }

                if (Location.X > 318f)
                {
                    Location.X -= 0.5f;
                }

                if (Location.X < 318f)
                {
                    Location.X += 0.5f;
                }
            }
        }
    }
}
