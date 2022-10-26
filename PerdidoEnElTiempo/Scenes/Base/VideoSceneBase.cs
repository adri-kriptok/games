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
}
