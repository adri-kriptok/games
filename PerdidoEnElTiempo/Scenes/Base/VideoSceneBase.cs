using Kriptok.Common;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Scenes;
using Kriptok.Views.Sprites;
using Kriptok.Views.Videos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerdidoEnElTiempo.Scenes.Base
{
    abstract class VideoSceneBase : SceneBase
    {
        internal const string AUTODESTRUCT = "AUTRODESTRUCCION";

        public void GameOver(SceneHandler h, int endScene)
        {
            h.Wait(250);
            h.FadeOff();
            h.Set(new GameOverScene(endScene));
        }

        internal static IVideoEntity PlayVideo(SceneHandler h, Resource r, bool autoKill = true)
        {
            var v = h.StartVideo(new FlicDecoder(r));
#if DEBUG
            h.WaitOrKeyPress(v);
#else
            h.Wait(v);
#endif
            if (autoKill)
            {
                v.Kill();
                return null;
            }
            return v;
        }

        internal static IVideoEntity PlayVideo(SceneHandler h, Resource r, Action beforeStart, bool autoKill = true)
        {
            var v = h.StartVideo(new FlicDecoder(r));
            beforeStart();
#if DEBUG
            h.WaitOrKeyPress(v);
#else
            h.Wait(v);
#endif
            if (autoKill)
            {
                v.Kill();
                return null;
            }
            return v;
        }

        internal void PlayTimeTravel(SceneHandler h)
        {
            h.FadeTo(Color.White);

            var v = h.StartVideo(new FlicDecoder(Resource.Get(Assembly, "Assets.Videos.Intro.A06.FLI")));
            h.WaitFrame();
            h.FadeFrom(Color.White);
            h.Wait(v);

            h.FadeTo(Color.White);
        }

        protected override void OnMessage(SceneHandler h, object message)
        {
            base.OnMessage(h, message);

            if (message as string == AUTODESTRUCT)
            {
                h.Set(new Space8Scene());
            }
        }

        internal static Tuple<ITextEntity, AutoDestructController> AutoDestructCheck(SceneHandler h)
        {
            if (Global.AutoDestructionTimer > 0)
            {
                return new Tuple<ITextEntity, AutoDestructController>
                (                
                    h.Write(Global.DangerFont, h.ScreenRegion.Size.Width / 2, 40,
                        () => $"AUTODESTRUCCIÓN EN {Global.AutoDestructionTimer}").CenterMiddle(),
                    h.Add(new AutoDestructController())
                );
            }
            return new Tuple<ITextEntity, AutoDestructController>(null, null);
        }
    }

    /// <summary>
    /// Marco para solucionar un problema en el video.
    /// </summary>
    internal class Frame : EntityBase<SpriteView>
    {
        public Frame() : base(new SpriteView(typeof(Frame).Assembly, "Assets.Images.Frame.png")
        {
            Center = new PointF(0f, 0f)
        })
        {
            Location.Z = -1;
        }

        protected override void OnFrame()
        {
        }
    }

    /// <summary>
    /// Controla la autodestrucción.
    /// </summary>
    internal class AutoDestructController : EntityBase
    {
        private DateTime lastState;

        public AutoDestructController()
        {
            this.lastState = DateTime.Now;
        }

        protected override void OnFrame()
        {
            if ((DateTime.Now - lastState).TotalSeconds > 1)
            {
                Global.AutoDestructionTimer -= 1;
                lastState = DateTime.Now;
            }

            if (Global.AutoDestructionTimer <= 0)
            {
                Scene.SendMessage(VideoSceneBase.AUTODESTRUCT);
            }
        }
    }
}
