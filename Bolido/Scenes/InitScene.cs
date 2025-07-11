using Bolido.Scenes.Level0;
using Kriptok.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading.Tasks;

namespace Bolido.Scenes
{
    internal class InitScene : SceneBase
    {
        protected override void Run(SceneHandler h)
        {
            h.Set(new Level0Scene());
        }
    }
}
