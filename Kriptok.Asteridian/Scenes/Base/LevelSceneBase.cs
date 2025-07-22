using Kriptok.Asteridian.Entities;
using Kriptok.Asteridian.Entities.Enemies.Base;
using Kriptok.Asteridian.Regions;
using Kriptok.Audio;
using Kriptok.Core;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Extensions;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Base;
using Kriptok.Scenes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Kriptok.Asteridian.Scenes.Base
{
    /// <summary>
    /// Esta clase representa los comportamientos básicos de todos los niveles del juego.
    /// </summary>
    public abstract class LevelSceneBase : SceneBase
    {

        internal PlayerShip PlayerShip;
        private AsteridianScrollTarget target;
        protected float fSpeedInPixels = 0.5f;
        protected int iSpeedInPixels = 1;
        private LayeredScrollRegionBase scroll = null;

        public LevelSceneBase()
        {
        }

        protected abstract string GetLevelName();

        // protected abstract PlayMusicOptions GetMusicOptions();

        protected override void Run(SceneHandler h)
        {
            GlobalConsts.ScreenSize = h.ScreenRegion.Size;

            var levelRegion = h.ScreenRegion.Rectangle;
            levelRegion.Width = levelRegion.Width * 13 / 16;

            scroll = StartScroll(h, levelRegion);

            // h.PlayMusic(GetMusicOptions());

            scroll.Ambience.SetLightSource(-0.33f, 0.66f, 1f);

            PlayerShip = h.Add(scroll, new PlayerShip(this));

            this.target = new AsteridianScrollTarget(scroll.Rectangle, PlayerShip, ((IAsteridianScroll)scroll).GetLevelHeight());
            scroll.SetTarget(target);
            PlayerShip.Camera = target;

            var list = new LevelEventList();
            GetEventList(list);

            h.Add(scroll, new EventGenerator(list, target));

            h.FadeOn();
            
#if DEBUG
            var start = DateTime.Now;
            //h.ResetTimer();
#endif            
            //var dt = 0f;
            //h.While(() => target.KeepMoving(), () =>
            //{
            //    var dt2 = dt + Sys.TimeDelta;
            //    OnLocation(new OnFrameHandler(h, dt, dt2));
            //    dt = dt2;
            //});

#if DEBUG
            var end = DateTime.Now - start;
#endif
        }

        /// <summary>
        /// Permite agregar los elementos a la lista de eventos.
        /// </summary>        
        protected abstract void GetEventList(LevelEventList list);

        protected abstract LayeredScrollRegionBase StartScroll(SceneHandler h, Rectangle rectangle);

        internal float GetLocationY() => target.GetLocationY();

        protected virtual void OnStartingLevel()
        {
        }

        public class OnFrameHandler
        {
            private SceneHandler h;
            private readonly float timeIntervalFrom;
            private readonly float timeIntervalTo;

            public OnFrameHandler(SceneHandler h, float dt, float dt2)
            {
                this.h = h;
                this.timeIntervalFrom = dt;
                this.timeIntervalTo = dt2;
            }

            public bool Contains(float dt) => (dt >= timeIntervalFrom && dt < timeIntervalTo);
        }

        public class LevelEventList
        {
            private readonly IList<LevelEvent> list = new List<LevelEvent>();

            internal float Timer = 0f;

            internal void Enqueue(float relativeTimer, EnemyBase entity)                
            {
                list.Add(new LevelEvent(Timer, entity));
                Timer += relativeTimer;
            }

            internal Queue<LevelEvent> GetEvents()
            {
                return new Queue<LevelEvent>(list.OrderBy(p => p.Time));
            }
        }

        internal class LevelEvent
        {
            public LevelEvent(float timer, EnemyBase entity)
            {
                this.Time = timer;
                this.Entity = entity;
            }

            internal readonly float Time;            
            internal readonly EnemyBase Entity;
        }

        private class EventGenerator : EntityBase
        {
            private readonly Queue<LevelEvent> queue;
            private readonly AsteridianScrollTarget target;
            private float counter = 0f;

            public EventGenerator(LevelEventList list, AsteridianScrollTarget target)
            {
                this.queue = list.GetEvents();
                this.target = target;
            }

            protected override void OnFrame()
            {
                counter += Sys.TimeDelta;
                var next = queue.Peek();

                while (next.Time < counter)
                {
                    queue.Pop(Execute);
                    
                    if (queue.Count == 0)
                    {
                        Die();
                        return;
                    }

                    next = queue.Peek();
                }
            }

            private void Execute(LevelEvent ev)
            {
                ev.Entity.StartOnTop(target.GetStartOnTopY());
                Add(ev.Entity);
            }
        }
    }
}
