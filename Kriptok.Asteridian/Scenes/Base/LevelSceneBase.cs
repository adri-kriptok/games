using Kriptok.Asteridian.Entities;
using Kriptok.Asteridian.Entities.Enemies.Base;
using Kriptok.Asteridian.Entities.Player;
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
    public interface IDifficultyManager
    {
        /// <summary>
        /// Obtiene la energía base con la que empieza un enemigo, en base a la dificultad y la salud inicial. 
        /// </summary>        
        float CalculateEnemyHealth(float health);
    }

    /// <summary>
    /// Esta clase representa los comportamientos básicos de todos los niveles del juego.
    /// </summary>
    public abstract class LevelSceneBase : SceneBase, IDifficultyManager
    {
        internal PlayerShip PlayerShip = null;
        internal PlayerHud PlayerHud = null;

        private AsteridianScrollTarget target = null;
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
            levelRegion.Width = levelRegion.Width - GlobalConsts.HudWidth;

            scroll = StartScroll(h, levelRegion);

            // h.PlayMusic(GetMusicOptions());

            scroll.Ambience.SetLightSource(-0.33f, 0.66f, 1f);

            PlayerShip = h.Add(scroll, new PlayerShip(this));
            PlayerHud = h.Add(new PlayerHud(this));

            this.target = new AsteridianScrollTarget(scroll.Rectangle, PlayerShip, ((IAsteridianScroll)scroll).GetLevelHeight());
            scroll.SetTarget(target);
            PlayerShip.Camera = target;

            var list = new LevelEventContext(levelRegion.Width);
            LoadLevelEvents(list);

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
        protected abstract void LoadLevelEvents(LevelEventContext list);

        protected abstract LayeredScrollRegionBase StartScroll(SceneHandler h, Rectangle rectangle);

        internal float GetLocationY() => target.GetLocationY();

        /// <inheritdoc/>
        public float CalculateEnemyHealth(float health) => health;

        /// <summary>
        /// Lista de eventos a agregar en el nivel.
        /// </summary>
        public class LevelEventContext
        {
            private readonly IList<LevelEvent> list = new List<LevelEvent>();

            /// <summary>
            /// Contador de tiempo durante la construcción del nivel.
            /// </summary>
            private float timer = 0f;

            /// <summary>
            /// Ancho de la pantalla.
            /// </summary>
            public readonly int ScreenWidth;

            public LevelEventContext(int screenWidth)
            {
                ScreenWidth = screenWidth;
            }

            /// <summary>
            /// Encola los eventos después del tiempo indicado.
            /// </summary>            
            internal void Enqueue(float relativeTimer, EnemyBase entity)                
            {
                list.Add(new LevelEvent(timer, entity));
                timer += relativeTimer;
            }

            /// <summary>
            /// Obtiene todos los eventos ordenados por tiempo.
            /// </summary>            
            internal Queue<LevelEvent> GetEvents() => new Queue<LevelEvent>(list.OrderBy(p => p.Time));            

            /// <summary>
            /// Agrega tiempo sin nada al contador de eventos.
            /// </summary>            
            internal void Wait(int v) => timer += v;
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

            public EventGenerator(LevelEventContext list, AsteridianScrollTarget target)
            {
                this.queue = list.GetEvents();
                this.target = target;
            }

            protected override void OnFrame()
            {
                counter += Sys2.TimeDelta;
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
