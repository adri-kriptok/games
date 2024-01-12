using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Games.Fostiator;
using Kriptok.Views.Div;

namespace Kriptok.Games.Div.Fostiator
{
    /// <summary>    
    /// Maneja la sombra de los muniecos
    /// </summary>
    class Shadow : EntityBase<DivFileXView>
    {
        private readonly EntityBase owner;

        public Shadow(EntityBase owner) : base(Global2.FileX.GetNewView())
        {
            this.owner = owner;
            View.Graph = 1;        // Elige el grafico
            Location.Z = 1;            // Lo pone por debajo del grafico del munieco
            Location.Y = 440;          // Inicializa la coordenada vertical
            View.Alpha = 0.5f;        // Y hace que sea transparente
        }

        protected override void OnFrame()
        {
            
        }

        public override Vector3F GetRenderLocation()
        {
            return new Vector3F(owner.Location.X, Location.Y, -1);
        }
    }
}

