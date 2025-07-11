using Kriptok.Audio;
using Kriptok.Common;
using Kriptok.Div;
using Kriptok.Entities;
using Kriptok.Extensions;
using Kriptok.Scenes;
using Kriptok.Views.Shapes;
using Kriptok.Views.Videos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Games.Alien.Scenes
{
    public class IntroScene : SceneBase
    {
        protected override void Run(SceneHandler h)
        {
            var video = new VideoWithSounds(h.StartVideo(new FlicDecoderWithSounds(h, Resource.Get(Assembly, "Assets.INTRO.FLI"))));
            //var video = new VideoWithSounds(h.StartVideo(new FlicDecoder(Resource.Get(Assembly, "Assets.INTRO.FLI"))));
            video.ScaleTo(h.ScreenRegion.Size);
            video.Pause();
            h.FadeOn(byte.MaxValue);
            video.Unpause();
            //h.Wait(video);                       


            h.While(() => !video.EndOfVideo(), () =>
            {
                var v = video;
            });            

            h.FadeTo(Color.White, 8);
            video.Kill();

            h.Set(new MainMenuScene());
        }

        public class VideoWithSounds : IVideoEntity
        {
            private readonly IVideoEntity video;

            public VideoWithSounds(IVideoEntity videoEntity)
            {
                this.video = videoEntity;
            }

            public bool Loop { get => video.Loop; set => video.Loop = value; }

            public bool EndOfVideo() => video.EndOfVideo();            

            public void GoToEnd() => video.GoToEnd();            

            public void Kill() => video.Kill();            

            public void Pause() => video.Pause();            

            public void ScaleTo(Size size) => video.ScaleTo(size);            

            public void Unpause() => video.Unpause();
        }

        public class FlicDecoderWithSounds : FlicDecoder
        {
            private readonly SceneHandler handler;
            private ISoundHandler motorSound;

            public FlicDecoderWithSounds(SceneHandler h, Resource resource) : base(resource)
            {
                this.handler = h;                
            }            

            protected override void OnFrame(int currentFrame)
            {
                base.OnFrame(currentFrame);

                if (currentFrame == 0)
                {
                }
                else if (currentFrame == 1)
                {
                    motorSound = handler.PlaySoundLooping(typeof(FlicDecoderWithSounds).Assembly, "Assets.Sounds.MOTOR.WAV");
                    handler.PlayMidiNote(MidiInstrumentEnum.Helicopter, 0, 51, 111);
                }
                else if (currentFrame.In(2, 27, 44, 77, 107))
                {
                    handler.PlayWave(typeof(FlicDecoderWithSounds).Assembly, "Assets.Sounds.EXPLOSI8.WAV");
                }
                else if (currentFrame == 70)
                {
                    handler.PlayWave(typeof(FlicDecoderWithSounds).Assembly, "Assets.Sounds.LASER6.WAV");
                }
                else if (currentFrame == 121)
                {
                    motorSound.Stop();
                    handler.StopMidiChannel(0);
                }
            }
        }
    }
}
