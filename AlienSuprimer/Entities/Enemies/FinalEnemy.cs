using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.Alien.Entities.Enemies
{
    internal class FinalEnemy : EntityBase<DirectionalSpriteView>
    {
        public FinalEnemy() : base(new DirectionalSpriteView(typeof(FinalEnemy).Assembly, "Assets.Images.FinalEnemy.png", 3, 1, new int[,]
        {
            { 02 }, { 02 }, { 02 }, { 02 },
            { 02 }, { 02 }, { 02 }, { 01 },
            { 01 }, { 01 }, { 00 }, { 00 },
            { 00 }, { 00 }, { 00 }, { 00 }
        }))
        {
            Location.X = 180f;
            Location.Y = 130f;
        }

        //protected override void OnBegin()
        //{            
        //}

        protected override void OnFrame()
        {
            if (Math.Abs(Global.MainRobot.Location.Y - Location.Y) > 280)
            {
                return;
            }
            
            Angle.Y = GetAngle2D(Global.MainRobot);
            // throw new NotImplementedException();
        }
    }
}
