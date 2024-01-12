using Kriptok.Entities.Base;
using Fostiator;
using Kriptok.Views.Div;

namespace Fostiator.Entities
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

