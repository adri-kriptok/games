using Kriptok.Audio;
using Kriptok.Div;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Collisions.Queries;
using Kriptok.Views.Div;
using Kriptok.Views.Gdip;
using Kriptok.Views.Primitives;
using Kriptok.Views.Sprites;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static Fostiator.Global2;

namespace Fostiator.Entities
{
    public class Fighter : ProcessBase<DivFileXView>
    {
        // Secuencia de animacin de cada uno de los estados de los muniecos
        private static readonly int[] anim0 = new int[] { 1, 1, 1, 14, 14, 15, 15, 16, 16, 17, 17, 17, 16, 16, 15, 15, 14, 14 };
        private static readonly int[] anim1 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static readonly int[] anim2 = new int[] { 1, 8, 7, 6, 5, 4, 3, 2 };
        private static readonly int[] anim3 = new int[] { 8, 9, 10, 11, 12, 13 };
        private static readonly int[] anim4 = new int[] { 13 };
        private static readonly int[] anim5 = new int[] { 13, 12, 11, 10, 9, 8 };
        private static readonly int[] anim6 = new int[] { 18, 19, 20, 21, 21, 22, 22, 22, 22, 23, 23, 23, 23, 24, 24, 24, 24, 24, 24, 24, 24 };
        private static readonly int[] anim7 = new int[] { 52, 54, 54, 54, 54, 53, 53, 52, 51 };
        private static readonly int[] anim8 = new int[] { 25, 26, 27, 28, 28, 28, 28, 27, 27, 26, 25 };
        private static readonly int[] anim9 = new int[] { 29, 30, 31, 32, 33, 33, 34, 35, 36, 37, 38, 39 };
        private static readonly int[] anim10 = new int[] { 29, 30, 31, 32, 32, 32, 31, 31, 30, 30, 29 };
        private static readonly int[] anim11 = new int[] { 18, 19, 20, 21, 21, 22, 22, 24, 40, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41 };
        private static readonly int[] anim12 = new int[] { 1, 42, 42, 43, 43, 42, 42 };
        private static readonly int[] anim13 = new int[] { 1, 42, 42, 43, 43, 44, 44, 45, 45, 46, 46, 47, 47, 48, 48, 49, 49, 50 };

        internal Fighter Enemy;
        internal int Health;
        private readonly FighterControlEnum tipo_control;
        private readonly int luchador;

        /// <summary>
        /// Indicador de la animacion actual.
        /// </summary>
        public FighterStatusEnum estado;

        /// <summary>
        /// Paso de la animacion actual.
        /// </summary>
        public int animationStep;

        /// <summary>
        /// Desplazamientos del munieco.
        /// </summary>
        public int inc_y, inc_x;

        public Fighter(int x, int y, DivFileX file, FlipEnum flip,
            FighterControlEnum tipo_control, int luchador, bool grayscale = false)
            : base(file.GetNewView())
        {         
            this.Location.X = x;
            this.Location.Y = y;
            this.View.Flip = flip;
            this.tipo_control = tipo_control;
            this.luchador = luchador;

            if (grayscale)
            {
                View.ToGrayScale();
                //View.RotateTransformAverage(90);
            }
        }

        /// <summary>
        /// Variable temporal para el estado del munieco.
        /// </summary>
        private FighterStatusEnum nuevo_estado;

        /// <summary>
        /// Variable temporal para la posicion de la animacion del munieco.
        /// </summary>
        private int nuevo_paso;

        /// <summary>
        /// Fuerza de cada uno de los golpes.
        /// </summary>
        private readonly int[] fuerza_golpe = new int[] { 35, 35, 55, 35, 35 };

        /// <summary>
        /// Fuerza variable dependiendo del nivel y del luchador.
        /// </summary>
        private int suma_fuerza;

        /// <summary>
        /// Sonidos que hace el peleador.
        /// </summary>
        private ISoundHandler turn05Sound, turn06Sound, turn07Sound, turn08Sound, turn09Sound,
            uah00Sound, whimper2Sound, whimper3Sound, aaah01Sound;

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;

            turn05Sound = h.Audio.GetWaveHandler("Sound.TURN05.WAV");
            turn06Sound = h.Audio.GetWaveHandler("Sound.TURN06.WAV");
            turn07Sound = h.Audio.GetWaveHandler("Sound.TURN07.WAV");
            turn08Sound = h.Audio.GetWaveHandler("Sound.TURN08.WAV");
            turn09Sound = h.Audio.GetWaveHandler("Sound.TURN09.WAV");

            uah00Sound = h.Audio.GetWaveHandler("Sound.UAH00.WAV");
            whimper2Sound = h.Audio.GetWaveHandler("Sound.WHIMPER2.WAV");
            whimper3Sound = h.Audio.GetWaveHandler("Sound.WHIMPER3.WAV");
            aaah01Sound = h.Audio.GetWaveHandler("Sound.AAAH01.WAV");
        }

        protected override void OnBegin()
        {
            Health = 203;        // Inicializa la energia
            Frame();
            Add(new Shadow(this));           // Crea la sombra del munieco

            if (tipo_control != 0)
            {                      
                // Si el ordenador no lleva este munieco
                if (Enemy.tipo_control == 0)
                {          
                    // Pero lleva el del enemigo
                    switch (Global2.DifficultyLevel)
                    {               
                        case 1:
                            // Dependiendo del nivel de juego (dificultad)
                            suma_fuerza = -10;
                            break;    
                            // Hace que los golpes quiten menos energia
                        case 2:
                            suma_fuerza = -18;
                            break;
                    }
                }
                else
                {
                    suma_fuerza = 10; // Humano vs Humano, iguala las fuerzas
                }
            }

            switch (luchador)        // Dependiendo del tipo de luchador
            {
                case 0: suma_fuerza -= 8; break; // Ripley pega menos
                case 1: suma_fuerza += 6; break; // Bishop es el segundo que pega mas
                case 2: suma_fuerza += 4; break; // Alien es el segundo que pega menos
                case 3: suma_fuerza += 8; break; // Nostrodomo pega mas que nadie
            }

            // Actualiza la fuerza de los golpes seg£n los jugadores
            for (int i = 0; i <= 4; i++)
            {
                fuerza_golpe[i] += suma_fuerza;
            }

            Loop(() =>
            {
                // Actualiza la variable temporal del estado del munieco
                nuevo_estado = estado;
                switch (estado)
                {         // Comprueba el estado del munieco

                    case FighterStatusEnum._parado:
                        View.Graph = anim0[animationStep++];        // Anima el grafico
                        if (animationStep == anim0.Length)
                        {    // Si no hay mas graficos en la animacion
                            animationStep = 0;                 // Empieza desde 0
                        }
                        if (View.Flip != FlipEnum.None)
                        {             // Hace que los muniecos se miren
                            if (Enemy.Location.X < Location.X)
                            {
                                View.Flip = FlipEnum.None;
                            }

                        }
                        else
                        {
                            if (Enemy.Location.X > Location.X)
                            {
                                View.Flip = FlipEnum.FlipX;
                            }
                        }
                        // Comprueba si se quiere cambiar de estado
                        if (Control(FighterActionEnum.Hit, this))
                        {      // Comprueba si se quiere golpear
                            nuevo_estado = FighterStatusEnum._punietazo; // Y golpea...
                        }
                        if (Control(FighterActionEnum.Jump, this))
                        {        // Comprueba si se quiere saltar
                            nuevo_estado = FighterStatusEnum._saltando; // Y salta...
                            inc_y = -16; inc_x = 0;     // Inicializa los incrementos para el salto
                        }
                        if (Control(FighterActionEnum.Duck, this))
                        {    // Comprueba si se quiere agachar
                            nuevo_estado = FighterStatusEnum._agachandose;  // Y se agacha...
                        }
                        if (Control(FighterActionEnum.Backward, this))
                        {  // Comprueba si se quiere retroceder
                            nuevo_estado = FighterStatusEnum._retrocediendo;// Y retrocede...
                        }
                        if (Control(FighterActionEnum.Foward, this))
                        {       // Comprueba si se quiere avanzar
                            nuevo_estado = FighterStatusEnum._avanzando;// Y avanza...
                        }
                        break;

                    case FighterStatusEnum._avanzando:                // Si se esta avanzando
                        View.Graph = anim1[animationStep++];        // Anima el grafico
                        if (animationStep == anim1.Length)
                        {
                            // Comprueba que no se ha llegado al final de la animacion
                            nuevo_estado = FighterStatusEnum._parado;   // Y si se ha llegado cambia de estado
                        }
                        if (View.Flip != FlipEnum.None)
                        {                // Mueve el munieco seg£n donde mire
                            Location.X += 4f;
                        }
                        else
                        {
                            Location.X -= 4;
                        }
                        // Comprueba si se quieren hacer otras acciones
                        if (Control(FighterActionEnum.Hit, this))
                        {          // Se quiere golpear
                            nuevo_estado = FighterStatusEnum._patada_normal;
                        }
                        if (Control(FighterActionEnum.Jump, this))
                        {             // Se quiere saltar
                            nuevo_estado = FighterStatusEnum._saltando;
                            inc_y = -16;
                            if (View.Flip != FlipEnum.None)
                            {
                                inc_x = 12;
                            }
                            else
                            {
                                inc_x = -12;
                            }
                        }
                        break;

                    case FighterStatusEnum._retrocediendo:            // Mira si se esta retrocediendo
                        View.Graph = anim2[animationStep++];        // Anima el grafico
                        if (animationStep == anim2.Length)
                        {   // Si se ha llegado al final
                            nuevo_estado = FighterStatusEnum._parado;   // Cambia de estado
                        }
                        if (View.Flip != FlipEnum.None)
                        {                  // Lo mueve seg£n a donde mire
                            Location.X -= 4;
                        }
                        else
                        {
                            Location.X += 4;
                        }
                        // Se mira si se quieren hacer otras acciones posibles
                        if (Control(FighterActionEnum.Hit, this))
                        {
                            nuevo_estado = FighterStatusEnum._patada_giratoria;
                            inc_y = -10;
                        }
                        if (Control(FighterActionEnum.Jump, this))
                        {
                            nuevo_estado = FighterStatusEnum._saltando;
                            inc_y = -16;
                            if (View.Flip != FlipEnum.None)
                            {
                                inc_x = -8;
                            }
                            else
                            {
                                inc_x = 8;
                            }
                        }
                        break;

                    case FighterStatusEnum._agachandose:  // Comprueba si se esta agachandose
                        View.Graph = anim3[animationStep++];        // Haz la animacion
                        if (animationStep == anim3.Length)
                        {   // Hasta que se llegue al final
                            nuevo_estado = FighterStatusEnum._agachado; // Y pasa al estado de agachado
                        }
                        break;

                    case FighterStatusEnum._agachado:         // Comprueba si esta agachado
                        View.Graph = anim4[0];     // Pone el grafico necesario
                        if (View.Flip != FlipEnum.None)
                        {
                            // Pone al grafico mirando al otro
                            if (Enemy.Location.X < Location.X)
                            {
                                View.Flip = FlipEnum.None;
                            }
                        }
                        else
                        {
                            if (Enemy.Location.X > Location.X)
                            {
                                View.Flip = FlipEnum.FlipX;
                            }
                        }

                        // Comprueba si se quiere hacer otras acciones
                        if (Control(FighterActionEnum.Hit, this))
                        {        // Comprueba si se quiere golpear
                            nuevo_estado = FighterStatusEnum._golpe_bajo;
                        }
                        if (!Control(FighterActionEnum.Duck, this))
                        {   // Si no quiere agacharse
                            nuevo_estado = FighterStatusEnum._levantandose; // Se pone levantandose
                        }
                        break;

                    case FighterStatusEnum._levantandose:     // Comprueba si esta levantandose
                        View.Graph = anim5[animationStep++];        // Animalo
                        if (animationStep == anim5.Length)
                        {  // Si se acabado
                            nuevo_estado = FighterStatusEnum._parado;   // Cambia de estado a parado
                        }
                        break;

                    case FighterStatusEnum._saltando:             // Comprueba si se esta saltando
                        View.Graph = anim6[animationStep++];    // Anima los graficos
                        if (animationStep > 4)
                        {         // Si se ha llegado al punto de la animacion
                            Location.X += inc_x;           // Mueve al munieco
                            Location.Y += inc_y * 3;
                            inc_y += 2;           // Cambia el incremento para que bote
                            if (Location.Y >= 440)
                            {
                                // Comprueba si ha tocado el suelo
                                Location.Y = 440;

                                Add(new Dust(Location.X, Location.Y));     // Crea polvo cuando cae
                                                                            // Mira si se quiere saltar otra vez
                                if (Control(FighterActionEnum.Jump, this))
                                {
                                    nuevo_estado = FighterStatusEnum._parado;
                                }
                                else
                                {
                                    nuevo_estado = FighterStatusEnum._agachandose;
                                }
                            }
                        }
                        if (Control(FighterActionEnum.Hit, this))
                        {    // Comprueba si se quiere golpear
                            nuevo_estado = FighterStatusEnum._patada_aerea;
                            nuevo_paso = animationStep;
                        }
                        break;

                    // Comprueba si se esta se hace el golpe bajo
                    case FighterStatusEnum._golpe_bajo:
                        View.Graph = anim7[animationStep++];        // Anima el grafico
                        if (animationStep == anim7.Length)
                        {   // Si se acaba la animacion
                            nuevo_estado = FighterStatusEnum._agachado; // Se pone en otro estado
                        }
                        if (View.Flip != FlipEnum.None)
                        {                 // Comprueba hacia donde mira
                            Location.X++;                    // Y se mueve un poco
                        }
                        else
                        {
                            Location.X--;
                        }

                        // Si se ha llegado al punto de la animacion correcto
                        if (animationStep == 2)
                        {
                            // Realiza sonido                            
                            turn06Sound.Play();

                            // Coge los puntos de control del punio

                            Add(new HitHitbox(this, View.GetCalculatedPoint(1), fuerza_golpe[0])); // Y comprueba si se ha tocado
                            if (fuerza_golpe[0] > 2)
                            { // Reduce la fuerza de golpe
                                fuerza_golpe[0]--;  // para la proxima vez
                            }
                        }
                        break;
                    // Comprueba si se esta haciendo la accion punietazo
                    case FighterStatusEnum._punietazo:
                        View.Graph = anim8[animationStep++];        // Anima el grafico
                        if (animationStep == anim8.Length)
                        {  // Hasta que se acabe la animacion
                            nuevo_estado = FighterStatusEnum._parado;   // Y cambia a un nuevo estado
                        }
                        if (View.Flip != FlipEnum.None)
                        {                // Comprueba hacia donde mira
                            Location.X++;                    // Y se mueve
                        }
                        else
                        {
                            Location.X--;
                        }
                        if (animationStep == 4)
                        {
                            // Si se ha llegado al punto de la animacion
                            // Realiza el golpe                                                            
                            turn07Sound.Play();

                            // Halla el punto donde se dara el golpe
                            Add(new HitHitbox(this, View.GetCalculatedPoint(1), fuerza_golpe[1])); // Comprueba si se da el golpe
                            if (fuerza_golpe[1] > 2)
                            {                // Quita fuerza al golpe
                                fuerza_golpe[1]--;
                            }
                        }
                        break;

                    case FighterStatusEnum._patada_giratoria: // Comprueba la patada giratoria como los anteriores golpes
                        View.Graph = anim9[animationStep++];
                        if (animationStep == anim9.Length)
                        {
                            nuevo_estado = FighterStatusEnum._parado;
                        }
                        Location.Y += inc_y; inc_y += 2;
                        if (Location.Y > 440)
                        {
                            Location.Y = 440;
                        }

                        if (animationStep == 5)
                        {
                            // Realiza sonido                            
                            turn08Sound.Play();


                            Add(new HitHitbox(this, View.GetCalculatedPoint(1), fuerza_golpe[2]));
                            if (fuerza_golpe[2] > 2)
                            {
                                fuerza_golpe[2]--;
                            }
                        }

                        break;

                    case FighterStatusEnum._patada_normal:    // Comprueba la patada normal
                        View.Graph = anim10[animationStep++];
                        if (animationStep == anim10.Length)
                        {
                            nuevo_estado = FighterStatusEnum._parado;
                        }
                        if (animationStep == 4)
                        {
                            // Realiza sonido                            
                            turn05Sound.Play();

                            Add(new HitHitbox(this, View.GetCalculatedPoint(1), fuerza_golpe[3]));
                            if (fuerza_golpe[3] > 2)
                            {
                                fuerza_golpe[3]--;
                            }
                        }
                        break;

                    case FighterStatusEnum._patada_aerea:     // Comprueba la patada aerea
                        View.Graph = anim11[animationStep++];
                        if (animationStep > 4)
                        {
                            Location.X += inc_x;
                            Location.Y += inc_y * 3;
                            inc_y += 2;
                            if (Location.Y >= 440)
                            {
                                Location.Y = 440;

                                Add(new Dust(Location.X, Location.Y));
                                if (Control(FighterActionEnum.Jump, this))
                                {
                                    nuevo_estado = FighterStatusEnum._parado;
                                }
                                else
                                {
                                    nuevo_estado = FighterStatusEnum._agachandose;
                                }
                            }
                        }
                        if (animationStep == 10 || animationStep == 19)
                        {
                            // Realiza sonido                            
                            turn09Sound.Play();

                            Add(new HitHitbox(this, View.GetCalculatedPoint(1), fuerza_golpe[4]));
                            if (fuerza_golpe[4] > 2)
                            {
                                fuerza_golpe[4]--;
                            }

                        }
                        break;

                    case FighterStatusEnum._tocado:   // Comprueba si el munieco ha sido tocado
                        View.Graph = anim12[animationStep++];       // anima el grafico
                        if (animationStep == anim12.Length)
                        {   
                            // Hasta que acabe
                            // Haz un sonido al azar de los que tiene
                            switch (Rand.Next(0, 3))
                            {
                                case 0:
                                    uah00Sound.Play();                                    
                                    break;
                                case 1:
                                    whimper2Sound.Play();                                    
                                    break;
                                case 2:
                                    whimper3Sound.Play();                                    
                                    break;
                            }
                            nuevo_estado = FighterStatusEnum._parado;   // Cambia de estado
                            Location.Y = 440;                  // Cae al suelo
                        }
                        Location.X += inc_x;      // Y mueve el munieco un poco
                                                  // Si se movia el munieco, lo frena
                        if (inc_x < 0)
                        { // Si se movia a la izquierda
                            inc_x++;   // Se frena
                        }
                        if (inc_x > 0)
                        {  // Si se movia a la derecha
                            inc_x--;   // Se frena
                        }
                        if (Location.Y < 440)
                        {    // Si estaba saltando
                            Location.Y += 8;      // Hace que baje
                        }
                        if (Location.Y >= 440)
                        {    // Si estaba por debajo del limite inferior
                            Location.Y = 440;     // Lo coloca
                        }
                        break;

                    case FighterStatusEnum._muerto:   // Comprueba si el munieco ha muerto
                        if (animationStep == 0)
                        {
                            // Hace el sonido de cuando muere                            
                            aaah01Sound.Play();
                        }
                        View.Graph = anim13[animationStep++];       // Anima el grafico hacia con una secuencia
                        if (animationStep == anim13.Length)
                        {  // Y la deja parada
                            animationStep--;
                        }
                        Location.X += inc_x;           // Mueve las coordenadas hasta cuadrarlas
                                                       // Como el caso anterior (tocado) frena el munieco
                        if (inc_x < 0)
                        {
                            inc_x++;
                        }
                        if (inc_x > 0)
                        {
                            inc_x--;
                        }
                        if (Location.Y < 440)
                        {
                            Location.Y += 8;
                        }
                        if (Location.Y >= 440)
                        {
                            Location.Y = 440;
                        }
                        break;

                }

                if (estado != nuevo_estado)
                { // Actualiza el estado del munieco
                    estado = nuevo_estado;
                    animationStep = nuevo_paso;        // Y el paso dentro de la animacion
                    nuevo_paso = 0;
                }

                if (Location.X < 60)
                {                 // Comprueba que no se ha salido de pantalla
                    Location.X = 60;                   // Y coloca el grafico si ha salido
                }
                if (Location.X > 900)
                {
                    Location.X = 900;
                }

                Frame();
            });
        }

        private bool Control(FighterActionEnum action, Fighter fighter)
        {
            float dist = 0;   // Distancia entre los muniecos

            // Comprueba que el juego no este en pausa
            if (Global2.GameState != 1) return false;

            // Lee la tecla dependiendo del tipo de control
            switch (fighter.tipo_control)
            {

                // Mira quien maneja el munieco
                case FighterControlEnum.Keyboard1:      // Si se controla con el teclado1

                    // Va mirando que accion se quiere hacer
                    // Y leyendo las teclas que se usan para ello
                    switch (action)
                    {        // Mira que accion se esta comprobando
                        case FighterActionEnum.Jump:

                            return Input.Key(Keys.Up);   // Devuelve TRUE si esta pulsada la tecla
                        case FighterActionEnum.Duck:
                            return Input.Key(Keys.Down);
                        case FighterActionEnum.Foward:
                            // Dependiendo hacia donde mire
                            if (fighter.View.Flip != FlipEnum.None)
                            {
                                return Input.Key(Keys.Right);    // Lee una tecla...
                            }
                            else
                            {
                                return Input.Key(Keys.Left);     // O la otra
                            }
                        case FighterActionEnum.Backward:
                            if (fighter.View.Flip != FlipEnum.None)
                            {
                                return Input.Key(Keys.Left);
                            }
                            else
                            {
                                return Input.Key(Keys.Right);
                            }
                        case FighterActionEnum.Hit:
                            return Input.Key(Keys.RControlKey);
                    }
                    break;
                case FighterControlEnum.Keyboard2:      // Se esta usando el teclado 2
                    switch (action)
                    {
                        // Se comprueba igual que antes.
                        case FighterActionEnum.Jump: return Input.Key(Keys.U);
                        case FighterActionEnum.Duck: return Input.Key(Keys.J);

                        case FighterActionEnum.Foward:
                            if (fighter.View.Flip != FlipEnum.None)
                            {
                                return Input.Key(Keys.K);
                            }
                            else
                            {
                                return Input.Key(Keys.H);
                            }

                        case FighterActionEnum.Backward:
                            if (fighter.View.Flip != FlipEnum.None)
                            {
                                return Input.Key(Keys.H);
                            }
                            else
                            {
                                return Input.Key(Keys.K);
                            }
                        case FighterActionEnum.Hit: return Input.Key(Keys.Q);

                    }
                    break;
                case FighterControlEnum.Computer:
                    // Lo maneja el ordenador
                    // Halla la distancia entre los muniecos
                    dist = Math.Abs(Fighter1.Location.X - Fighter2.Location.X);
                    switch (fighter.estado)
                    {
                        // Mira que se quiere comprobar
                        // Va mirando que accion se quiere hacer
                        // Y se comprueba si es conveniente
                        case FighterStatusEnum._parado:
                            switch (action)
                            {
                                case FighterActionEnum.Jump:
                                    // Si se esta a distancia correcta y...
                                    // da la suerte devuelve TRUE
                                    if (dist < 160 && Rand.Next(0, 35) == 0)
                                    {
                                        return true;
                                    }
                                    break;
                                case FighterActionEnum.Duck: // Igual para las demas acciones
                                    if (dist < 160 && Rand.Next(0, 25) == 0)
                                    {
                                        return true;
                                    }
                                    break;
                                case FighterActionEnum.Foward:
                                    if (dist > 400)
                                    {
                                        if (Rand.Next(0, 4) == 0)
                                        {
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        if (dist > 120 && Rand.Next(0, 8) == 0)
                                        {
                                            return true;
                                        }
                                    }
                                    break;
                                case FighterActionEnum.Backward:
                                    if (dist < 180 && Rand.Next(0, 25) == 0)
                                    {
                                        return true;
                                    }
                                    if (dist < 80 && Rand.Next(0, 5) == 0)
                                    {
                                        return true;
                                    }
                                    break;
                                case FighterActionEnum.Hit:
                                    if (dist < 180 && dist > 60 && Rand.Next(0, 5) == 0)
                                    {
                                        return true;
                                    }
                                    break;
                            }
                            break;
                        case FighterStatusEnum._avanzando:
                            switch (action)
                            {
                                case FighterActionEnum.Jump:
                                    if (dist > 180 && dist < 300 && Rand.Next(0, 8) == 0)
                                    {
                                        return true;
                                    }
                                    break;
                                case FighterActionEnum.Hit:
                                    if (dist < 140)
                                    {
                                        return true;
                                    }
                                    break;
                            }
                            break;
                        case FighterStatusEnum._retrocediendo:
                            switch (action)
                            {
                                case FighterActionEnum.Jump:
                                    if (dist < 120 && Rand.Next(0, 6) == 0)
                                    {
                                        return true;
                                    }
                                    break;
                                case FighterActionEnum.Hit:
                                    if (dist > 70 && dist < 150)
                                    {
                                        return true;
                                    }
                                    break;
                            }
                            break;
                        case FighterStatusEnum._agachado:
                            switch (action)
                            {
                                case FighterActionEnum.Duck:
                                    if (Rand.Next(0, 5) != 0)
                                    {
                                        return true;
                                    }
                                    break;
                                case FighterActionEnum.Hit:
                                    if (dist < 180 && Rand.Next(0, 5) == 0)
                                    {
                                        return true;
                                    }
                                    break;
                            }
                            break;
                        case FighterStatusEnum._saltando:
                            switch (action)
                            {
                                case FighterActionEnum.Hit:
                                    if (Rand.Next(0, 10) == 0)
                                    {
                                        return true;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
            // Si la accion es correcta se devuelve 1, si no 0
            return false;  // No hace ninguna accion	            
        }

        class HitHitbox : EntityBase<EllipseView>
        {
            private readonly Fighter fighter;
            private readonly int damage;
#if DEBUG
            private bool counter = true;
#endif
            private IMultipleCollisionQuery<Fighter> collisions;

            /// <summary>
            /// Sonidos de golpes.
            /// </summary>
            private ISoundHandler hit00Sound, hit01Sound, hit02Sound;

            public HitHitbox(Fighter fighter, PointF location, int damage) 
                : base(new EllipseView(10, 10, Color.Red))
            {
                this.fighter = fighter;
                this.damage = damage;
                this.Location.X = location.X;
                this.Location.Y = location.Y;
            }

            protected override void OnStart(EntityStartHandler h)
            {
                base.OnStart(h);
                h.CollisionType = Collision2DTypeEnum.Auto;
                this.collisions = h.GetCollisions2D<Fighter>();

                hit00Sound = h.Audio.GetWaveHandler("Sound.HIT00.WAV");
                hit01Sound = h.Audio.GetWaveHandler("Sound.HIT01.WAV");
                hit02Sound = h.Audio.GetWaveHandler("Sound.HIT02.WAV");
            }

            protected override void OnFrame()
            {
#if DEBUG
                View.Scale = new PointF(10, 10);
                Location.Z = -100000000;          // Lo pone por delante de los muniecos
#else
                Location.Z = 10000;               // Lo pone por delante de los muniecos
#endif
                if (collisions.OnCollision(out Fighter[] fighters))
                {
                    foreach (var contact in fighters)
                    {
                        // Y que no sea el mismo que llamo a este proceso.                    
                        if (!fighter.Equals(contact))
                        {
                            var enemy = contact;

                            // Hace un sonido al azar de los disponibles
                            switch (Rand.Next(0, 2))
                            {
                                case 0:                                    
                                    hit00Sound.Play();
                                    break;
                                case 1:
                                    hit01Sound.Play();
                                    break;
                                case 2:
                                    hit02Sound.Play();
                                    break;
                            }
                            enemy.animationStep = 0;               // Actualiza la animacion del que ha sido tocado
                            enemy.Health -= damage / 2;      // Le quita energia
#if DEBUG
                            Trace.WriteLine($"{enemy.Id} : {enemy.Health}");
#endif
                            if (enemy.Health <= 0)       // Si no le queda energia
                            {
                                enemy.Health = 0;        // Es que esta muerto
                                enemy.estado = FighterStatusEnum._muerto;
                                Global2.GameState = 2;

                            }
                            else
                            {
                                // Si no, esta simplemente tocado
                                enemy.estado = FighterStatusEnum._tocado;
                            }

                            // Mueve el grafico un poco para atras
                            if (enemy.View.Flip == FlipEnum.FlipX)
                            {
                                enemy.inc_x = -8;
                            }
                            else
                            {
                                enemy.inc_x = 8;
                            }

                            // Crea la sangre
                            switch (BloodLevel)
                            {
                                // Dependiendo del nivel elegido en las opciones
                                case 0:
                                    // Golpe sin sangre
                                    Add(new NoBloodPunch(Location.X, Location.Y));
                                    break;
                                case 1:
                                    {
                                        // Golpe con sangre
                                        var parts = damage / 3 + 1;

                                        for (int i = 0; i < parts; i++)
                                        {
                                            Add(new BloodParticle(Location.X, Location.Y, enemy.inc_x + Rand.Next(-2, 2), Rand.Next(-4, 0), Rand.Next(10, 20)));
                                        }
                                    }
                                    break;
                                case 2:
                                    {
                                        // Golpe con mucha sangre
                                        var parts = damage + 2;

                                        for (int i = 0; i < parts; i++)
                                        {
                                            Add(new BloodParticle(Location.X, Location.Y, enemy.inc_x * 2 + Rand.Next(-4, 4), Rand.Next(-8, 2), Rand.Next(15, 30)));
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }                
#if DEBUG
                if (counter)
                {
                    counter = false;
                }
                else
                {
#endif
                    Die();
#if DEBUG
                }
#endif
            }
        }
    }
}