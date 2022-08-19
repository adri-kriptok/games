using Kriptok.Entities.Base;
using Kriptok.Views;
using Kriptok.Views.Shapes;
using Kriptok.Views.Texts;
using System.Drawing;
using Tekrips.Scenes;

namespace Tekrips.Entities
{
    internal class GameOver: EntityBase<TextPanelView>
    {
        public static FontFamily GameFontFamily =
            Fonts.GetFontFamily(typeof(GameOver).Assembly, "8bitoperator.ttf");

        public static readonly SuperFont GameOverFont =
            new SuperFont(new Font(GameFontFamily, 32), Color.White, Color.OrangeRed)
            //.SetBorder(Color.White, 1f)
            .SetShadow(-2, 2, Color.Black);

        public GameOver()
            : base(new TextPanelView(GameOverFont, " GAME OVER", 
                new FillConfig(Color.DarkBlue, Color.Black, GradientDirectionEnum.Vertical),
                    Strokes.Get(Color.LightGray, 2)))
        {
            View.SetShadow(-6, 6, Color.FromArgb(224 ,32, 32, 32));

            Location.Z = -10000;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            Location.X = h.RegionSize.Width / 2;
            Location.Y = h.RegionSize.Height / 2;
        }

        protected override void OnFrame()
        {
        }
    }
}