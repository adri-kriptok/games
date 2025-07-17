using Kriptok.Common;
using Kriptok.Sdk.RM2000.Views.CharSet.Base;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Adventure.Views.Base
{
    public abstract class CharacterViewBase : RpgMaker2kCharViewBase
    {
        private static readonly int[] walkCycle = new int[] { 2, 1, 0, 1 };

        protected CharacterViewBase(Resource resource, int x, int y, int[,] indexesMatrix)
            : base(resource, x, y, indexesMatrix)
        {
            ReMap(0, 1, 1, 1, 2, 3, 3, 3);
        }

        public CharacterViewBase(Resource resource, int x, int y)
            : base(resource, x, y)
        {
            ReMap(0, 1, 1, 1, 2, 3, 3, 3);
        }

        public CharacterViewBase(Assembly assembly, string resourceName, int x, int y)
            : base(assembly, resourceName, x, y)
        {
            ReMap(0, 1, 1, 1, 2, 3, 3, 3);
        }

        internal void SetWalkingGraph(float v)
        {
            Graph = walkCycle[((int)v) % 4];
        }
    }
}
