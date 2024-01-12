using Kriptok.Entities.Base;
using Fostiator;
using Kriptok.Views.Div;

namespace Fostiator.Entities
{
    //------------------------------------------------------------------------------
    // Proceso particula_sangre
    // Maneja la sangre que sale cuando se da un golpe
    // Entradas: 'x,y'        : Coordenadas desde donde sale la sangre
    //           'inc_x,inc_y': Incrementos en el movimiento de la sangre (caida)
    //           'cont_tiempo': Contador del tiempo que aparecera la sangre
    //------------------------------------------------------------------------------
    class BloodParticle : EntityBase<DivFileXView>
    {
        private float xInc;
        private float yInc;
        private int cont_tiempo;

        private int frame;
        private readonly int wait;
        private bool stay = false;

        public BloodParticle(float x, float y, int inc_x, int inc_y, int cont_tiempo) 
            : base(Global2.FileX.GetNewView())
        {
            this.Location.X = x;
            this.Location.Y = y;
            this.xInc = inc_x;
            this.yInc = inc_y;
            this.cont_tiempo = cont_tiempo;

            if (Global2.BloodLevel == 2)
            {
                wait = Rand.Next(0, 400) / 100; // Espera un tiempo al azar
            }
            View.Graph = Rand.Next(50, 53);      // Elige uno de los graficos disponibles al azar

            View.Alpha = 0.5f;
            
            Location.Z = -2;                   // Lo pone por encima de todo
        }

        protected override void OnFrame()
        {
            frame++;

            if (frame < wait)
            {
                return;
            }

            if (cont_tiempo > 0)
            {
                // Mueve la sangre mientras hjaya tiempo
                // Realiza los incrementos en las coordenadas
                Location.X += xInc;           
                Location.Y += yInc;

                // Hace que cada vez caiga mas rapido
                yInc++;          

                // Pero a horizontalmente se mueva mas lento
                if (xInc > 0)       
                {
                    xInc--;
                }

                if (xInc < 0)
                {
                    xInc++;
                }

                if (Location.Y > 410)          // Comprueba si ha tocado el suelo
                {
                    // Deja sangre pegada al suelo de forma aleatoria
                    if (Rand.Next(0, 80) == 0 && Global2.BloodCount < 50 && Location.Y < 480)
                    {
                        Location.Z = 256;          // La pone detras
                        Global2.BloodCount++; // Incrementa el contador de sangre

                        stay = true;
                    }
                }
                cont_tiempo--;          // Incrementa el contador de tiempo
                return;
            }

            if (!stay)
            {
                Die();
                return;
            }
        }
    }
}

