using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Views.Sprites;

namespace Exploss.Entities
{
    /// <summary>
    /// Fisparo del jugador.
    /// </summary>
    public class Shot : EntityBase<SpriteView>
    {
        private readonly Vector2F inc;

        public Shot(float x, float y, int direction) 
            : base(new SpriteView(typeof(Shot).Assembly, "Assets.Images.Shot.png"))
        {
            this.Location.X = x;
            this.Location.Y = y;

            // En funcion de la direccion del coche determinamos la trayectoria del disparo.
            switch (direction)
            {
                case 1:
                    inc.Y -= 16;
                    break;
                case 2:
                    inc.Y += 16;
                    break;
                case 3:
                    inc.X += 16;
                    break;
                case 4:
                    inc.X -= 16;
                    break;
                case 5:
                    inc.Y -= 16;
                    inc.X -= 16;
                    break;
                case 6:
                    inc.Y -= 16;
                    inc.X += 16;
                    break;
                case 7:
                    inc.Y += 16;
                    inc.X -= 16;
                    break;
                case 8:
                    inc.Y += 16;
                    inc.X += 16;
                    break;
                default:
                    inc.Y -= 16;
                    break;
            }

            Location.X += inc.X;
            Location.Y += inc.Y;

            Location.Z = -1f;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Ellipse;

            h.Audio.GetSoundHandler("Sounds.Shoot.wav").Play();
        }

        protected override void OnFrame()
        {
            Location.X += inc.X;
            Location.Y += inc.Y;

            // Si el disparo llega a ciertas coordenadas se destruye
            if (Location.X <= 43 || Location.X >= 610 || Location.Y <= 43 || Location.Y >= 343)
            {
                Die();
            }
        }
    }
}
