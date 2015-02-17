using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class Enemy: Sprite
    {

        public Enemy(Texture2D spriteTexture, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int timePerFrame)
            : base(spriteTexture, position,
                frameSize, collisionOffset, currentFrame, sheetSize, speed, timePerFrame)
        {
        }

        public override Vector2 Direction
        {
            get { return speed; }
        }

        public override void Update(GameTime gameTime, Rectangle clientRect)
        {
            position += Direction;

            base.Update(gameTime, clientRect);
        }


    }
}
