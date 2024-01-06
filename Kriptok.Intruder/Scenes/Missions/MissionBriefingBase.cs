using Kriptok.Drawing;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Helpers;
using Kriptok.Intruder.Scenes.Maps.Map01_TheBeach;
using Kriptok.Scenes;
using Kriptok.Views.Base;
using Kriptok.Views.Sprites;
using Kriptok.Views.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Scenes.Missions
{
    internal abstract class MissionBriefingBase : SceneBase
    {
        private static readonly SuperFont font = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Courier New", 8f);
            builder.SetColor(ColorHelper.Green);
        });            

        protected override void Run(SceneHandler h)
        {
            for (int i = 0; i < 6; i++)
            {
                h.Add(new AdnElement(i)
                {
                    Location = new Vector3F(45, i * 32 + 16, 0f)
                });
            }

            h.FadeOn();

            var text = h.Add(new TextWritter(GetMissionText()));

            h.WaitWhile(() => !text.IsReady());
            h.WaitForKeyPress();

            h.FadeOff();

            h.Set(new TheBeachMapScene());
        }

        protected abstract string GetMissionText();

        private class TextWritter : EntityBase<TextView>
        {
            private readonly string fullText;
            private int counterToNext = 100;
            private char nextLetter = '\0';
            private int textLetter = 0;
            private bool writting = true;

            public TextWritter(string text) : base(new TextView(font, string.Empty))
            {
                fullText = text;
                nextLetter = text.First();

                View.Center = new PointF(0f, 0f);
                
                Location.X = 100f;
                Location.Y = 15f;
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                // View.MaxWidth = (int)(h.RegionSize.Width - Location.X - 12);

                // midi percussion 36.
            }

            protected override void OnFrame()
            {
                if (!writting)
                {
                    return;
                }

                if (counterToNext-- <= 0)
                {
                    View.Message += nextLetter;

                    textLetter += 1;

                    if (textLetter >= fullText.Length)
                    {
                        writting = false;
                        return;
                    }
                    
                    nextLetter = fullText[textLetter];


                    if (nextLetter.Equals(Environment.NewLine))
                    {
                        counterToNext = 20;
                    }
                    else 
                    {
                        counterToNext = 4;
                    }

                    if (!string.IsNullOrWhiteSpace(nextLetter.ToString()))
                    {
                        Audio.PlayMidiPercussionNote(36, 127);
                    }
                }
            }

            /// <summary>
            /// Indica si terminó de presentar el texto.
            /// </summary>            
            internal bool IsReady() => !writting;
        }

        private class AdnElement : EntityBase<IndexedSpriteView>
        {
            private int counter = 0;

            public AdnElement(int i) : base(
                new IndexedSpriteView(typeof(AdnElement).Assembly, "Assets.Images.Menues.Adn.png", 1, 7))
            {
                for (int j = 0; j < i; j++)
                {                    
                    View.Rotate();
                }

                View.Transform = new ColorTransform()
                {
                    A = 1f,
                    R = new ColorVector(1f, 0f, 0f),
                    G = new ColorVector(0.25f, 0f, 0f),
                    B = new ColorVector(0.25f, 0f, 0f)
                };

                View.RotateTransform(i * 90);

                //View.Transform.R.R = 0;
                //View.Transform.R.G = 0;
                //View.Transform.R.B = 0;
            }

            protected override void OnFrame()
            {
                if (counter++ == 10)
                {
                    View.Rotate();
                    counter = 0;
                }
            }
        }
    }
}
