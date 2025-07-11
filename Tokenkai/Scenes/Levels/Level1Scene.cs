using Kriptok.Scenes;
using System.Drawing;

namespace Tokenkai.Scenes
{
    internal class Level1Scene : LevelSceneBase
    {
        public Level1Scene() : base(1)
        {
        }

        protected override Point GetStartLocation() => new Point(858, 864);        
    }
}