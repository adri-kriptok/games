using Kriptok.Drawing.Algebra;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Intruder.Entities;
using Kriptok.Intruder.Entities.Hud;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Scenes.Maps
{
    public enum MapMessages
    {
        PlayerDied = 0
    }

    abstract class MapSceneBase : SceneBase
    {
        protected sealed override void Run(SceneHandler h)
        {
            var rect = new Rectangle(0, 0, h.ScreenRegion.Rectangle.Width,
                h.ScreenRegion.Rectangle.Height - IntruderConsts.HudHeight);

            Run(h, rect);
        }

        protected abstract void Run(SceneHandler h, Rectangle rect);

        protected void AddHud(SceneHandler h, Player player, Rectangle rect)
        {
            var ammoY = h.ScreenRegion.Rectangle.Height - 16;
            h.Add(h.ScreenRegion, new Hud(player, ammoY, rect));
            
            h.Write(IntruderConsts.LargeHudFont, 72, ammoY, () => player.ShotgunAmmo.ToString())
                .RightMiddle();
            
            h.Write(IntruderConsts.SmallHudFont, 180, IntruderConsts.LifeBarHeight + 5, 
                () => $"{(player.GetLife() * 100f).Floor()}%").RightMiddle();
            h.Write(IntruderConsts.SmallHudFont, 180, IntruderConsts.EnergyBarHeight + 5,
                () => $"{(player.GetEnergy() * 100f).Floor()}%").RightMiddle();
        }

        protected override void OnMessage(SceneHandler h, object message)
        {
            base.OnMessage(h, message);

            if (message is MapMessages msg)
            {
                if (msg == MapMessages.PlayerDied)
                {
                    h.FadeTo(Color.Red, 4);                    
                    h.Kill<EntityBase>();
                    h.WaitForKeyPress();
                    h.FadeFromTo(Color.Red, Color.Black);
                    
                    // Cargo el nivel actual nuevamente.
                    h.Set((SceneBase)Activator.CreateInstance(GetType()));
                }
            }
        }
    }    
}
