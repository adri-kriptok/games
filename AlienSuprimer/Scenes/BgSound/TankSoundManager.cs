using Kriptok.Entities.Base;
using Kriptok.Games.Alien.Entities.Enemies;
using Kriptok.Helpers;
using Kriptok.IO;
using System.IO;
using System.Linq;
using System.Media;

namespace Kriptok.Games.Alien.Scenes
{
    internal class TankSoundManager : EntityBase
    {
        private bool playingSound = false;
        private TankCannon[] tanks;

        private Stream stream;
        private SoundPlayer soundPlayer;

        public TankSoundManager()
        {
            this.playingSound = false;

            stream = ResourcesHelper.GetStream(Assembly, "Assets.Sounds.MOTOR.WAV");
            soundPlayer = new SoundPlayer(stream);
        }

        protected override void OnFrame()
        {
            if (tanks != null)
            {
                if (playingSound)
                {
                    if (tanks.All(p => p.IsOutOfScreen()))
                    {
                        soundPlayer.Stop();
                        playingSound = false;
                    }
                }
                else
                {
                    if (tanks.Any(p => !p.IsOutOfScreen()))
                    {
                        if (Config.Get<BaseConfiguration>().SoundOn)
                        {
                            soundPlayer.PlayLooping();
                        }
                        playingSound = true;
                    }
                }
            }
            else
            {
                tanks = Find.All<TankCannon>().ToArray();
            }
        }

        protected override void Dispose()
        {
            base.Dispose();

            if (soundPlayer != null)
            {
                soundPlayer.Stop();
                soundPlayer.Dispose();
                soundPlayer = null;
            }

            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }
    }
}