﻿using Kriptok.Audio;
using Kriptok.Entities.Base;
using Kriptok.Views.Sprites;
using System;

namespace Pong.Entities
{
    class Ball : EntityBase
    {
        private const int minY = 8;
        private const int maxY = 192;
        private const int minX = 10;
        private const int maxX = 310;

        private Racket leftRacket;
        private Racket rightRacket;
        private int xMod;
        private int yMod;
        private float speed = 1f;
        private const int halfRacketHeight = Racket.Height / 2;

        /// <summary>
        /// Sonidos.
        /// </summary>
        private ISoundHandler scoreSound, racketSound;

        public Ball(Racket leftRacket, Racket rightRachet, int xMod = -1, int yMod = -1)
            :base(new SpriteView(typeof(Ball).Assembly, "Ball.png"))
        {
            this.xMod = xMod;
            this.yMod = yMod;

            this.leftRacket = leftRacket;
            this.rightRacket = rightRachet;
        }

        protected override void OnStart(EntityStartHandler h)
        {
            Location.X = h.RegionSize.Width / 2;
            Location.Y = h.RegionSize.Height / 2;

            this.scoreSound = h.Audio.GetSoundHandler("Sound.Attack1.wav");
            this.racketSound = h.Audio.GetSoundHandler("Sound.Cursor1.wav");
        }

        protected override void OnFrame()
        {
            Location.X += xMod * speed;
            Location.Y += yMod * speed;

            if (Location.Y < minY)
            {
                Location.Y = minY;
                yMod = -yMod;
                racketSound.Play();
                IncSpeed();
            }

            if (Location.Y > maxY)
            {
                Location.Y = maxY;
                yMod = -yMod;
                racketSound.Play();
                IncSpeed();
            }

            if (Location.X < minX)
            {
                var dist = (int)Math.Abs(leftRacket.Location.Y - Location.Y);
                if (dist < halfRacketHeight)
                {
                    leftRacket.Points += halfRacketHeight - dist;
                    xMod = -xMod;
                    Location.X = minX;
                    IncSpeed();
                    racketSound.Play();
                }
            }

            if (Location.X < 0)
            {
                rightRacket.Points += 1000;
                Add(new Ball(leftRacket, rightRacket, xMod, yMod));
                scoreSound.Play();
                Die();
                return;
            }

            if (Location.X > maxX)
            {
                var dist = (int)Math.Abs(rightRacket.Location.Y - Location.Y);
                if (dist < halfRacketHeight)
                {
                    rightRacket.Points += halfRacketHeight - dist;
                    xMod = -xMod;
                    Location.X = maxX;
                    IncSpeed();
                    racketSound.Play();
                }
            }

            if (Location.X > 320)
            {
                leftRacket.Points += 1000;
                Add(new Ball(leftRacket, rightRacket, xMod, yMod));
                scoreSound.Play();                
                Die();
                return;
            }
        }

        private void IncSpeed()
        {
            speed *= 1.1f;
        }
    }
}
