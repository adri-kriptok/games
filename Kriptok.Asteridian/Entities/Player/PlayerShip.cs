using Kriptok.Asteridian.Entities.Player.Weapons.Base;
using Kriptok.Asteridian.Entities.Player.Weapons.Front;
using Kriptok.Asteridian.Regions;
using Kriptok.Asteridian.Scenes.Base;
using Kriptok.Asteridian.Views.Ships;
using Kriptok.Audio;
using Kriptok.Drawing.Algebra;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Queries;
using Kriptok.Extensions;
using Kriptok.Helpers;
using Kriptok.Views.Primitives;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Kriptok.Asteridian.Scenes.Base.LevelSceneBase;

namespace Kriptok.Asteridian.Entities
{
    class PlayerShip : EntityBase<PolygonView>
    {
        private const float speedIncrement = 1f / 16f;
        private const float speedDecrement = 1f / 8f;
        private float Speed = 0.25f;

        public const float VerticalMax = 87.5f;
        public const float VerticalMin = -VerticalMax;


        internal FrontWeaponBase FrontGun;
        internal RearWeaponBase RearGun;

        public LevelSceneBase Level;

        private readonly int MaxSpeed = 30;
        private readonly int MinSpeed = 3;
        public bool DemoMode;

        public float ModifierY = 0;

        private CustomMouseLocationQuery mouseLocationQuery;

        public PlayerShip(LevelSceneBase level) : base(new TriangleShipView(Color.Yellow, Color.DarkOrange))
        {
            Location.Z = GlobalConsts.ZLevel.StandarAir;

            Level = level;
            Angle.Z = -MathHelper.HalfPIF;
        }

        /// <summary>
        /// Controlador de la cámara.
        /// </summary>
        internal AsteridianScrollTarget Camera;

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);

            h.CollisionType = Collision2DTypeEnum.Auto;

            //this.shadow = Add(new ShipShadow(this));
            this.mouseLocationQuery = new CustomMouseLocationQuery(h, Level);

            this.FrontGun = Add(new FrontLaserGun0(this));
            //this.FrontGun = Add(new FrontProtonGun9(this));
        }

        protected override void OnFrame()
        {
            UpdateY();

#if DEBUG
            if (Input.Key(Keys.Add)) FrontGun = FrontGun.GetLevelUp();            
            if (Input.Key(Keys.Subtract)) FrontGun = FrontGun.GetLevelDown();            
#endif
        }

        internal void UpdateY()
        {
            var newY = Camera.Inc(Sys.TimeDelta * 0.0625f);

            Location.Y = newY + ModifierY;
            //shadow.Update();

            CheckForMovement();
            Camera.SetX(Location.X * 0.1f);
        }

        //public override Vector3F GetPosition()
        //{
        //    //return base.GetPosition();
        //    var pos = base.GetPosition();
        //    return new Vector3F(pos.X, Level.Y + ModifierY, pos.Z);
        //}

        private void CheckForMovement()
        {
            //Location = new Vector3F(mouseLocationQuery.GetLocation(), Location.Z);

            Speed = Math.Max(MinSpeed, Speed - Sys.TimeDelta * speedDecrement);

            var acelerating = Speed < MaxSpeed;

            CheckMouseMovement(acelerating);
        }

        private void CheckMouseMovement(bool acelerating)
        {
            var mouseCoords = mouseLocationQuery.Result;

            if (mouseCoords.X > Location.X)
            {
                MoveRight(acelerating, Math.Min(mouseCoords.X - Location.X, Speed));
            }
            else if (mouseCoords.X < Location.X)
            {
                MoveLeft(acelerating, Math.Min(Location.X - mouseCoords.X, Speed));
            }
          
            if (mouseCoords.Y < Location.Y)
            {
                MoveUp(acelerating, Math.Min(Location.Y - mouseCoords.Y, Speed));
            }
            else if (mouseCoords.Y > Location.Y)
            {
                MoveDown(acelerating, Math.Min(mouseCoords.Y - Location.Y, Speed));
            }
        }

        private void MoveRight(bool acelerating, float speed)
        {
            Location.X += speed; 
            Acelerate(acelerating);
        }

        private void MoveLeft(bool acelerating, float speed)
        {
            Location.X -= speed; 
            Acelerate(acelerating);
        }

        private void MoveUp(bool acelerating, float speed)
        {
            ModifierY -= speed;
            Acelerate(acelerating);
        }

        private void MoveDown(bool acelerating, float speed)
        {
            ModifierY += speed;
            Acelerate(acelerating);
        }

        private void Acelerate(bool acelerating)
        {
            if (acelerating)
            {
                Speed += Sys.TimeDelta * speedIncrement;
            }
        }
    }

    internal class CustomMouseLocationQuery : ItemBase, IMouseLocationQuery
    {
        private readonly LevelSceneBase level;
        private readonly float halfScreenWidth;
        private readonly float halfScreenHeight;
        private readonly float minY;
        private readonly float relY;

        public CustomMouseLocationQuery(EntityStartHandler h, LevelSceneBase level)
        {
            this.level = level;
            this.halfScreenWidth = GlobalConsts.ScreenSize.Width * 0.5f;
            this.halfScreenHeight = GlobalConsts.ScreenSize.Height * 0.5f;

            this.minY = halfScreenHeight + PlayerShip.VerticalMin;
            this.relY = ((halfScreenHeight + PlayerShip.VerticalMax) - minY) / GlobalConsts.ScreenSize.Height;
        }

        public Vector2F Result
        {
            get
            {
                var mouseY = (Mouse.Y * relY) + minY;

                return new Vector2F((Mouse.X - halfScreenWidth) * 0.8f, level.GetLocationY() + mouseY - halfScreenHeight);
            }
        }

        internal Vector2F GetLocation()
        {
            var mouseY = (Mouse.Y * relY) + minY;

            return new Vector2F((Mouse.X - halfScreenWidth) * 0.8f, level.GetLocationY() + mouseY - halfScreenHeight);
        }
    }
}
