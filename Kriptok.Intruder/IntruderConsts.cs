using Kriptok.Extensions;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder
{
    /// <summary>
    /// https://fontmeme.com/jurassic-park-font/    
    /// </summary>
    public static class IntruderConsts
    {
        public const int GunMidiChannel = 0;

        public const int TweetMidiChannel = 1; // 45/70

        public const int SeashoreMidiChannel = 2; // 45/70

        /// <summary>
        /// Máxima pendiente que el jugador puede escalar.
        /// </summary>
        public const float MaxSlopePlayerAbleToClimb = 0.2f;

        public static readonly SuperFont LargeHudFont = new SuperFont(new Font("Arial", 12, FontStyle.Bold), Color.Red,
            Color.FromArgb(192, 192, 0))
            .SetBorder(Color.DarkRed)
            .SetShadow(1, 1, Color.FromArgb(32, 16, 16));//.SetAlpha(128));

        public static readonly SuperFont SmallHudFont = new SuperFont(new Font("Courier New", 6), Color.White)
            .SetShadow(1, 1, Color.Black);

        public static readonly SuperFont MenuFont = new SuperFont(new Font("Arial", 16, FontStyle.Bold), Color.Red,
            Color.FromArgb(192, 192, 0))
            .SetBorder(Color.DarkRed)
            .SetShadow(1, 1, Color.FromArgb(32, 16, 16));//.SetAlpha(128));

        /// <summary>
        /// Altura del heads up display.
        /// </summary>
        public const int HudHeight = 32;

#if DEBUG || SHOWFPS
        public const bool Freeze = false;

#endif

        public const int LifeBarHeight = 167;
        public const int EnergyBarHeight = 179;

        public const string VertexDataName = "Type";
        public const string VertexDataValueShore = "Shore";
        public const string VertexDataValueWaterfall = "Waterfall";
    }
}
