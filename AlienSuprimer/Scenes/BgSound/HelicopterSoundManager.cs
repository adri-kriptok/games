using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Games.Alien.Entities.Enemies;
using Kriptok.Helpers;
using System.Linq;

namespace Kriptok.Games.Alien.Scenes
{
    internal class HelicopterSoundManager : EntityBase
    {
        /// <summary>
        /// Indica si debe reproducir el sonido de un helicóptero.
        /// </summary>
        public static bool HelicopterSoundPlaying = false;

        public HelicopterSoundManager()
        {
        }

        protected override void OnFrame()
        {
            var helicopters = Find.All<HelicopterBase>().Count();

            if (helicopters > 0)
            {
                if (!HelicopterSoundPlaying)
                {
                    Audio.PlayMidiNote(MidiInstrumentEnum.Helicopter, 0, 51, 111);
                    HelicopterSoundPlaying = true;
                }
            }
            else
            {
                if (HelicopterSoundPlaying)
                {
                    Audio.StopMidi(0);                    
                    HelicopterSoundPlaying = false;
                }
            }
        }            
    }
}