using Kriptok.Div;
using Kriptok.Entities.Base;
using Kriptok.Games.Fostiator;
using Kriptok.Views.Div;
using Kriptok.Views.Gdip;
using Kriptok.Views.Sprites;
using System;

namespace Kriptok.Games.Dgs.Fostiator.Entities
{
    /// <summary>    
    /// Pone los graficos estaticos del marcador y la pantalla de opciones
    /// Entradas: fichero grafico, coordenadas, grafico, bandera (der/izq.),
    ///           zona de la pantalla 
    /// </summary>
    public class GenericObject : EntityBase<DivFileXView>
    {
        private readonly Func<int> getGraph;

        public GenericObject(int x, int y, int graph, FlipEnum flags, bool grayscale = false)
            : this(x, y, () => graph, flags, grayscale)
        {
        }

        public GenericObject(int x, int y, Func<int> graph, FlipEnum flags, bool grayscale = false)
            : this(Global2.FileX, x, y, graph, flags, grayscale)
        {            
        }

        public GenericObject(DivFileX file, int x, int y, int graph, FlipEnum flags, bool grayscale = false)
           : this(file, x, y, () => graph, flags, grayscale)
        {
        }

        public GenericObject(DivFileX file, int x, int y, Func<int> graph, FlipEnum flags, bool grayscale = false)
           : base(file.GetNewView())
        {
            Location.X = x;
            Location.Y = y;

            this.getGraph = graph;
            View.Flip = flags;

            if (grayscale)
            {
                View.ToGrayScale();
            }

            // Lo pone por debajo de otros graficos
            Location.Z = 10;
        }

        protected override void OnFrame()
        {
            View.Graph = getGraph();
        }
    }
}
