using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class Megaman : Sprite 
    {

        public List<Bullet> bullets = new List<Bullet>();
        Texture2D bulletTexture;
        SpriteManager manager;

        public Megaman(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int timePerFrame, Texture2D bulletTexture,SpriteManager manager): base(textureImage, position, frameSize, collisionOffset,
            currentFrame, sheetSize, speed, timePerFrame)
        {
            this.manager = manager;
            this.bulletTexture = bulletTexture;
        }

        public override Vector2 Direction
        {
            get
            {
                //throw new NotImplementedException(); 
                Vector2 inputDirection = Vector2.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.W)) { inputDirection.Y-=3; }
                if (Keyboard.GetState().IsKeyDown(Keys.S)) { inputDirection.Y +=3; }

                return inputDirection * speed;
            }
        }

        public override void Update(GameTime gameTime, Rectangle client)
        {
            position += Direction;

            //Keeps Megaman within the bounds of the window
            if (position.X < 0) position.X = 0;
            if (position.Y < 0) position.Y = 0;
            if (position.X > client.Width) position.X = client.Width;
            if (position.Y > client.Height-128) position.Y = client.Height-128;

            base.Update(gameTime, client);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            base.Draw(gameTime, spriteBatch);
        }

        public Vector2 Position
        {
            get
            {
                return this.position;
            }
        }

    }
}
