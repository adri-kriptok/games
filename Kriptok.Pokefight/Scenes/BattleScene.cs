using Kriptok.Audio;
using Kriptok.Drawing.Algebra;
using Kriptok.Pokefight.Common;
using Kriptok.Pokefight.Processes.Base;
using Kriptok.Pokefight.Processes.Battle;
using Kriptok.Pokefight.Processes.Pokemons;
using Kriptok.IO;
using Kriptok.Regions.Scroll;
using Kriptok.Regions.Scroll.Base;
using Kriptok.Scenes;
using Kriptok.Views;
using Kriptok.Views.Gdip;

namespace Kriptok.Pokefight.Scenes
{
    class BattleScene : SceneBase
    {
        private PokemonBase leftPokemon;
        private PokemonBase rightPokemon;
        private ControlConfig controls1;
        private ControlConfig controls2;

        protected override void Init(SceneInitializer init)
        {
            base.Init(init);

            var config = Config.Load<Configuration>();
#if DEBUG
            config.Mute();
#endif
            controls1 = Config.Get<Configuration>().Player1;
            controls2 = Config.Get<Configuration>().Player2;
        }

        protected override void Run(SceneHandler h)
        {
            h.PlayMusic(new PlayMusicOptions(Assembly, "Music.Battle1.mid")
            {
                Loop = true,
                From = 2000
            });

            var scroll = h.StartScroll(new GdipBrushScrollLayer(Assembly, "Resources.Images.Backgrounds.Bridge.png", false, false));

            leftPokemon = h.Add(scroll, new Pidgey(controls1));
            rightPokemon = h.Add(scroll, new Pikachu(controls2));

            scroll.SetTarget(new BattleCam(leftPokemon, rightPokemon));

            leftPokemon.SetStart(rightPokemon, 400);
            rightPokemon.SetStart(leftPokemon, 600);

            h.Add(new EnergyBar(leftPokemon, 80, FlipEnum.None));
            h.Add(new EnergyBar(rightPokemon, 240, FlipEnum.FlipX));

            var text1 = h.Write(Program.BattleNameFont, 80, 24, leftPokemon.GetType().Name);
            text1.LocationZ = -10000;
            text1.SetAlign(ShapeAlignEnum.Left, ShapeVerticalAlignEnum.Middle);

            var text2 = h.Write(Program.BattleNameFont, 240, 24, rightPokemon.GetType().Name);
            text2.LocationZ = -10000;
            text2.SetAlign(ShapeAlignEnum.Right, ShapeVerticalAlignEnum.Middle);

            h.FadeOn();
        }

        private class BattleCam : IScrollTarget
        {
            private readonly PokemonBase leftPokemon;
            private readonly PokemonBase rightPokemon;

            private Vector2F location;

            public BattleCam(PokemonBase leftPokemon, PokemonBase rightPokemon)
            {
                this.leftPokemon = leftPokemon;
                this.rightPokemon = rightPokemon;
            }

            public Vector2F GetLocation2D()
            {
                if (leftPokemon.PokemonX < rightPokemon.PokemonX)
                {
                    CalculateX(leftPokemon.PokemonX, rightPokemon.PokemonX);
                }
                else
                {
                    CalculateX(rightPokemon.PokemonX, leftPokemon.PokemonX);
                }

                if (leftPokemon.Location.Y < rightPokemon.Location.Y)
                {
                    CalculateY(leftPokemon.Location.Y, rightPokemon.Location.Y);
                }
                else
                {
                    CalculateY(rightPokemon.Location.Y, leftPokemon.Location.Y);
                }

                return location;
            }

            private void CalculateX(float minX, float maxX)
            {
                location.X = (maxX - minX) / 2 + minX;
            }

            private void CalculateY(float minY, float maxY)
            {
                location.Y = (maxY - minY) / 2 + minY;
            }
        }
    }
}
