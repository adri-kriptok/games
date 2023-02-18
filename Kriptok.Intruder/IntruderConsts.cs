using Kriptok.Extensions;
using Kriptok.Views.Shapes;
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

        public static readonly SuperFont LargeHudFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Arial", 12, FontStyle.Bold);
            builder.SetColor(Color.Red, Color.FromArgb(192, 192, 0));
            builder.Border = Strokes.Get(Color.DarkRed);
            builder.SetShadow(1, 1, Color.FromArgb(32, 16, 16));
        });
            
        public static readonly SuperFont SmallHudFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Courier New", 6);
            builder.SetColor(Color.White);
            builder.SetShadow(1, 1, Color.Black);
        });
        
        public static readonly SuperFont MenuFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Arial", 16, FontStyle.Bold);
            builder.SetColor(Color.Red, Color.FromArgb(192, 192, 0));
            builder.Border = Strokes.Get(Color.DarkRed);
            builder.SetShadow(1, 1, Color.FromArgb(32, 16, 16));
        });
        
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
