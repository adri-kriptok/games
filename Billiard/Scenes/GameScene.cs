using Kriptok.Scenes;
using Billiard.Entities;
using Kriptok.Views;
using Kriptok.Views.Texts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Billiard.Scenes
{
    class GameScene : SceneBase
    {
        internal static readonly SuperFont WhiteSmallFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Bahnschrift", 15, FontStyle.Bold);
            builder.SetColor(Color.White);
            builder.SetShadow(1, 1, Color.Gray);
        });                                    

        private static readonly SuperFont whiteBigFont = SuperFont.Build(builder =>
        {
            builder.Font = new Font("Bahnschrift", 36, FontStyle.Bold);
            builder.SetColor(Color.White);
            builder.SetShadow(2, 2, Color.Gray);
        });                                    

        private bool finished = false;

        protected override void Run(SceneHandler h)
        {
            // Imprime los textos necesarios
            h.Write(WhiteSmallFont, 12, 6, "Jugador 1").LeftTop();
            h.Write(WhiteSmallFont, 628, 6, "Jugador 2").RightTop();
            h.Write(whiteBigFont, 115, 0, () => Global.ScorePlayer1.ToString()).LeftTop();
            h.Write(whiteBigFont, 525, 0, () => Global.ScorePlayer2.ToString()).RightTop();

            // Pone los graficos de la mesa y la bola de efectos
            h.ScreenRegion.SetBackground(bg =>
            {
                bg.Blit(Assembly, "Assets.Table.png", 11, 54);
                bg.Draw(Assembly, "Assets.EffectBall.png", 576, 424);
            });

            // Crea el proceso que maneja los efectos
            Global.EffectObject = h.Add(new Effect());

            // Escribe el texto del turno            
            h.Write(WhiteSmallFont, 12, 428, () => $"Jugador {Global.CurrentTurn}    {Global.Mode.ToText()}").LeftTop();

            h.FadeOn();

            // Crea las bolas
            Global.WhiteBall = h.Add(new Ball(320, 240, "Assets.BallWhite.png"));
            Global.YellowBall = h.Add(new Ball(128, 240 - 48, "Assets.BallYellow.png"));
            Global.RedBall = h.Add(new Ball(128, 240 + 48, "Assets.BallRed.png"));

            // Valores iniciales
            finished = false;
            Global.CollisionOtherBall = false;
            Global.CollisionRedBall = false;
            Global.ScorePlayer1 = 0;
            Global.ScorePlayer2 = 0;
            Global.LastBall = null;
            Global.CollisionBall = null;
            Global.CurrentTurn = 2;

            h.Repeat(() =>
            {
                // Bucle principal
                // Comprueba que este parado todo y que no haya palo, es decir, el fin de la tirada
                if (Global.WhiteBall.Speed == 0 &&
                    Global.YellowBall.Speed == 0 &&
                    Global.RedBall.Speed == 0 &&
                    h.FindFirst<Stick>() == null)
                {
                    // Si no ha chocado con las dos bolas a la vez,
                    // es que no ha sido una carambola
                    if (!(Global.CollisionOtherBall && Global.CollisionRedBall))
                    {
                        // Cambia de turno
                        if (Global.CurrentTurn == 1)
                        {
                            Global.CurrentTurn = 2;
                        }
                        else
                        {
                            Global.CurrentTurn = 1;
                        }
                    }
                    else
                    {
                        // Carambola conseguida
                        if (Global.CurrentTurn == 1)  // Mira quien jugaba en ese turno
                        {
                            // Mira los puntos obtenidos hasta ahora
                            if (++Global.ScorePlayer1 == Global.WinningScore)
                            {
                                var winnerText = h.Write(WhiteSmallFont, 800, 240, "Jugador uno gana")
                                    .SetAlign(ShapeAlignEnum.Center, ShapeVerticalAlignEnum.Middle);

                                for (var x = 800; x >= 320; x -= 8)
                                {
                                    winnerText.LocationX = x;
                                    h.WaitFrame();
                                }

                                for (var i = 0; i < 29; i++)
                                {
                                    h.WaitFrame();
                                }
                                finished = true;
                            }
                        }
                        else
                        {
                            // Comprueba y hace lo mismo que antes pero para el otro jugador
                            if (++Global.ScorePlayer2 == Global.WinningScore)
                            {
                                var winnerText = h.Write(WhiteSmallFont, 800, 240, "Jugador dos gana")
                                    .SetAlign(ShapeAlignEnum.Center, ShapeVerticalAlignEnum.Middle);

                                for (var x = 800; x >= 320; x -= 8)
                                {
                                    winnerText.LocationX = x;
                                    h.WaitFrame();
                                }

                                for (var i = 0; i <= 29; i++)
                                {
                                    h.WaitFrame();
                                }

                                finished = true;
                            }
                        }
                    }

                    // Comprueba si no ha acabado
                    if (finished == false)
                    {
                        // Pone el palo dependiendo del turno al lado de la bola apropiada
                        if (Global.CurrentTurn == 1)
                        {                            
                            h.Add(new Stick(Global.WhiteBall)); h.WaitFrame();
                        }
                        else
                        {
                            h.Add(new Stick(Global.YellowBall)); h.WaitFrame();
                        }
                        Global.CollisionOtherBall = false;
                        Global.CollisionRedBall = false;
                    }
                }

                // Si se pulsa la tecla escape sale del juego
                if (Input.Key(Keys.Escape))
                {
                    finished = true;
                }
            }, () => finished);

            h.FadeOff();

            h.Set(new TitleScene());
        }
    }
}
