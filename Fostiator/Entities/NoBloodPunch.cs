using Kriptok.Entities.Base;
using Fostiator;
using Kriptok.Views.Div;

namespace Fostiator.Entities
{
    /// <summary>    
    /// Pone un grafico alternativo al de la sangre para los modos sin ella
    /// </summary>
    class NoBloodPunch : ProcessBase<DivFileXView>
    {
        public NoBloodPunch(float x, float y) : base(Global2.FileX.GetNewView())
        {
            Location.X = x;
            Location.Y = y;
        }

        protected override void OnBegin()
        {
            Location.Z = -2;
            for (var i = 60; i <= 66; i++)
            {
                View.Graph = i;
                Frame();    
            }
        }
    }
}

