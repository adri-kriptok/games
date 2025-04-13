using Kriptok.Scenes;
using Kriptok.Drawing.Algebra;
using Kriptok.Extensions;
using Billiard.Scenes;
using Kriptok.Helpers;
using Kriptok.Entities;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System.Drawing;
using Kriptok.Audio;

namespace Billiard.Entities
{
    /// <summary>
    /// Proceso palo
    /// Controla el palo
    /// </summary>
    public class Stick : Process2Base<SpriteView>
    {
        private const float startAngle = -MathHelper.PIF * 0.5f;

        public float lastMouseLocation;   // Ultima posicion del raton

        /// <summary>
        /// Coordenadas reales del raton.
        /// </summary>
        private Point mouse;

        /// <summary>
        /// Para coger la distancia.
        /// </summary>
        public float distance;

        /// <summary>
        /// Bola que está jugando actualmente Blanca/Amarilla.
        /// </summary>
        private readonly Ball currentBall;

        private StickShadow shadow;


        private ISoundHandler bandSound;

        public Stick(Ball currentBall) : base(new SpriteView(typeof(Stick).Assembly, "Assets.Stick.png"))
        {
            View.Center = new PointF(0f, 0.5f);

            Location.Z = -99;
            this.currentBall = currentBall;
        }

        protected override void OnStart(ProcessStartHandler h)
        {
            base.OnStart(h);

            bandSound = h.Audio.GetWaveHandler(Global.BandaSound);
        }

        protected override void OnBegin(ProcessHandler h)
        {
            // Crea la sombra del palo.
            shadow = Add(new StickShadow(this));      

            // Imprime el modo de juego  
            Global.Mode = GameModeEnum.PointAt;

            Mouse.CenterXY();
            
            lastMouseLocation = Mouse.X;
            
            // Coge las coordenadas respecto a la bola que seniala
            Location.X = currentBall.Location.X;
            Location.Y = currentBall.Location.Y;
            
            Angle.Z = startAngle;    // Angulo inicial
                                // Posicion inicial de los efectos
            Global.EffectObject.Location.X = Effect.StartX;
            Global.EffectObject.Location.Y = Effect.StartY;

            Loop(() =>
            {
                // Esta en el modo de apuntar con el taco
                if (Global.Mode == GameModeEnum.PointAt)
                {
                    // Actualiza la posicion del taco con la del raton
                    Angle.Z += (lastMouseLocation - Mouse.X) * (MathHelper.PIF / 256f);
                    lastMouseLocation = Mouse.X;
                    
                    // Hace que el raton este entre 512 y 512
                    if (Mouse.X < 128)
                    {
                        lastMouseLocation = Mouse.X;
                    }

                    if (Mouse.X > 512)
                    {
                        lastMouseLocation = Mouse.X;
                    }

                    // Comprueba si se ha pulsado el boton del raton
                    if (Mouse.Left)
                    {
                        Global.Mode = GameModeEnum.Effect;

                        // Centra el raton
                        Mouse.CenterXY();

                        // Espera a que se suelte el boton del raton
                        h.Repeat(() =>
                        {
                            Frame();
                        }, () => !Mouse.Left);

                        // Cambia el modo de tratar el taco
                        Global.Mode = GameModeEnum.Effect;

                        // Coge las coordenadas del raton y las guarda en variables propias
                        mouse = new Point(Mouse.X, Mouse.Y);                        
                    }
                    else
                    {
                        // Si no se pulsa el raton crea el visor
                        Add(new Aim(currentBall, Angle.Z + MathHelper.PIF));
                    }
                }

                // Modo en el que se selecciona efecto
                if (Global.Mode == GameModeEnum.Effect)
                {
                    // Actualiza el afecto dependiendo de las coordenadas del raton
                    Global.EffectObject.Location.X = Effect.StartX + (Mouse.X - mouse.X) / 3;
                    Global.EffectObject.Location.Y = Effect.StartY + (Mouse.Y - mouse.Y) / 3;

                    // Coge la distancia del centro de la bola a la posicion del efecto
                    distance = MathHelper.GetDistanceF(
                        Effect.StartX,
                        Effect.StartY,
                        Global.EffectObject.Location.X,
                        Global.EffectObject.Location.Y);

                    // Si la longitud es mayor que 22 la trunca para hacerlo redondo
                    if (distance > 22f)
                    {
                        var ang = MathHelper.GetAngleF(
                            Effect.StartX, Effect.StartY,
                            Global.EffectObject.Location.X, Global.EffectObject.Location.Y);
                        Global.EffectObject.Location.X = Effect.StartX + PolarVector.ProjectX(ang, 22);
                        Global.EffectObject.Location.Y = Effect.StartY + PolarVector.ProjectY(ang, 22);
                    }

                    // Mira si se pulsa el boton del raton
                    if (Mouse.Left)
                    {
                        Global.Mode = GameModeEnum.Shoot;

                        // Centra el raton
                        Mouse.CenterXY();

                        // Espera a que se suelte el boton del raton
                        h.Repeat(() =>
                        {
                            Frame();
                        }, () => !Mouse.Left);

                        // Actualiza las variables del proceso
                        lastMouseLocation = Mouse.Y;
                        mouse.X = Location.X.Round();
                        mouse.Y = Location.Y.Round();

                        // Cambia el modo de accion del taco
                        Global.Mode = GameModeEnum.Shoot;

                        // Guarda los efectos elegidos en sus variables
                        Global.EffectModifier.Y = Global.EffectObject.Location.Y - Effect.StartY;
                        if (Global.EffectModifier.Y < 0f)
                        {
                            Global.EffectModifier.Y = 80f - 28f * -Global.EffectModifier.Y / 22f;
                        }
                        else
                        {
                            Global.EffectModifier.Y = 80f + Global.EffectModifier.Y * 120f / 22f;
                        }
                        Global.EffectModifier.X = Global.EffectObject.Location.X - Effect.StartX;
                    }
                }

                // Modo de taco de seleccion de fuerza
                if (Global.Mode == GameModeEnum.Shoot)
                {
                    // Coloca el taco en relacion a las coordenadas del raton
                    Location.X = mouse.X + PolarVector.ProjectX(Angle.Z, Mouse.Y - 240f);
                    Location.Y = mouse.Y + PolarVector.ProjectY(Angle.Z, Mouse.Y - 240f);

                    // Si se pasa de un limite significa que se ha hecho el tiro
                    if (Mouse.Y < 232)
                    {
                        // Borra el texto de modo                        
                        Global.Mode = GameModeEnum.None;

                        // Hace un sonido de rebote.                        
                        bandSound.Play();

                        // Halla la fuerza, dependiendo de la ultima posicion del raton
                        // Comprueba si la fuerza se pasa de los limites permitidos                        
                        // Guarda la fuerza como velocidad de la bola
                        currentBall.Speed = ((lastMouseLocation - Mouse.Y) * 100).Clamp(200, 8000);

                        // Y pone el angulo apropiado a la bola
                        currentBall.MovementAngle = Angle.Z + MathHelper.PIF;

                        // Borra todo y se prepara para salir del proceso
                        Global.LastBall = null;

                        Frame();
                        Die();
                        shadow.Die();
                    }

                    // Comprueba si se pulsa el boton del raton
                    if (Mouse.Left)
                    {
                        // Borra el texto antiguo y pone el nuevo                        
                        Global.Mode = GameModeEnum.PointAt;

                        // Actualiza la posicion del taco al lado de la bola
                        Location.X = currentBall.Location.X;
                        Location.Y = currentBall.Location.Y;

                        // Centra el raton                        
                        Mouse.CenterXY();

                        // Espera hasta que se suelte el boton del raton
                        h.Repeat(() =>
                        {
                            Frame();
                        }, () => !Mouse.Left);

                        // Cambia de modo
                        lastMouseLocation = Mouse.X;

                        Global.Mode = GameModeEnum.PointAt;
                    }
                    else
                    {
                        // Actualiza la ultima posicion del raton
                        lastMouseLocation = Mouse.Y;
                    }
                }

                Frame();
            });
            
            shadow.Die();
        }
    }
}
