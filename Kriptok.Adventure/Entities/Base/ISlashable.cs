using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Regions.Scroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Adventure.Entities.Base
{
    /// <summary>
    /// Entidad que puede ser atacada por el jugador con su espada.
    /// </summary>
    interface ISlashable : IScrollCircleEntity
    {
        void Slash(Vector2F push);
    }
}
