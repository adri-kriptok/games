using Kriptok.Core;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Queries;
using Kriptok.Entities.Queries.Base;
using Kriptok.Views.Sprites;
using System;

namespace Kriptok.Games.BlastemUp.Player
{
    public class Bonus : EntityBase<IndexedSpriteView>
    {
        /// <summary>
        /// Primer grafico de la animacion.
        /// </summary>
        private readonly int baseGraph = 0;

        /// <summary>
        /// Contador de animacion.
        /// </summary>
        private float animationIndex = 0f;

        internal int info;

        /// <summary>
        /// Consulta para saber si quedó fuera de la pantalla.
        /// </summary>
        private IQuery<bool?> outOfScreen;

        public Bonus(float x, float y, int info) : base(new BonusView())
        {
		    this.Location.X = x;
		    this.Location.Y = y;
		    this.info = info;

            Location.Z = 1f;

            switch (info)       // y prepara los graficos
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    baseGraph = 8 + info * 8;
                    break;
                case 6:
                    baseGraph = 0;
                    break;
                case 7:
                    baseGraph = 8;
                    break;
                default:
                    Die();
                    return;
            }
            View.Graph = baseGraph;

        }

        public override int GetDrawOrder() => 1;

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.SetCollision2DEllipse();
            this.outOfScreen = h.GetOutOfScreenQuery();
        }

        protected override void OnFrame()
        {
            // Mueve el grafico hacia la izquierda
            Location.X -= 6 * Consts.SpeedModifier;           

            View.Graph = baseGraph + (int)animationIndex;

            // Realiza la animacion.
            animationIndex += Consts.SpeedModifier;
            if (animationIndex >= 8f)
            {
                animationIndex = 0f;
            }

            if (outOfScreen.Result.GetValueOrDefault(false))
            {                
                Die();
                return;
            }
        }

        internal void AddLetter(LetterFactory factory)
        {
            factory.Add(info - 1);
        }
    }
}
