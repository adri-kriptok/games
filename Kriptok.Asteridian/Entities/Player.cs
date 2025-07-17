using Kriptok.Asteridian.Scenes.Base;
using Kriptok.Asteridian.Views.Ships;
using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Entities.Collisions;
using Kriptok.Entities.Queries;
using Kriptok.Helpers;
using Kriptok.Views.Primitives;
using Kriptok.Views.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Asteridian.Entities
{
    class Player : EntityBase<PolygonView>
    {
        //public WeaponBase FrontGun;
        //public WeaponBase RearGun;

        private int nextShotCounter;

        //private ControllerType controller;

        /*
        private int xModifier;
        private int yModifier;
        */
        public LevelSceneBase Level;
        public float Speed = 1f;

        private readonly int MaxSpeed = 24;
        private readonly int MinSpeed = 6;
        public double RotationX;

        public bool DemoMode;

        public float ModifierY = 0;
        //private ShipShadow shadow;
        private IMouseLocationQuery mouseLocationQuery;
        // private ISoundHandler shootSound;

        public Player(LevelSceneBase level)
            : base(new TriangleShipView(Color.Yellow))
        {
            Location.Z = GlobalConsts.ZLevel.StandarAir;

            Level = level;
            Angle.Z = -MathHelper.HalfPIF;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            base.OnStart(h);
            h.CollisionType = Collision2DTypeEnum.Auto;

            //this.shadow = Add(new ShipShadow(this));
            this.mouseLocationQuery = h.GetMouseScrollLocationQuery();

            // this.shootSound = h.Audio.GetSoundHandler("Resources.Sound.Weapons.Shot1.wav");
        }

        //public void SetProperties(ShipInformation shipInformation)
        //{
        //    FrontGun = shipInformation.FrontGun;
        //    RearGun = shipInformation.RearGun;
        //    controller = shipInformation.Controller;
        //}

        protected override void OnFrame()
        {
            if (nextShotCounter == 0)
            {
                if (Mouse.Left /*|| InputController.Instance.Space() || DemoMode*/)
                {
                    if (!DemoMode)
                    {
                        //shootSound.Play();
                    }

                    //FrontGun.ShootFront(this);

                    //if (RearGun != null)
                    //{
                    //    RearGun.ShootRear(this);
                    //}

                    // Add(new BasicCannonShot(-MathHelper.HalfPIF - 0.5f, Location.X - 20, Location.Y - 20, 0, 0));
                    // Add(new BasicCannonShot(-MathHelper.HalfPIF - 0.25f, Location.X - 10, Location.Y - 30, 0, 0));
                    // Add(new BasicCannonShot(-MathHelper.HalfPIF, Location.X, Location.Y - 30, 0, 0));
                    // Add(new BasicCannonShot(-MathHelper.HalfPIF + 0.25f, Location.X + 10, Location.Y - 30, 0, 0));
                    // Add(new BasicCannonShot(-MathHelper.HalfPIF + 0.5f, Location.X + 20, Location.Y - 20, 0, 0));
                }
                nextShotCounter = 3;
            }
            else
            {
                nextShotCounter--;
            }

            // View.Graph = GetBitmapIndex();
        }

        internal float UpdateY()
        {
            CheckForMovement();

            Location.Y = Level.GetLocationY() + ModifierY;
            //shadow.Update();

            return (Location.X - GlobalConsts.PlayerShip_MidX) * 0.1f + GlobalConsts.PlayerShip_MidX;
        }

        //public override Vector3F GetPosition()
        //{
        //    //return base.GetPosition();
        //    var pos = base.GetPosition();
        //    return new Vector3F(pos.X, Level.Y + ModifierY, pos.Z);
        //}

        private void CheckForMovement()
        {
            if (Speed > MinSpeed)
            {
                Speed -= 2;
            }
            bool acelerating = Speed < MaxSpeed;

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
            else
            {
                if (((int)RotationX % 16 == 0))
                {
                    RotationX = 0d;
                }
                else
                {
                    RotationX += -RotationX / Math.Abs(RotationX) * 0.2;
                }
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
            Location.X += speed; Acelerate(acelerating);
            RotationX -= 0.25;
            if (RotationX == -9) RotationX = 7;
        }

        private void MoveLeft(bool acelerating, float speed)
        {
            Location.X -= speed; Acelerate(acelerating);
            RotationX += 0.25;
            if (RotationX == 9) RotationX = -7;
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
            if (acelerating) Speed += 4;
        }

        //private void Modify(int sign, ref int modifier)
        //{
        //    if (Math.Sign(modifier) != sign)
        //    {
        //        modifier = -modifier / 3;
        //    }
        //    if (Math.Abs(modifier) < 30)
        //    {                
        //        modifier += sign * 2;
        //    }
        //    //coord += modifier;
        //}

        //public override void SetViews()
        //{
        //    var view = new ThirdShipView();
        //    if (KryrianController.Instance.Options.ShadowsOn && Level != null && Level.HasShadows)
        //    {
        //        AddViews(/*view.CreateShadowView(),*/ view);
        //    }
        //    else
        //    {
        //        AddViews(view);
        //    }
        //}    

        public int GetBitmapIndex()
        {
            var rot = (int)RotationX;
            if (rot > 0)
            {
                return rot - (rot / 16) * 16;
            }
            else if (rot < 0)
            {
                return rot + ((Math.Abs(rot + 1) / 16) + 1) * 16;
            }
            else
            {
                return 0;
            }
        }
    }
}
