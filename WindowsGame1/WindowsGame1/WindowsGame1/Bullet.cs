using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class Bullet
    {
        //Instance variables
        List<Vector2> bullets = new List<Vector2>();
        Texture2D bulletTexture;
        float bulletSpeed = 400f;
        protected Vector2 position;
        protected Vector2 speed;
        protected Point frameSize;
        Point currentFrame;
        Point sheetSize;
        int timeSinceLastFrame = 0;
        int timePerFrame;
        int collisionOffset;

        public Bullet(Texture2D bulletTexture, Vector2 position, Vector2 speed, Point frameSize, Point currentFrame,
            Point sheetSize, int timePerFrame, int collisionOffset)
        {
            this.bulletTexture = bulletTexture;
            this.position = position;
            this.speed = speed;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.timePerFrame = timePerFrame;
            this.collisionOffset = collisionOffset;
        }

        public void Update(GameTime gameTime, Rectangle client)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame > timePerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    if (currentFrame.Y >= sheetSize.Y) currentFrame.Y = 0;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                bullets.Add(position);

            for (int i = 0; i < bullets.Count; i++)
            {
                float x = bullets[i].X + 10f;
                x += bulletSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                bullets[i] = new Vector2(x, bullets[i].Y);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Vector2 b in bullets)
            {
                spriteBatch.Draw(bulletTexture, new Vector2 (b.X+70, b.Y+20), new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X,
                frameSize.Y), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
        }

        public Vector2 getPosition
        {
            get
            {
                return this.position;
            }
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                foreach (Vector2 b in bullets)
                {
                    return new Rectangle((int)b.X + 10, (int)b.Y,
                        frameSize.X, frameSize.Y);
                }
                return new Rectangle((int)position.X+30, (int)position.Y, frameSize.X, frameSize.Y);
            }
        }

    }
}
