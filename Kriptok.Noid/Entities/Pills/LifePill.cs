using Kriptok.Drawing.Algebra;
using Kriptok.Noid.Scenes;

namespace Kriptok.Noid.Entities.Pills
{
    internal class LifePill : PillBase
    {
        internal LifePill(Vector3F location) : base(location, "P_Life.png")
        {
        }

        protected override void OnPick()
        {
            Global.Lives += 1;
            Scene.SendMessage(LevelSceneMessages.UpdateLives);
        }
    }
}
