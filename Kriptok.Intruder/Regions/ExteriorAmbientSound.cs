using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Intruder.Entities;

namespace Kriptok.Intruder.Scenes.Maps.Map01_TheBeach
{
    internal class ExteriorAmbientSound : EntityBase
    {
        private readonly Player player;

        private float tweetCounter = 0f;
        private float nextTweet = 2000f;

        private float shoreCounter = 0f;
        private float nextShore = 2000f;

        public ExteriorAmbientSound(Player player)
        {
            this.player = player;
        }

        protected override void OnFrame()
        {
            tweetCounter += Sys.TimeDelta;

            if (tweetCounter >= nextTweet)
            {
                var note = (byte)Rand.Next(45, 70);

                Audio.PlayMidiNote(MidiInstrumentEnum.BirdTweet, IntruderConsts.TweetMidiChannel, 
                    note, (byte)Rand.Next(32, 127));

                nextTweet = 10000f * (1f / note * Rand.NextF() + 1f);
                tweetCounter = 0f;
            }

            // Si está en la costa, reproduzco sonidos de costa.
            if(player.IsOnShore())
            {
                shoreCounter += Sys.TimeDelta;
                if (shoreCounter >= nextShore)
                {
                    var note = (byte)Rand.Next(50, 60);

                    Audio.PlayMidiNote(MidiInstrumentEnum.Seashore, IntruderConsts.TweetMidiChannel,
                        note, (byte)Rand.Next(32, 127));

                    nextShore = 5000f * (1f / note * Rand.NextF() + 1f);
                    shoreCounter = 0f;
                }
            }

#if DEBUG
            //Add(new PlayerShot(this, partitionedMap));

           // if (Input.KeyPressed(Keys.Tab))
           // {
           //     region.Ambience.Switch();
           // }
#endif
        }
    }
}