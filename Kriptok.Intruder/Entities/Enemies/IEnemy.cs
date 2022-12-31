using Kriptok.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Entities.Enemies
{
    internal interface IEnemy : ILocalizable3D
    {
#if DEBUG || SHOWFPS
        void Pointed();
#endif
        void Hit(int damage);
    }
}
