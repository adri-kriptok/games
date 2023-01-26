using Kriptok.Drawing;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Extensions;
using Pacoman.Views;
using System;

namespace Pacoman.Entities
{
    public class Ghost : ProcessBase<PacomanClippedView>
    {
        private readonly int model;
        private int imagen;         // Contador de graficos
        private int num_imagenes;   // Numero de imagenes en que da en pantalla
        private int dir = 3;          // Direccion 0=izq. 1=der. 2=abajo 3=arriba


        public int state;
        private readonly int level;

        /// <summary>
        /// Mapa de durezas.
        /// </summary>
        private readonly FastBitmap8 hardnesses;

        public Ghost(FastBitmap8 hardnesses, int x, int y, int modelo, int level) 
            : base(new PacomanClippedView(new GhostView(modelo)))
        {
            this.level = level;
            this.hardnesses = hardnesses;

            this.Location.X = x;
            this.Location.Y = y;
            this.model = modelo;
        }

        public int X { get { return Location.X.Floor(); } set { Location.X = value; } }

        public int Y { get { return Location.Y.Floor(); } set { Location.Y = value; } }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;
        }

        protected override void OnBegin()
        {            
            Loop(() =>
            {
                // Si el fantasma esta en casa entonces da mas imagenes.
                if (Sample(X, Y) == 11 || state > 0)
                {
                    num_imagenes = 1;
                }
                else
                {
                    num_imagenes = 2;
                }

                // Si el fantasma esta en un cruce selecciona un camino.
                if (GetDirectionCount(X, Y) > 2)
                {
                    dir = GetDirection(X, Y, dir); // Devuelve una direccion correcta.
                }
                else
                {
                    // Si no tiene ningun camino por donde avanza
                    if (!IsPath(X, Y, dir))
                    {
                        dir = GetDirection(X, Y, dir);    // Elige otro camino.
                    }

                    // Aleatoriamente cambia la direccion si se puede.
                    if (Rand.Next(0, 1000) < 2)
                    {
                        dir = GetDirection(X, Y, dir ^ 1);
                    }
                }

                switch (dir)    // Mueve al fantasma.
                {
                    case 0: X -= 2; break;
                    case 1: X += 2; break;
                    case 2: Y += 2; break;
                    case 3: Y -= 2; break;
                }

                // Comprueba si se sale por los lados de la pantalla.
                if (X <= 95) X += 450;
                if (X >= 546) X -= 450;

                // Comprueba el estado del fantasma.
                if (state == 0)
                {
                    View.Graph = dir;   // Elige un grafico de la direccion en estado normal.
                }
                else
                {
                    // El fantasma puede ser comido y parpadea cambiando graficos.
                    if (state < 70 && state % 7 != 0)
                    {
                        View.Graph = 5;
                    }
                    else
                    {
                        View.Graph = 4;
                    }

                    state--;           // Decrementa el contador de estado del fantasma
                }

                // Unicamente muestra la imagen a la velocidad del fantasma
                if (imagen >= num_imagenes)
                {
                    Frame();
                    imagen = 0;
                }
                imagen++;               // Incrementa el contador de imagenes
            });
        }

        private int Sample(int x, int y)
        {
            // Comprueba si son los lados de la pantalla
            if ((x < 105 || x > 534) && (y == 225 || y == 226))
            {
                return 12; // Devuelve un color de camino
            }

            x = (x - 105) / 2;
            y = (y - 1) / 2;

#if DEBUG
            if (x == 107 && y == 111)
            {                
            }
#endif
            // Devuelve el color del mapa de durezas
            return hardnesses.Sample((ushort)x, (ushort)y, 0);
        }

        private bool IsPath(int x, int y, int dir)
        {
            // Color 10=Punto grande 11=Casa de fantasma 12=Camino 14=Punto
            int mapColor = 0;  

            // Comprueba la direccion
            switch (dir)
            {
                case 0: mapColor = Sample(x - 2, y); break;
                case 1: mapColor = Sample(x + 2, y); break;
                case 2: mapColor = Sample(x, y + 2); break;
                case 3: mapColor = Sample(x, y - 2); break;
            }

            // El fantasma no vuelve a entrar en su casa
            if (mapColor == 11 && dir == 2 && Sample(x, y) == 12)
            {
                mapColor = 0;
            }

            // Retorna verdadero (TRUE) si es un camino correcto
            return mapColor.In(11, 10, 12, 14);
        }

        private int GetDirectionCount(int x, int y)
        {
            int dir = 0; //     Numero de direcciones
            int counter = 0; // Contador de uso general

            // Va mirando por todas la direcciones
            do           
            {
                // Si el camino es posible incrementa el contador
                if (IsPath(x, y, counter))
                {
                    dir++; // Contando salidas
                }
            } while (!(counter++ == 3));

            // Devuelve el numero de direcciones posibles
            return dir;    
        }

        private int GetDirection(int x, int y, int oldDirection)
        {
            int[] dir = new int[3];       // Tabla de posibles direcciones
            int num_dir = 0;                // Numero de direcciones
            
            // Dirección final.
            int finalDirection = 0;              
            
            int dir1 = 0;                     // Direcciones temporales
            int dir2 = 0;

            // Contando direcciones
            for (finalDirection = 0; finalDirection <= 3; finalDirection++)
            {
#if DEBUG
                if (finalDirection == 3)
                {                    
                }
#endif

                // Comprueba si se puede avanzar en la direccion de contador0
                if (IsPath(x, y, finalDirection) && oldDirection != (finalDirection ^ 1))
                {
                    dir[num_dir] = finalDirection; // Si se puede se guarda
                    num_dir++;              // Y se incrementa el contador de direcciones posibles
                }
            }

            // Cambia la direccion si no hay ninguna otra
            if (num_dir == 0)
            {
                dir[num_dir] = oldDirection ^ 1;
                num_dir++;
            }

            // Selecciona la direccion de acuerdo con el nivel
            finalDirection = dir[Rand.Next(0, num_dir - 1)];

            // Aleatoriamente y segun el nivel elige una direccion u otra
            if (Rand.Next(0, 100) < Global.GhostIntellgence[level])
            {
                // Mira en que estado estaba el fantasma
                if (state == 0)
                {
                    // Estado "persecutor" de paco.
                    if (Global.Player != null)       // Comprueba si paco existe
                    {
                        // Es el estado de poder comer y va hacia paco
                        // Mira que distancia es menor, la horizontal o la vertical
                        if (Math.Abs(Global.Player.Location.X - Location.X) > 
                            Math.Abs(Global.Player.Location.Y - Location.Y))
                        {
                            // Mira si a la derecha o a la izquierda
                            if (Global.Player.Location.X > Location.X)
                            {
                                dir1 = 1; // Guarda la primera direccion posible
                            }
                            else
                            {
                                dir1 = 0;
                            }
                            if (Global.Player.Location.Y > Location.Y)
                            {
                                dir2 = 2; // Guarda una segunda direccion posible
                            }
                            else
                            {
                                dir2 = 3;
                            }
                        }
                        else
                        {            
                            // La diferencia vertical es mayor
                            if (Global.Player.Location.Y > Location.Y)
                            {
                                dir1 = 2; // Guarda la primera direccion posible
                            }
                            else
                            {
                                dir1 = 3;
                            }
                            if (Global.Player.Location.X > Location.X)
                            {
                                dir2 = 1; // Guarda una segunda direccion posible
                            }
                            else
                            {
                                dir2 = 0;
                            }
                        }
                    }
                }
                else
                {
                    // Es el estado de ser comido, se aleja de paco
                    // Se comprueba que diferencia es mayor, la horizontal o la vertical
                    if (Math.Abs(Global.Player.Location.X - Location.X) < 
                        Math.Abs(Global.Player.Location.Y - Location.Y))
                    {
                        // Y comprueba si es a la izquierda o a la derecha
                        if (Global.Player.Location.X > Location.X)
                        {
                            dir1 = 0;     // Guarda la primera direccion posible
                        }
                        else
                        {
                            dir1 = 1;
                        }
                        if (Global.Player.Location.Y > Location.Y)
                        {
                            dir2 = 3;     // Guarda una segunda direccion posible
                        }
                        else
                        {
                            dir2 = 2;
                        }
                    }
                    else
                    {
                        // Mira si hacia arriba o hacia abajo
                        if (Global.Player.Location.Y > Location.Y)
                        {
                            dir1 = 3;    // Guarda la primera direccion posible
                        }
                        else
                        {
                            dir1 = 2;
                        }
                        if (Global.Player.Location.X > Location.X)
                        {
                            dir2 = 0;    // Guarda una segunda direccion posible
                        }
                        else
                        {
                            dir2 = 1;
                        }
                    }
                }

                // Si se puede avanzar en la primera direccion posible, la devuelve
                if (oldDirection == dir1 && IsPath(x, y, dir1))
                {
                    return dir1;
                }
                else
                {    // Si no, si se puede avanzar, devuelve la segunda posible direccion
                    if (!IsPath(x, y, dir1) && oldDirection == dir2 && IsPath(x, y, dir2))
                    {
                        return dir2;
                    }
                }
                // Si no devuelve la posicion en la que sea posible avanzar
                if (IsPath(x, y, dir1) && oldDirection != (dir1 ^ 1))
                {
                    return dir1;
                }
                else
                {
                    if (IsPath(x, y, dir2) && oldDirection != (dir2 ^ 1))
                    {
                        return dir2;
                    }
                }
            }
            return finalDirection;
        }

        internal void Eaten()
        {
            // Crea unos ojos de fantasma.
            Add(new Eyes(hardnesses, X, Y, model, level));

            // Elimina el fantasma comido
            Die();
        }
    }
}
