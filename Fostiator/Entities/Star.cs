using Kriptok.Entities.Base;
using Kriptok.Games.Fostiator;
using Kriptok.Views.Div;

namespace Kriptok.Games.Div.Fostiator
{
    class Star : EntityBase<DivFileXView>
    {
        public Star(int x, int y, int graph) : base(Global2.FileX.GetNewView())
        {
            this.Location.X = x;
            this.Location.Y = y;
            View.Graph = graph;
        }

        protected override void OnFrame()
        {
        }
    }
}

